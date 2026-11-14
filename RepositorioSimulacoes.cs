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

    // Cada quadro conserva os índices originais; após uma colisão, o corpo absorvido
    // simplesmente deixa de ter linha nos quadros seguintes.
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
            // Conecta primeiro ao servidor sem selecionar um banco específico para poder criá-lo.
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

            // Com o banco criado, conecta diretamente nele e garante a existência das tabelas.
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

        public override async Task<long> CriarAsync(int intervaloMs, double deltaTempo, Corpo?[] corpos)
        {
            await using var db = NovaConexao();
            await db.OpenAsync();
            // Metadados e quadro zero devem entrar juntos: sem um deles, o replay ficaria incompleto.
            await using var transacao = await db.BeginTransactionAsync();
            await using var comando = new MySqlCommand(
                "INSERT INTO simulacoes (intervalo_ms, delta_tempo_s) VALUES (@intervalo, @delta)", db, transacao);
            comando.Parameters.AddWithValue("@intervalo", intervaloMs);
            comando.Parameters.AddWithValue("@delta", deltaTempo);
            await comando.ExecuteNonQueryAsync();
            long id = comando.LastInsertedId;
            await InserirCorposAsync(db, transacao, id, 0, corpos);
            await transacao.CommitAsync();
            return id;
        }

        public override async Task GravarAsync(long id, int iteracao, Corpo?[] corpos)
        {
            await using var db = NovaConexao();
            await db.OpenAsync();
            // Confirma o quadro e o contador na mesma transação para não indicar
            // uma iteração cujos corpos ainda não foram gravados.
            await using var transacao = await db.BeginTransactionAsync();
            await InserirCorposAsync(db, transacao, id, iteracao, corpos);
            await using var comando = new MySqlCommand(
                "UPDATE simulacoes SET ultima_iteracao=@iteracao WHERE id=@id", db, transacao);
            comando.Parameters.AddWithValue("@iteracao", iteracao);
            comando.Parameters.AddWithValue("@id", id);
            await comando.ExecuteNonQueryAsync();
            await transacao.CommitAsync();
        }

        private static async Task InserirCorposAsync(MySqlConnection db, MySqlTransaction tx,
            long id, int iteracao, Corpo?[] corpos)
        {
            const string sql =
                "INSERT INTO estados_corpos " +
                "(simulacao_id, iteracao, indice_corpo, nome, massa_kg, densidade_kg_m3, " +
                "pos_x, pos_y, vel_x_m_s, vel_y_m_s, cor_argb) " +
                "VALUES (@id, @it, @indice, @nome, @massa, @densidade, @x, @y, @vx, @vy, @cor)";

            await using var comando = new MySqlCommand(sql, db, tx);
            comando.Parameters.Add("@id", MySqlDbType.UInt64);
            comando.Parameters.Add("@it", MySqlDbType.UInt32);
            comando.Parameters.Add("@indice", MySqlDbType.UInt32);
            comando.Parameters.Add("@nome", MySqlDbType.VarChar);
            comando.Parameters.Add("@massa", MySqlDbType.Double);
            comando.Parameters.Add("@densidade", MySqlDbType.Double);
            comando.Parameters.Add("@x", MySqlDbType.Double);
            comando.Parameters.Add("@y", MySqlDbType.Double);
            comando.Parameters.Add("@vx", MySqlDbType.Double);
            comando.Parameters.Add("@vy", MySqlDbType.Double);
            comando.Parameters.Add("@cor", MySqlDbType.Int32);

            for (int i = 0; i < corpos.Length; i++)
            {
                Corpo? c = corpos[i];
                // O índice mantém a identidade do corpo entre quadros; null indica
                // que ele foi absorvido em uma colisão anterior.
                if (c == null) continue;
                comando.Parameters["@id"].Value = id;
                comando.Parameters["@it"].Value = iteracao;
                comando.Parameters["@indice"].Value = i;
                comando.Parameters["@nome"].Value = c.Nome;
                comando.Parameters["@massa"].Value = c.Massa;
                comando.Parameters["@densidade"].Value = c.Densidade;
                comando.Parameters["@x"].Value = c.PosX;
                comando.Parameters["@y"].Value = c.PosY;
                comando.Parameters["@vx"].Value = c.VelX;
                comando.Parameters["@vy"].Value = c.VelY;
                comando.Parameters["@cor"].Value = c.Cor.ToArgb();
                await comando.ExecuteNonQueryAsync();
            }
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
                // Recria os espaços vazios dos corpos absorvidos para o replay
                // preservar as mesmas posições no array original.
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
