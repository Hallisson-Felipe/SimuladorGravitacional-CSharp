using System;
using System.IO;

namespace SimuladorGavitacional
{

    internal class GravadorArquivoTexto : GravadorDados
    {
        
        public override void GravarPosicoesIniciais(Corpo?[] corpos, string caminhoArquivo, int qtdIteracoes, int intervaloIteracoes)
        {
            try
            {
                //abre o arquivo para escrita sobrescrevendo caso ja exista
                using (StreamWriter escritor = new StreamWriter(caminhoArquivo, false))
                {
                    //laco para contar apenas os corpos validos
                    int cont = 0;
                    for(int i = 0; i<corpos.Length; i++)
                    {
                        if (corpos[i] == null)
                        {
                            continue;
                        }
                        cont++;
                    }

                    //salva a quatidade de corpos, quantidade de iteracoes e o tempo entre as iteracoes
                    escritor.WriteLine($"{cont};{qtdIteracoes};{intervaloIteracoes}");

                    for (int i = 0; i < corpos.Length; i++)
                    {
                        Corpo? c = corpos[i];

                        //ignora posicoes nulas (caso algum corpo tenha sido descartado na geracao)
                        if (c != null)
                        {
                            //escreve os dados formatados do corpo
                            escritor.WriteLine($"{c.Nome};{c.PosX};{c.PosY};{c.Massa};{c.Densidade};{c.VelX};{c.VelY}");
                        }
                    }
                }
            }
            //em caso de excecao, informa ao usuário que houve um erro
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar arquivo:\n{ex.Message}");
            }
        }
    }
}
