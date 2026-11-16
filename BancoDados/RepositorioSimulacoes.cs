using System.Text;
using MySqlConnector;

namespace SimuladorGavitacional
{
    public class RepositorioSimulacoes : GravadorDados
    {
        //string de conexao banco de dados MySQL
        private readonly string stringConexao;

        public RepositorioSimulacoes(string stringConexao)
        {
            this.stringConexao = stringConexao;
        }

        //conecta o banco  
        private MySqlConnection NovaConexao()
        {
            return new MySqlConnection(stringConexao);
        }

        //funcao que cria o banco de dados e as tabelas necessarias, se nao existir
        public async Task InicializarBancoAsync()
        {
            //conecta ao servidor MySQL mesmo que o banco não exista ainda
            MySqlConnectionStringBuilder dadosServidor = new MySqlConnectionStringBuilder(stringConexao);
            dadosServidor.Database = "";

            await using (MySqlConnection conexaoServidor = new MySqlConnection(dadosServidor.ConnectionString))
            {
                await conexaoServidor.OpenAsync();

                //script de criacao do banco
                const string sqlCriarBanco =
                    "CREATE DATABASE IF NOT EXISTS simulador_gravitacional " +
                    "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";

                //roda o script
                await using (MySqlCommand comandoBanco = new MySqlCommand(sqlCriarBanco, conexaoServidor))
                {
                    await comandoBanco.ExecuteNonQueryAsync();
                }
            }
            await using (MySqlConnection conexao = NovaConexao())
            {
                await conexao.OpenAsync();

                //script de criacao da tabela simulacoes
                const string sqlSimulacoes =
                    "CREATE TABLE IF NOT EXISTS simulacoes (" +
                    "id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT, " +
                    "criada_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
                    "intervalo_ms INT UNSIGNED NOT NULL, " +
                    "delta_tempo_s DOUBLE NOT NULL, " +
                    "ultima_iteracao INT UNSIGNED NOT NULL DEFAULT 0, " +
                    "PRIMARY KEY (id)" +
                    ") ENGINE = InnoDB;";

                //roda os scripts
                await using (MySqlCommand comandoSimulacoes = new MySqlCommand(sqlSimulacoes, conexao))
                {
                    await comandoSimulacoes.ExecuteNonQueryAsync();
                }

                //script de criacao da tabela estados
                const string sqlEstados =
                    "CREATE TABLE IF NOT EXISTS estados_corpos (" +
                    "simulacao_id BIGINT UNSIGNED NOT NULL, " +
                    "iteracao INT UNSIGNED NOT NULL, " +
                    "indice_corpo INT UNSIGNED NOT NULL, " +
                    "nome VARCHAR(255) NOT NULL, " +
                    "massa_kg DOUBLE NOT NULL, " +
                    "densidade_kg_m3 DOUBLE NOT NULL, " +
                    "pos_x DOUBLE NOT NULL, " +
                    "pos_y DOUBLE NOT NULL, " +
                    "vel_x_m_s DOUBLE NOT NULL, " +
                    "vel_y_m_s DOUBLE NOT NULL, " +
                    "cor_argb INT NOT NULL, " +
                    "PRIMARY KEY (simulacao_id, iteracao, indice_corpo), " +
                    "CONSTRAINT fk_estados_simulacao " +
                    "FOREIGN KEY (simulacao_id) REFERENCES simulacoes(id) " +
                    "ON DELETE CASCADE" +
                    ") ENGINE = InnoDB;";

                //roda os scripts
                await using (MySqlCommand comandoEstados = new MySqlCommand(sqlEstados, conexao))
                {
                    await comandoEstados.ExecuteNonQueryAsync();
                }
            }
        }

        //testa a conexao com o banco
        public async Task TestarAsync()
        {
            await using MySqlConnection conexao = NovaConexao();
            await conexao.OpenAsync();
            await using MySqlCommand comando = new MySqlCommand("SELECT 1 FROM simulacoes LIMIT 1", conexao);
            await comando.ExecuteScalarAsync();
        }

        //sobrecarga do metodo abstrato que salva as simulacoes
        public override async Task<long> SalvarSimulacaoAsync(int intervaloMs, double deltaTempo, int ultimaIteracao, string caminhoArquivo)
        {
            await using MySqlConnection conexao = NovaConexao();
            await conexao.OpenAsync();

            //inicia a transacao
            await using MySqlTransaction transacao = await conexao.BeginTransactionAsync();

            //insere os dados da simulacao
            await using MySqlCommand comandoSimulacao = new MySqlCommand(
                "INSERT INTO simulacoes (intervalo_ms, delta_tempo_s) VALUES (@intervalo, @delta)", conexao, transacao);
            comandoSimulacao.Parameters.AddWithValue("@intervalo", intervaloMs);
            comandoSimulacao.Parameters.AddWithValue("@delta", deltaTempo);
            await comandoSimulacao.ExecuteNonQueryAsync();

            //pega o ultimo ID
            long id = comandoSimulacao.LastInsertedId;

            //insere os estados
            await InserirEstadosEmLotesAsync(conexao, transacao, id, caminhoArquivo);

            //atualiza a ultima iteracao     
            await using MySqlCommand comandoAtualizar = new MySqlCommand(
                "UPDATE simulacoes SET ultima_iteracao=@iteracao WHERE id=@id", conexao, transacao);
            comandoAtualizar.Parameters.AddWithValue("@iteracao", ultimaIteracao);
            comandoAtualizar.Parameters.AddWithValue("@id", id);
            await comandoAtualizar.ExecuteNonQueryAsync();

            //confirma a transacao
            await transacao.CommitAsync();
            return id;
        }

