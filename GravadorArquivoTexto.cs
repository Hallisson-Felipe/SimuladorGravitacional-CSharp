using System;
using System.IO;

namespace SimuladorGavitacional
{
    /// <summary>
    /// Implementação concreta da classe abstrata GravadorDados voltada para gravação em arquivo texto (.txt).
    /// Atende ao requisito do projeto de salvar a posição inicial de cada corpo para posterior recuperação.
    /// </summary>
    internal class GravadorArquivoTexto : GravadorDados
    {
        /// <summary>
        /// Grava a posição inicial e propriedades físicas básicas de cada corpo em um arquivo texto delimitado por ponto e vírgula.
        /// </summary>
        /// <param name="corpos">Vetor com os corpos gerados no universo</param>
        /// <param name="caminhoArquivo">Caminho do arquivo texto onde os dados serão armazenados</param>
        public override void GravarPosicoesIniciais(Corpo?[] corpos, string caminhoArquivo)
        {
            try
            {
                // Abre o arquivo para escrita sobrescrevendo caso já exista
                using (StreamWriter escritor = new StreamWriter(caminhoArquivo, false))
                {
                    // Cabeçalho descritivo das colunas
                    escritor.WriteLine("Nome;PosX;PosY;Massa;Densidade;VelX;VelY");

                    // Itera sobre todos os corpos gerados
                    for (int i = 0; i < corpos.Length; i++)
                    {
                        Corpo? c = corpos[i];

                        // Ignora posições nulas (caso algum corpo tenha sido descartado na geração)
                        if (c != null)
                        {
                            // Escreve os dados formatados do corpo
                            escritor.WriteLine($"{c.Nome};{c.PosX:F4};{c.PosY:F4};{c.Massa:F4};{c.Densidade:F4};{c.VelX:F4};{c.VelY:F4}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Em caso de falha de I/O, exibe mensagem no console de debug para não travar a aplicação
                System.Diagnostics.Debug.WriteLine($"Erro ao gravar posições iniciais em arquivo texto: {ex.Message}");
            }
        }
    }
}
