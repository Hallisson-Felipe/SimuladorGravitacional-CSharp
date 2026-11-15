using System.IO;
using System.Text;

namespace SimuladorGavitacional
{
    // Registra os estados da simulação em um arquivo texto local (simulacao_temp.txt)
    // para evitar acessos ao MySQL durante a execução da física.
    public class RegistroLocalSimulacao
    {
        public string CaminhoArquivo { get; set; }
        public bool EstaAberto
        {
            get { return escritor != null; }
        }
        private StreamWriter? escritor;

        public RegistroLocalSimulacao()
        {
            CaminhoArquivo = ObterCaminhoPadrao();
        }

        public static string ObterCaminhoPadrao()
        {
            return Path.Combine(AppContext.BaseDirectory, "simulacao_temp.txt");
        }

        // Garante que o arquivo temporário exista ao iniciar o programa,
        // sem apagar dados pendentes caso já exista conteúdo.
        public static void GarantirArquivoExiste()
        {
            string caminho = ObterCaminhoPadrao();
            if (!File.Exists(caminho))
            {
                File.WriteAllText(caminho, string.Empty, Encoding.UTF8);
            }
        }

        // Abre o arquivo em UTF-8 para gravação incremental da nova simulação,
        // sobrescrevendo qualquer conteúdo anterior.
        public void Iniciar()
        {
            Fechar();
            escritor = new StreamWriter(CaminhoArquivo, false, Encoding.UTF8);
        }

        // Grava no arquivo todos os corpos vivos da iteração informada,
        // preservando o índice original de cada corpo no universo.
        public void RegistrarIteracao(int iteracao, Corpo?[]? corpos)
        {
            if (escritor == null || corpos == null)
            {
                return;
            }

            escritor.WriteLine($"ITERACAO;{iteracao}");

            for (int i = 0; i < corpos.Length; i++)
            {
                Corpo? corpo = corpos[i];
                if (corpo == null)
                {
                    continue;
                }

                escritor.WriteLine(
                    $"CORPO;{i};{corpo.Nome};{corpo.Massa};{corpo.Densidade};" +
                    $"{corpo.PosX};{corpo.PosY};{corpo.VelX};{corpo.VelY};{corpo.Cor.ToArgb()}");
            }

            escritor.WriteLine();
        }

        // Descarrega os dados em buffer e fecha o StreamWriter.
        public void Fechar()
        {
            if (escritor != null)
            {
                escritor.Flush();
                escritor.Dispose();
                escritor = null;
            }
        }

        // Limpa o conteúdo do arquivo temporário após confirmação de gravação no banco,
        // mantendo o arquivo existente para reutilização nas próximas simulações.
        public void Excluir()
        {
            Fechar();
            File.WriteAllText(CaminhoArquivo, string.Empty, Encoding.UTF8);
        }
    }
}