        //le o arquivo TXT local e insere os registros no banco em lotes de 200
        private static async Task InserirEstadosEmLotesAsync(MySqlConnection conexao, MySqlTransaction transacao, long simulacaoId, string caminhoArquivo)
        {
            //define o tamanho do lote 
            const int tamanhoLote = 200;

            //abre o arquivo txt que contem a simulacao
            using StreamReader leitor = new StreamReader(caminhoArquivo, Encoding.UTF8);


            List<string> valoresSql = new List<string>(tamanhoLote);

            //cria o comando de insercao
            using MySqlCommand comando = new MySqlCommand();
            comando.Connection = conexao;
            comando.Transaction = transacao;

            int iteracaoAtual = 0;
            int indiceNoLote = 0;
            string? linha;

            //le o arquivo
            while ((linha = await leitor.ReadLineAsync()) != null)
            {
                //ignora se a linha for nula ou vazia
                if (string.IsNullOrWhiteSpace(linha))
                {
                    continue;
                }

                string[] partes = linha.Split(';');

                //se for a linha de iteracao atualiza a variavel e pula para a proxima linha
                if (partes[0] == "ITERACAO")
                {
                    iteracaoAtual = int.Parse(partes[1]);
                    continue;
                }

                //ignora se nao for linha de corpo
                if (partes[0] != "CORPO")
                {
                    continue;
                }

                //converte os dados do txt para o tipo certo
                int indiceCorpo = int.Parse(partes[1]);
                string nome = partes[2];
                double massa = double.Parse(partes[3]);
                double densidade = double.Parse(partes[4]);
                double posX = double.Parse(partes[5]);
                double posY = double.Parse(partes[6]);
                double velX = double.Parse(partes[7]);
                double velY = double.Parse(partes[8]);
                int corArgb = int.Parse(partes[9]);

                //cria os parametros para cada corpo
                string paramIteracao = "@it" + indiceNoLote;
                string paramIndice = "@ind" + indiceNoLote;
                string paramNome = "@nome" + indiceNoLote;
                string paramMassa = "@massa" + indiceNoLote;
                string paramDensidade = "@dens" + indiceNoLote;
                string paramPosX = "@x" + indiceNoLote;
                string paramPosY = "@y" + indiceNoLote;
                string paramVelX = "@vx" + indiceNoLote;
                string paramVelY = "@vy" + indiceNoLote;
                string paramCor = "@cor" + indiceNoLote;

                //adiciona a estrutura do registro na lista
                valoresSql.Add(
                    $"(@id, {paramIteracao}, {paramIndice}, {paramNome}, {paramMassa}, {paramDensidade}, {paramPosX}, {paramPosY}, {paramVelX}, {paramVelY}, {paramCor})");

                //associa os parametros ao comando
                comando.Parameters.AddWithValue(paramIteracao, iteracaoAtual);
                comando.Parameters.AddWithValue(paramIndice, indiceCorpo);
                comando.Parameters.AddWithValue(paramNome, nome);
                comando.Parameters.AddWithValue(paramMassa, massa);
                comando.Parameters.AddWithValue(paramDensidade, densidade);
                comando.Parameters.AddWithValue(paramPosX, posX);
                comando.Parameters.AddWithValue(paramPosY, posY);
                comando.Parameters.AddWithValue(paramVelX, velX);
                comando.Parameters.AddWithValue(paramVelY, velY);
                comando.Parameters.AddWithValue(paramCor, corArgb);

                indiceNoLote++;

                //executa quando o lote atinge o tamanho maximo
                if (indiceNoLote >= tamanhoLote)
                {
                    //executa o lote, limpa a lista e o comando e zera o indice
                    await ExecutarLoteAsync(comando, simulacaoId, valoresSql);
                    valoresSql.Clear();
                    comando.Parameters.Clear();
                    indiceNoLote = 0;
                }
            }

            //executa o ultimo lote se tiver sobrado algum
            if (valoresSql.Count > 0)
            {
                await ExecutarLoteAsync(comando, simulacaoId, valoresSql);
            }
        }

        //metodo para executar o lote
        private static async Task ExecutarLoteAsync(MySqlCommand comando, long simulacaoId, List<string> valoresSql)
        {
            //define os parametros
            comando.Parameters.AddWithValue("@id", simulacaoId);

            //define o comando em lote
            comando.CommandText =
                "INSERT INTO estados_corpos " +
                "(simulacao_id, iteracao, indice_corpo, nome, massa_kg, densidade_kg_m3, " +
                "pos_x, pos_y, vel_x_m_s, vel_y_m_s, cor_argb) VALUES " +
                string.Join(", ", valoresSql);

            //executa o comando
            await comando.ExecuteNonQueryAsync();
        }

