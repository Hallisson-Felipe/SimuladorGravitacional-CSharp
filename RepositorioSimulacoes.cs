using System.Text;
using MySqlConnector;

namespace SimuladorGavitacional
{
    public class SimulacaoSalva
    {
        public long Id { get; set; }
        public DateTime CriadaEm { get; set; }
        public int UltimaIteracao { get; set; }

        public SimulacaoSalva(long id, DateTime criadaEm, int ultimaIteracao)
        {
            Id = id;
            CriadaEm = criadaEm;
            UltimaIteracao = ultimaIteracao;
        }

        public override string ToString()
        {
            return $"#{Id} — {CriadaEm:dd/MM/yyyy HH:mm} — {UltimaIteracao} iterações";
        }
    }

    public class Quadro
    {
        public int Iteracao { get; set; }
        public Corpo?[] Corpos { get; set; }

        public Quadro(int iteracao, Corpo?[] corpos)
        {
            Iteracao = iteracao;
            Corpos = corpos;
        }
    }

    //cada quadro conserva os indices originais; apos uma colisao, o corpo absorvido
    //simplesmente deixa de ter linha nos quadros seguintes
    public class RepositorioSimulacoes : GravadorDados
    {
        private readonly string stringConexao;

        public RepositorioSimulacoes(string stringConexao)
        {
            this.stringConexao = stringConexao;
        }

        private MySqlConnection NovaConexao()
        {
            return new MySqlConnection(stringConexao);
        }

