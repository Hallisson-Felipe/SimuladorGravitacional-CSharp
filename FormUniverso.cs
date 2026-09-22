using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace SimuladorGavitacional
{
    partial class FormUniverso : Form
    {
        private Universo universo;
        private double deltaTempo;
        public FormUniverso(Universo u)
        {
            InitializeComponent();
            universo = u;
            deltaTempo = 0.0005;
            universo.EscalaUniverso = 300;
            this.WindowState = FormWindowState.Maximized;
        }


        private void FormUniverso_Load(object sender, EventArgs e)
        {
            using (Graphics graphics = panelUniverso.CreateGraphics())
            {
                universo.GerarPosicoes(panelUniverso.Height, panelUniverso.Width, universo, graphics);
            }

            //faz um reload no panelUniverso
            panelUniverso.Invalidate();


        }


        //evento que "pinta" o panelUniverso assim que ele e gerado
        private void panelUniverso_Paint(object sender, PaintEventArgs e)
        {
            //antialiasing para suavisar as bordas dos corpos
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //loop para percorrer os corpos e desenha-los na tela
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                Corpo? corpo = universo.Corpos[i];

                if (corpo != null)
                {
                    //transforma o raio em metros para pixels usando a escalaUniverso
                    float raioPixels = (float)(corpo.CalcularRaio() * universo.EscalaUniverso);

                    //calculo para garantir que PosX e PosY sejam o centro do corpo
                    //e nao o canto superior esquerdo, que e o padrao do metodo FillEllipse()
                    float x = (float)corpo.PosX - raioPixels;
                    float y = (float)corpo.PosY - raioPixels;

                    //transofrma o raio em diametro
                    float diametro = raioPixels * 2;

                    using (SolidBrush brush = new SolidBrush(corpo.Cor))
                    {
                        //desenha o corpo
                        e.Graphics.FillEllipse(brush, x, y, diametro, diametro);

                        //faz uma elipse pequena no centro do corpo
                        e.Graphics.FillEllipse(Brushes.Black, (float)corpo.PosX - 2, (float)corpo.PosY - 2, 4, 4);

                        //escreve o nome do corpo
                        e.Graphics.DrawString(corpo.Nome, this.Font, Brushes.Black, x, y - 20);
                    }
                }
            }

        }

        //evento tick do timer que executa uma iteracao a cada 16ms
        private void timerUniverso_Tick(object sender, EventArgs e)
        {
            //executa a iteracao com o valor de delta tempo de 0.0001 segundos e atualiza o panel
            universo.ExecutarIteracao(deltaTempo);
            panelUniverso.Invalidate();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //inicia o timer e muda o texto do botao
            timerUniverso.Start();
            btnIniciar.Text = "Continuar";
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            //para o timer
            timerUniverso.Stop();
        }

        //evento acionado quando btnReiniciar e clicado
        private void btnReiniciar_Click(object sender, EventArgs e)
        {
            //para o timer
            timerUniverso.Stop();

            //laco que percorre o array de corpos e reinicia as posicoes
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                //ignora o corpo caso ele seja nulo
                if (universo.Corpos[i] == null)
                {
                    continue;
                }

                //reseta as posicoes
                universo.Corpos[i]!.PosX = universo.posicoesXIniciais[i];
                universo.Corpos[i]!.PosY = universo.posicoesYIniciais[i];

            }

            //muda o texto do botao e reinicia o formulario;
            btnIniciar.Text = "Iniciar";
            panelUniverso.Invalidate();
        }

        //evento acionado quando o usuario altera a velocidade da simulacao
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //calcula o deltaTempo com base no valor selecionado pelo usuario
            deltaTempo = 0.0001 * trackBar1.Value / 50.0;

            //exibe o multiplicador de velocidade
            labelVelocidade.Text = $"{trackBar1.Value / 50.0:F1}x";
        }
    }
}