        //metodo que lista as simulacoes salvas
        public override async Task<List<SimulacaoSalva>> ListarAsync()
        {
            //cria a lista das simulacoes
            List<SimulacaoSalva> resultado = new List<SimulacaoSalva>();

            //conecta no banco
            await using MySqlConnection conexao = NovaConexao();
            await conexao.OpenAsync();

            //cria o comando de busca
            await using MySqlCommand comando = new MySqlCommand(
                "SELECT id, criada_em, ultima_iteracao FROM simulacoes ORDER BY id DESC", conexao);

            //variavel que le a consulta
            await using MySqlDataReader leitor = await comando.ExecuteReaderAsync();

            //adiciona as simulacoes na lista
            while (await leitor.ReadAsync())
            {
                resultado.Add(new SimulacaoSalva(
                    checked((long)leitor.GetUInt64(0)),
                    leitor.GetDateTime(1),
                    leitor.GetInt32(2)));
            }
            return resultado;
        }


        public override async Task<(int IntervaloMs, double DeltaTempo, List<Quadro> Quadros)> CarregarAsync(long id)
        {
            //abre a conexao com o banco e consulta as configuracoes da simulacao
            await using MySqlConnection conexao = NovaConexao();
            await conexao.OpenAsync();
            await using MySqlCommand comandoMetadados = new MySqlCommand(
                "SELECT intervalo_ms, delta_tempo_s FROM simulacoes WHERE id=@id", conexao);

            //informa o identificador da simulacao que sera consultada
            comandoMetadados.Parameters.AddWithValue("@id", id);

            //declara as variaveis que armazenarao as configuracoes da simulacao
            int intervalo;
            double delta;

            //executa a consulta e verifica se a simulacao existe antes de recuperar seus dados
            await using (MySqlDataReader leitor = await comandoMetadados.ExecuteReaderAsync())
            {
                if (!await leitor.ReadAsync()) throw new InvalidOperationException("Simulação não encontrada.");
                intervalo = leitor.GetInt32(0);
                delta = leitor.GetDouble(1);
            }

            //consulta os estados dos corpos da simulacao, organizando os resultados por iteracao e indice do corpo
            const string sql =
                "SELECT iteracao, indice_corpo, nome, massa_kg, densidade_kg_m3, " +
                "pos_x, pos_y, vel_x_m_s, vel_y_m_s, cor_argb " +
                "FROM estados_corpos WHERE simulacao_id=@id ORDER BY iteracao, indice_corpo";

            //prepara a consulta e informa qual simulacao deve ser carregada
            await using MySqlCommand comando = new MySqlCommand(sql, conexao);
            comando.Parameters.AddWithValue("@id", id);

            //cria uma estrutura para agrupar os corpos de cada iteracao e guarda o tamanho necessario para os arrays
            SortedDictionary<int, Dictionary<int, Corpo>> grupos = new SortedDictionary<int, Dictionary<int, Corpo>>();
            int tamanho = 0;

            //executa a consulta e percorre todos os estados dos corpos encontrados
            await using MySqlDataReader dados = await comando.ExecuteReaderAsync();
            while (await dados.ReadAsync())
            {
                int iteracao = dados.GetInt32(0);
                int indice = dados.GetInt32(1);
                tamanho = Math.Max(tamanho, indice + 1);

                //procura o grupo da iteracao atual e cria um novo caso ainda nao exista
                if (!grupos.TryGetValue(iteracao, out Dictionary<int, Corpo>? grupo))
                {
                    grupo = new Dictionary<int, Corpo>();
                    grupos[iteracao] = grupo;
                }

                //recria o corpo com os dados recuperados do banco e armazena no indice correspondente
                grupo[indice] = new Corpo
                {
                    Nome = dados.GetString(2),
                    Massa = dados.GetDouble(3),
                    Densidade = dados.GetDouble(4),
                    PosX = dados.GetDouble(5),
                    PosY = dados.GetDouble(6),
                    VelX = dados.GetDouble(7),
                    VelY = dados.GetDouble(8),
                    Cor = Color.FromArgb(dados.GetInt32(9))
                };
            }

            //cria a lista que armazenara os quadros reconstruidos da simulacao
            List<Quadro> quadros = new List<Quadro>();

            //percorre os grupos de iteracoes para reconstruir os respectivos quadros
            foreach (KeyValuePair<int, Dictionary<int, Corpo>> grupo in grupos)
            {
                //cria um array para preservar as posicoes originais dos corpos, mantendo vazias as posicoes sem registros
                Corpo?[] corpos = new Corpo?[tamanho];

                //coloca cada corpo na sua posicao original dentro do array
                foreach (KeyValuePair<int, Corpo> item in grupo.Value)
                {
                    corpos[item.Key] = item.Value;
                }

                //cria um quadro com o numero da iteracao e os corpos recuperados
                quadros.Add(new Quadro(grupo.Key, corpos));
            }

            //retorna as configuracoes e os quadros recuperados para reproduzir a simulacao
            return (intervalo, delta, quadros);
        }

    }
}