        public async Task InicializarBancoAsync()
        {
            //conecta primeiro ao servidor sem selecionar um banco especifico para poder cria-lo
            MySqlConnectionStringBuilder dadosServidor = new MySqlConnectionStringBuilder(stringConexao);
            dadosServidor.Database = "";

            await using (MySqlConnection dbServidor = new MySqlConnection(dadosServidor.ConnectionString))
            {
                await dbServidor.OpenAsync();

                const string sqlCriarBanco =
                    "CREATE DATABASE IF NOT EXISTS simulador_gravitacional " +
                    "CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;";

                await using (MySqlCommand cmdBanco = new MySqlCommand(sqlCriarBanco, dbServidor))
                {
                    await cmdBanco.ExecuteNonQueryAsync();
                }
            }

            //com o banco criado, conecta diretamente nele e garante a existencia das tabelas
            await using (MySqlConnection db = NovaConexao())
            {
                await db.OpenAsync();

                const string sqlSimulacoes =
                    "CREATE TABLE IF NOT EXISTS simulacoes (" +
                    "id BIGINT UNSIGNED NOT NULL AUTO_INCREMENT, " +
                    "criada_em DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
                    "intervalo_ms INT UNSIGNED NOT NULL, " +
                    "delta_tempo_s DOUBLE NOT NULL, " +
                    "ultima_iteracao INT UNSIGNED NOT NULL DEFAULT 0, " +
                    "PRIMARY KEY (id)" +
                    ") ENGINE = InnoDB;";

                await using (MySqlCommand cmdSimulacoes = new MySqlCommand(sqlSimulacoes, db))
                {
                    await cmdSimulacoes.ExecuteNonQueryAsync();
                }

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

                await using (MySqlCommand cmdEstados = new MySqlCommand(sqlEstados, db))
                {
                    await cmdEstados.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task TestarAsync()
        {
            await using var db = NovaConexao();
            await db.OpenAsync();
            await using var comando = new MySqlCommand("SELECT 1 FROM simulacoes LIMIT 1", db);
            await comando.ExecuteScalarAsync();
        }

        public override async Task<long> SalvarSimulacaoAsync(
            int intervaloMs,
            double deltaTempo,
            int ultimaIteracao,
            string caminhoArquivo)
        {
            await using var db = NovaConexao();
            await db.OpenAsync();

            //metadados, estados dos corpos e ultima iteracao entram na mesma transacao
            //para garantir que nenhuma simulacao fique incompleta no banco
            await using var transacao = await db.BeginTransactionAsync();

            await using var cmdSimulacao = new MySqlCommand(
                "INSERT INTO simulacoes (intervalo_ms, delta_tempo_s) VALUES (@intervalo, @delta)", db, transacao);
            cmdSimulacao.Parameters.AddWithValue("@intervalo", intervaloMs);
            cmdSimulacao.Parameters.AddWithValue("@delta", deltaTempo);
            await cmdSimulacao.ExecuteNonQueryAsync();

            long id = cmdSimulacao.LastInsertedId;

            await InserirEstadosEmLotesAsync(db, transacao, id, caminhoArquivo);

            await using var cmdAtualizar = new MySqlCommand(
                "UPDATE simulacoes SET ultima_iteracao=@iteracao WHERE id=@id", db, transacao);
            cmdAtualizar.Parameters.AddWithValue("@iteracao", ultimaIteracao);
            cmdAtualizar.Parameters.AddWithValue("@id", id);
            await cmdAtualizar.ExecuteNonQueryAsync();

            await transacao.CommitAsync();
            return id;
        }

        //le o arquivo TXT local progressivamente e insere os registros no MySQL em lotes
        private static async Task InserirEstadosEmLotesAsync(
            MySqlConnection db,
            MySqlTransaction tx,
            long simulacaoId,
            string caminhoArquivo)
        {
            const int tamanhoLote = 200;

            using StreamReader leitor = new StreamReader(caminhoArquivo, Encoding.UTF8);

            List<string> valoresSql = new List<string>(tamanhoLote);
            using MySqlCommand comando = new MySqlCommand();
            comando.Connection = db;
            comando.Transaction = tx;

            int iteracaoAtual = 0;
            int indiceNoLote = 0;
            string? linha;

            while ((linha = await leitor.ReadLineAsync()) != null)
            {
                if (string.IsNullOrWhiteSpace(linha))
                {
                    continue;
                }

                string[] partes = linha.Split(';');

                if (partes[0] == "ITERACAO")
                {
                    iteracaoAtual = int.Parse(partes[1]);
                    continue;
                }

                if (partes[0] != "CORPO")
                {
                    continue;
                }

                int indiceCorpo = int.Parse(partes[1]);
                string nome = partes[2];
                double massa = double.Parse(partes[3]);
                double densidade = double.Parse(partes[4]);
                double posX = double.Parse(partes[5]);
                double posY = double.Parse(partes[6]);
                double velX = double.Parse(partes[7]);
                double velY = double.Parse(partes[8]);
                int corArgb = int.Parse(partes[9]);

                string pIt = "@it" + indiceNoLote;
                string pInd = "@ind" + indiceNoLote;
                string pNome = "@nome" + indiceNoLote;
                string pMassa = "@massa" + indiceNoLote;
                string pDens = "@dens" + indiceNoLote;
                string pX = "@x" + indiceNoLote;
                string pY = "@y" + indiceNoLote;
                string pVx = "@vx" + indiceNoLote;
                string pVy = "@vy" + indiceNoLote;
                string pCor = "@cor" + indiceNoLote;

                valoresSql.Add(
                    $"(@id, {pIt}, {pInd}, {pNome}, {pMassa}, {pDens}, {pX}, {pY}, {pVx}, {pVy}, {pCor})");

                comando.Parameters.AddWithValue(pIt, iteracaoAtual);
                comando.Parameters.AddWithValue(pInd, indiceCorpo);
                comando.Parameters.AddWithValue(pNome, nome);
                comando.Parameters.AddWithValue(pMassa, massa);
                comando.Parameters.AddWithValue(pDens, densidade);
                comando.Parameters.AddWithValue(pX, posX);
                comando.Parameters.AddWithValue(pY, posY);
                comando.Parameters.AddWithValue(pVx, velX);
                comando.Parameters.AddWithValue(pVy, velY);
                comando.Parameters.AddWithValue(pCor, corArgb);

                indiceNoLote++;

                if (indiceNoLote >= tamanhoLote)
                {
                    await ExecutarLoteAsync(comando, simulacaoId, valoresSql);
                    valoresSql.Clear();
                    comando.Parameters.Clear();
                    indiceNoLote = 0;
                }
            }

            if (valoresSql.Count > 0)
            {
                await ExecutarLoteAsync(comando, simulacaoId, valoresSql);
            }
        }

        private static async Task ExecutarLoteAsync(
            MySqlCommand comando,
            long simulacaoId,
            List<string> valoresSql)
        {
            comando.Parameters.AddWithValue("@id", simulacaoId);
            comando.CommandText =
                "INSERT INTO estados_corpos " +
                "(simulacao_id, iteracao, indice_corpo, nome, massa_kg, densidade_kg_m3, " +
                "pos_x, pos_y, vel_x_m_s, vel_y_m_s, cor_argb) VALUES " +
                string.Join(", ", valoresSql);

            await comando.ExecuteNonQueryAsync();
        }

        public override async Task<List<SimulacaoSalva>> ListarAsync()
        {
            List<SimulacaoSalva> resultado = new List<SimulacaoSalva>();
            await using var db = NovaConexao();
            await db.OpenAsync();
            await using var comando = new MySqlCommand(
                "SELECT id, criada_em, ultima_iteracao FROM simulacoes ORDER BY id DESC", db);
            await using var leitor = await comando.ExecuteReaderAsync();
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
            await using var db = NovaConexao();
            await db.OpenAsync();
            await using var meta = new MySqlCommand(
                "SELECT intervalo_ms, delta_tempo_s FROM simulacoes WHERE id=@id", db);
            meta.Parameters.AddWithValue("@id", id);
            int intervalo;
            double delta;
            await using (var leitor = await meta.ExecuteReaderAsync())
            {
                if (!await leitor.ReadAsync()) throw new InvalidOperationException("Simulação não encontrada.");
                intervalo = leitor.GetInt32(0);
                delta = leitor.GetDouble(1);
            }

            const string sql =
                "SELECT iteracao, indice_corpo, nome, massa_kg, densidade_kg_m3, " +
                "pos_x, pos_y, vel_x_m_s, vel_y_m_s, cor_argb " +
                "FROM estados_corpos WHERE simulacao_id=@id ORDER BY iteracao, indice_corpo";

            await using var comando = new MySqlCommand(sql, db);
            comando.Parameters.AddWithValue("@id", id);
            SortedDictionary<int, Dictionary<int, Corpo>> grupos = new SortedDictionary<int, Dictionary<int, Corpo>>();
            int tamanho = 0;
            await using var dados = await comando.ExecuteReaderAsync();
            while (await dados.ReadAsync())
            {
                int it = dados.GetInt32(0);
                int indice = dados.GetInt32(1);
                tamanho = Math.Max(tamanho, indice + 1);
                if (!grupos.TryGetValue(it, out Dictionary<int, Corpo>? grupo))
                {
                    grupo = new Dictionary<int, Corpo>();
                    grupos[it] = grupo;
                }
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

            List<Quadro> quadros = new List<Quadro>();
            foreach (KeyValuePair<int, Dictionary<int, Corpo>> grupo in grupos)
            {
                //recria os espacos vazios dos corpos absorvidos para o replay
                //preservar as mesmas posicoes no array original
                Corpo?[] corpos = new Corpo?[tamanho];
                foreach (KeyValuePair<int, Corpo> item in grupo.Value)
                {
                    corpos[item.Key] = item.Value;
                }
                quadros.Add(new Quadro(grupo.Key, corpos));
            }
            return (intervalo, delta, quadros);
        }
    }
}
