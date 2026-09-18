using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SimuladorGavitacional
{
    class Universo
    {
        //array de corpos
        public Corpo[] Corpos;
        //array de nomes
        public string[] Nomes = new string[100];

        //limites dos corpos
        public double EscalaUniverso { get; set; }

        public double MassaMin { get; set; }
        public double MassaMax { get; set; }

        public double DensidadeMin { get; set; }
        public double DensidadeMax { get; set; }

        public double VelocidadeMin { get; set; }
        public double VelocidadeMax { get; set; }

        public Universo()
        {
            //lê o arquivo com os nomes aleatórios e coloca todos no array de nomes
            string aux = File.ReadAllText("nomes_corpos_simulador.txt");
            Nomes = aux.Split(';');
        }


        //gera os corpos aleatórios com base na quantidade e nos limites estabelecidos pelo usuario
        public void GerarCorposAleatorios(int quantidade)
        {
            Random random = new Random();
            Corpos = new Corpo[quantidade];

            for (int i = 0; i < quantidade; i++)
            {
                Corpo corpo = new Corpo();

                corpo.Nome = Nomes[random.Next(Nomes.Length)];

                corpo.Massa = MassaMin + (random.NextDouble() * (MassaMax - MassaMin));

                corpo.Densidade = DensidadeMin + (random.NextDouble() * (DensidadeMax - DensidadeMin));

                corpo.VelX = VelocidadeMin + (random.NextDouble() * (VelocidadeMax - VelocidadeMin));
                corpo.VelY = VelocidadeMin + (random.NextDouble() * (VelocidadeMax - VelocidadeMin));

                corpo.Cor = Color.FromArgb(random.Next(256),random.Next(256),random.Next(256));

                Corpos[i] = corpo;

            }
             
        }

        public void GerarPosicoes(int altura, int largura, Universo universo, Graphics graphics)
        {
            Random random = new Random();
            int margem = 100;
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                if (Corpos[i] == null)
                {
                    return;
                }
                if(i == 0)
                {
                    int tentativas = 0;
                    bool posValida = false;
                    while (!posValida || tentativas >1000)
                    {
                        Corpos[i].PosX = margem + random.NextDouble() * (largura - 2 * margem);
                        Corpos[i].PosY = margem + random.NextDouble() * (altura - 2 * margem);
                        float raioPixels = (float)(Corpos[i].CalcularRaio() * universo.EscalaUniverso);

                        float x = (float)Corpos[i].PosX - raioPixels;
                        float y = (float)Corpos[i].PosY - raioPixels;

                        float diametro = raioPixels * 2;
                        using (GraphicsPath caminho = new GraphicsPath())
                        {
                            caminho.AddEllipse(x, y, diametro, diametro);
                            Corpos[i].AreaOcupada = new Region(caminho);
                        }
                        

                        if (ValidarPosicao(Corpos[i].PosX, Corpos[i].PosY, raioPixels,altura, largura))
                        {
                            posValida = true;
                        }

                    }
                }
                else
                {
                    int tentativas = 0;
                    bool posValida = false;
                    while (!posValida || tentativas > 1000)
                    {
                        Corpos[i].PosX = margem + random.NextDouble() * (largura - 2 * margem);
                        Corpos[i].PosY = margem + random.NextDouble() * (altura - 2 * margem);
                        float raioPixels = (float)(Corpos[i].CalcularRaio() * universo.EscalaUniverso);

                        float x = (float)Corpos[i].PosX - raioPixels;
                        float y = (float)Corpos[i].PosY - raioPixels;

                        float diametro = raioPixels * 2;
                        GraphicsPath caminho = new GraphicsPath();
                        caminho.AddEllipse(x, y, diametro, diametro);
                        Region regiao = new Region(caminho);
                        if (ValidarPosicao(regiao, i, graphics) && ValidarPosicao(Corpos[i].PosX, Corpos[i].PosY, raioPixels, altura - 2*margem, largura - 2 * margem))
                        {
                            Corpos[i].AreaOcupada = regiao;
                            posValida = true;
                        }
                        else 
                        {
                            regiao.Dispose();
                            tentativas++;
                        }

                    }

                }
            }
        }


        public bool ValidarPosicao(Region regiao, int indice, Graphics graphics)
        {
            for(int i = 0; i< indice; i++)
            {
                using (Region intersecao = regiao.Clone())
                {
                    intersecao.Intersect(Corpos[i].AreaOcupada);

                    if (!intersecao.IsEmpty(graphics))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool ValidarPosicao(double x, double y, double raio, double altura, double largura)
        {
            if (x - raio < 0 || x + raio > largura || y - raio < 0 || y + raio > altura)
            {
                return false;
            }
            return true;

        }

    }
}