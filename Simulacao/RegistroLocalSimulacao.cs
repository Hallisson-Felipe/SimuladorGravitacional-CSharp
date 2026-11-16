using System.IO;
using System.Text;

namespace SimuladorGavitacional
{
    //registra os estados da simulacao em um arquivo texto local (simulacao_temp.txt)
    //para evitar acessos ao MySQL durante a execucao da fisica
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

        //garante que o arquivo temporario exista ao iniciar o programa,
        //sem apagar dados pendentes caso ja exista conteudo
        public static void GarantirArquivoExiste()
        {
            string caminho = ObterCaminhoPadrao();
            if (!File.Exists(caminho))
            {
                File.WriteAllText(caminho, string.Empty, Encoding.UTF8);
            }
        }

        //abre o arquivo em UTF-8 para gravacao incremental da nova simulacao,
        //sobrescrevendo qualquer conteudo anterior
        public void Iniciar()
        {
            Fechar();
            escritor = new StreamWriter(CaminhoArquivo, false, Encoding.UTF8);
        }

        //grava no arquivo todos os corpos vivos da iteracao informada,
        //preservando o indice original de cada corpo no universo
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

        //descarrega os dados em buffer e fecha o StreamWriter
        public void Fechar()
        {
            if (escritor != null)
            {
                escritor.Flush();
                escritor.Dispose();
                escritor = null;
            }
        }

        //limpa o conteudo do arquivo temporario apos confirmacao de gravacao no banco,
        //mantendo o arquivo existente para reutilizacao nas proximas simulacoes
        public void Excluir()
        {
            Fechar();
            File.WriteAllText(CaminhoArquivo, string.Empty, Encoding.UTF8);
        }
    }
}
