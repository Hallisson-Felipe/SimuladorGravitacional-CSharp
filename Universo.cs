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

        public double[] posicoesXIniciais;
        public double[] posicoesYIniciais;

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

            //laço para gerar os corpos
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


        //metodo para gerars as posiçoes dos corpos no universo
        public void GerarPosicoes(int altura, int largura, Universo universo, Graphics graphics)
        {
            posicoesXIniciais = new double[universo.Corpos.Length];
            posicoesYIniciais = new double[universo.Corpos.Length];
            int corposIgnorados = 0;
            Random random = new Random();

            //define a distancia dos objetos em relacao a margem no momento da criacao
            int margem = 100;

            //laço para gerar as posicoes
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                //sai do loop se o corpo nao existir
                if (Corpos[i] == null)
                {
                    continue;
                }

                //gera a posicao do primeiro corpo
                if(i == 0)
                {
                    int tentativas = 0;

                    bool posValida = false;
                    
                    //laço que gera as coordenadas conforme validacao
                    while (!posValida && tentativas < 1000)
                    {
                        tentativas++;

                        Corpos[i].PosX = margem + random.NextDouble() * (largura - 2 * margem);
                        Corpos[i].PosY = margem + random.NextDouble() * (altura - 2 * margem);

                        //converte o raio de metros para pixels
                        float raioPixels = (float)(Corpos[i].CalcularRaio() * universo.EscalaUniverso);

                        float x = (float)Corpos[i].PosX - raioPixels;
                        float y = (float)Corpos[i].PosY - raioPixels;

                        //calcula o diametro em pixels do corpo
                        float diametro = raioPixels * 2;

                        //gera uma regiao que corresponde a area do corpo gerado de acordo com o diametro do mesmo
                        using (GraphicsPath caminho = new GraphicsPath())
                        {
                            caminho.AddEllipse(x, y, diametro, diametro);
                            Corpos[i].AreaOcupada = new Region(caminho);
                        }
                        
                        //valida a posicao criada, salva as posicoes iniciais e quebra o lopp
                        if (ValidarPosicao(Corpos[i].PosX, Corpos[i].PosY, raioPixels,altura, largura))
                        {
                            posicoesXIniciais[i] = Corpos[i].PosX;
                            posicoesYIniciais[i] = Corpos[i].PosY;
                            posValida = true;
                        }
                    }
                    //caso nao exista posicao valida para um corpo, ele nao sera exibido
                    if (!posValida)
                    {
                        Corpos[i] = null;
                        corposIgnorados++;
                    }
                }

                //gera a posicao dos demais corpos
                else
                {
                    int tentativas = 0;

                    bool posValida = false;

                    //laço que gera as coordenadas conforme validacao
                    while (!posValida && tentativas < 1000)
                    {
                        tentativas++;

                        Corpos[i].PosX = margem + random.NextDouble() * (largura - 2 * margem);
                        Corpos[i].PosY = margem + random.NextDouble() * (altura - 2 * margem);

                        //converte o raio de metros para pixels
                        float raioPixels = (float)(Corpos[i].CalcularRaio() * universo.EscalaUniverso);

                        float x = (float)Corpos[i].PosX - raioPixels;
                        float y = (float)Corpos[i].PosY - raioPixels;

                        //calcula o diametro em pixels do corpo
                        float diametro = raioPixels * 2;

                        //gera uma regiao que corresponde a area do corpo gerado de acordo com o diametro do mesmo
                        GraphicsPath caminho = new GraphicsPath();
                        caminho.AddEllipse(x, y, diametro, diametro);
                        Region regiao = new Region(caminho);

                        //garante que os corpos nao se sobreponham nem sejam criados fora da area visivel do universo
                        if (ValidarPosicao(regiao, i, graphics) && ValidarPosicao(Corpos[i].PosX, Corpos[i].PosY, raioPixels, altura, largura))
                        {
                            //confirma a posicao
                            Corpos[i].AreaOcupada = regiao;

                            //salva as posicoes iniciais dos corpos
                            posicoesXIniciais[i] = Corpos[i].PosX;
                            posicoesYIniciais[i] = Corpos[i].PosY;

                            posValida = true;

                        }
                        else 
                        {
                            //discarta a regiao invalida e incrementa as tentativas
                            caminho.Dispose();
                            regiao.Dispose();
                        }

                    }

                    //caso nao exista posicao valida para um corpo, ele nao sera exibido
                    if (!posValida)
                    {
                        Corpos[i] = null;
                        corposIgnorados++;
                    }

                }
            }

            //alerta o usuario caso algum corpo nao sja exibido
            if(corposIgnorados > 0)
            {
                MessageBox.Show($"{corposIgnorados} corpos foram ignorados por não caberem no universo.");
            }
        }

        
        //metodo para verificar se um corpo nao esta sendo criado em cima de outro
        public bool ValidarPosicao(Region regiao, int indice, Graphics graphics)
        {
            //laco para percorrer todas ao posicoes anteriores ao indice do corpo verificado
            for(int i = 0; i< indice; i++)
            {
                if (Corpos[i] == null || Corpos[i].AreaOcupada == null)
                {
                    continue;
                }
                //cria um clone da regiao do novo corpo para que caso ela seja valida, nao seja aletaraada
                using (Region intersecao = regiao.Clone())
                {
                    
                    //faz a intersecao das regioes onde os corpos estao
                    intersecao.Intersect(Corpos[i].AreaOcupada);

                    //caso tenha algo na intersecao significa que os corpos nao sobrepostos 
                    if (!intersecao.IsEmpty(graphics))
                    {
                        //retorna falso indiciando sobreposicao
                        return false;
                    }
                }
            }
            
            //retorna true caso o corpo esteja em uma posicao valida
            return true;
        }

        //sobrecarga do metodo ValidarPosicao que verifica se o corpo esta inteiramente dentro visivel
        public bool ValidarPosicao(double x, double y, double raio, double altura, double largura)
        {
            //verifica se o corpo foi gerado fora dos limites do universo
            if (x - raio < 0 || x + raio > largura || y - raio < 0 || y + raio > altura)
            {
                return false;
            }
            return true;
        }


        //metodo para calcular a forca gravitacional
        public (double forcaTotalx, double forcaTotalY) CalcularForcaGravitacional(int indice)
        {
            const double G = 6.67430e-11;
            double distancia;
            double forcaTotalX = 0;
            double forcaTotalY = 0;

            //verifica se o corpo existe
            if (Corpos[indice] == null)
            {
                return (0, 0);
            }

            for (int i = 0; i< Corpos.Length; i++)
            {
                if (i == indice || Corpos[i] == null)
                {
                    continue;
                }
                //converte as posicoes de pixels para metros
                double x1 = Corpos[indice].PosX / EscalaUniverso;
                double y1 = Corpos[indice].PosY / EscalaUniverso;

                double x2 = Corpos[i].PosX / EscalaUniverso;
                double y2 = Corpos[i].PosY / EscalaUniverso;

                //calcula a distancia entre os corpos
                distancia = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));


                //calcula a forca gravitacional entre os corpos
                double forca = G * (Corpos[indice].Massa * Corpos[i].Massa) / Math.Pow(distancia, 2);

                //calcula a forca exercida sobre o corpo
                double dx = x2 - x1;
                double dy = y2 - y1;

                //calcula a direcao da forca
                double direcaoX = dx / distancia;
                double direcaoY = dy / distancia;

                //decompoe a forca nos eixos x e y
                double forcaX = forca * direcaoX;
                double forcaY = forca * direcaoY;

                //soma as forcas exercidas pelos outros corpos
                forcaTotalX += forcaX;
                forcaTotalY += forcaY;
            }
            return (forcaTotalX, forcaTotalY);

        }

        //metodo para calcular a aceleracao do corpo com base na forca gravitacional resultante
        public (double aceleracaoX, double aceleracaoY) CalcularAceleracao(int indice)
        {
            //verifica se o corpo existe
            if (Corpos[indice] == null)
            {
                return (0, 0);
            }

            //calcula a forca gravitacional resultante nos eixos X e Y
            (double forcaX, double forcaY) = CalcularForcaGravitacional(indice);

            //calcula a aceleracao do corpo utilizando a segunda lei de Newton
            double aceleracaoX = forcaX / Corpos[indice].Massa;
            double aceleracaoY = forcaY / Corpos[indice].Massa;

            //retorna a aceleracao nos eixos X e Y
            return (aceleracaoX, aceleracaoY);
        }


        //metodo para atualizar a velocidade do corpo com base na aceleracao e no tempo decorrido
        public void AtualizarVelocidade(int indice, double deltaTempo)
        {
            //verifica se o corpo existe
            if (Corpos[indice] == null)
            {
                return;
            }

            //calcula a aceleracao do corpo nos eixos X e Y
            (double aceleracaoX, double aceleracaoY) = CalcularAceleracao(indice);

            //atualiza a velocidade do corpo considerando a aceleracao e o tempo decorrido
            Corpos[indice].VelX += aceleracaoX * deltaTempo;
            Corpos[indice].VelY += aceleracaoY * deltaTempo;
        }


        //metodo para atualizar a posicao do corpo com base na velocidade e no tempo decorrido
        public void AtualizarPosicao(int indice, double deltaTempo)
        {
            //verifica se o corpo existe
            if (Corpos[indice] == null)
            {
                return;
            }

            //calcula o deslocamento do corpo em metros nos eixos X e Y
            double deslocamentoX = Corpos[indice].VelX * deltaTempo;
            double deslocamentoY = Corpos[indice].VelY * deltaTempo;

            //converte o deslocamento de metros para pixels e atualiza a posicao do corpo
            Corpos[indice].PosX += deslocamentoX * EscalaUniverso;
            Corpos[indice].PosY += deslocamentoY * EscalaUniverso;
        }


        public void ExecutarIteracao(double deltaTempo)
        {
            double[] aceleracoesX = new double[Corpos.Length];
            double[] aceleracoesY = new double[Corpos.Length];

            //calcular todas as aceleracoes
            for (int i = 0; i < Corpos.Length; i++)
            {
                if (Corpos[i] == null)
                {
                    continue;
                }

                double ax, ay;
                (ax,ay) = CalcularAceleracao(i);

                aceleracoesX[i] = ax;
                aceleracoesY[i] = ay;
            }

            //atualizar todas as velocidades
            for (int i = 0; i < Corpos.Length; i++)
            {
                if (Corpos[i] == null)
                {
                    continue;
                }

                double ax, ay;
                (ax, ay) = CalcularAceleracao(i);

                Corpos[i].VelX += aceleracoesX[i] * deltaTempo;
                Corpos[i].VelY += aceleracoesY[i] * deltaTempo;
            }

            //atualizar todas as posicoes
            for (int i = 0; i < Corpos.Length; i++)
            {
                if (Corpos[i] == null)
                {
                    continue;
                }

                AtualizarPosicao(i, deltaTempo);
            }
        }
    }
}