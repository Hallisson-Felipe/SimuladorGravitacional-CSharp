using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace SimuladorGavitacional
{

    internal class GravadorArquivoTexto : GravadorDados
    {
        
        public override void GravarPosicoesIniciais(Corpo?[] corpos, string caminhoArquivo, int qtdIteracoes, int intervaloIteracoes)
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
                            escritor.WriteLine($"{c.Nome};{c.Massa};{c.Densidade};{c.PosX};{c.PosY};{c.VelX};{c.VelY}");
                        }
                    }
                }
        }

        public override Corpo[] CarregarPosicoesIniciais(
            string caminhoArquivo,
            out int qtdIteracoes,
            out int intervaloIteracoes)
        {
            if (!File.Exists(caminhoArquivo))
            {
                throw new FileNotFoundException("O arquivo informado não foi encontrado.", caminhoArquivo);
            }

            string[] linhas = File.ReadAllLines(caminhoArquivo);
            if (linhas.Length == 0)
            {
                throw new FormatException("O arquivo está vazio.");
            }

            // Remove eventual BOM (marca de arquivo UTF-8) e espaços antes de analisar o cabeçalho.
            string primeiraLinha = linhas[0].Trim('\uFEFF', ' ', '\t', '\r');
            string[] cabecalho = primeiraLinha.Split(';', StringSplitOptions.TrimEntries);
            if (cabecalho.Length != 3 ||
                !TryParseInt(cabecalho[0], out int quantidadeCorpos) ||
                !TryParseInt(cabecalho[1], out qtdIteracoes) ||
                !TryParseInt(cabecalho[2], out intervaloIteracoes) ||
                quantidadeCorpos < 0 || qtdIteracoes < 0 || intervaloIteracoes <= 0)
            {
                throw new FormatException(
                    $"Cabeçalho inválido: '{primeiraLinha}'. Esperado: quantidade;iterações;intervalo. " +
                    "Selecione posicoes_iniciais.txt, não nomes_corpos_simulador.txt.");
            }

            List<Corpo> corpos = new List<Corpo>(quantidadeCorpos);

            for (int i = 1; i < linhas.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(linhas[i]))
                {
                    continue;
                }

                string[] dados = linhas[i].Split(';');
                if (dados.Length != 7)
                {
                    throw new FormatException($"A linha {i + 1} não possui os 7 campos esperados.");
                }

                if (!TryParseDouble(dados[1], out double massa) ||
                    !TryParseDouble(dados[2], out double densidade) ||
                    !TryParseDouble(dados[3], out double posX) ||
                    !TryParseDouble(dados[4], out double posY) ||
                    !TryParseDouble(dados[5], out double velX) ||
                    !TryParseDouble(dados[6], out double velY))
                {
                    throw new FormatException($"Os valores numéricos da linha {i + 1} são inválidos.");
                }

                if (massa <= 0 || densidade <= 0)
                {
                    throw new FormatException($"A massa e a densidade da linha {i + 1} devem ser maiores que zero.");
                }

                corpos.Add(new Corpo
                {
                    Nome = dados[0],
                    Massa = massa,
                    Densidade = densidade,
                    PosX = posX,
                    PosY = posY,
                    VelX = velX,
                    VelY = velY,
                    Cor = Color.FromArgb(100, 180, 220)
                });
            }

            if (corpos.Count != quantidadeCorpos)
            {
                throw new FormatException("A quantidade de corpos não corresponde ao cabeçalho do arquivo.");
            }

            return corpos.ToArray();
        }

        private static bool TryParseDouble(string valor, out double resultado)
        {
            // Aceita tanto arquivos antigos da cultura local quanto valores com ponto decimal.
            return double.TryParse(valor, NumberStyles.Float, CultureInfo.CurrentCulture, out resultado) ||
                   double.TryParse(valor, NumberStyles.Float, CultureInfo.InvariantCulture, out resultado);
        }

        private static bool TryParseInt(string valor, out int resultado)
        {
            return int.TryParse(valor.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out resultado) ||
                   int.TryParse(valor.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out resultado);
        }
    }
}
