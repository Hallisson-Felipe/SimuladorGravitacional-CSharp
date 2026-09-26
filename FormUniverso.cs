using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace SimuladorGavitacional
{
    partial class FormUniverso : Form
    {
        private Universo universo;
        private double deltaTempo;
        private Corpo[] copia;
        private int IntervaloIteracoes;
        private int qtdIteracoes;
        public FormUniverso(Universo u)
        {
            //inicializa o formulario e o universo
            InitializeComponent();
            universo = u;

            //define o intervalo entre as iteracoes em ms
            IntervaloIteracoes = 16;
            timerUniverso.Interval = IntervaloIteracoes;

            //define o deltaTempo e a escala do universo em pixels/metro
            deltaTempo = 0.0005;
            universo.EscalaUniverso = 300;

            //contador da quantidade de iteracoes
            qtdIteracoes = 0;

            this.WindowState = FormWindowState.Maximized;
        }


        private void FormUniverso_Load(object sender, EventArgs e)
        {
            //gera as posicoes
            Graphics graphics = panelUniverso.CreateGraphics();
            universo.GerarPosicoes(panelUniverso.Height, panelUniverso.Width, universo, graphics);
            graphics.Dispose();

            //faz uma copia do estado inicial do array de corpos
            copia = universo.CopiarCorpos();

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
            //incrementa a quantidade de iteracoes e atualiza a tabela com os dados dos corpos
            qtdIteracoes++;
            labelIteracoes.Text = $"{qtdIteracoes} Iteracoes";
            AtualizarDataGrid();

            //executa a iteracao com o valor de delta tempo de 0.0001 segundos e atualiza o panel
            universo.ExecutarIteracao(deltaTempo);
            panelUniverso.Invalidate();
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            //inicia o timer, desabilita o botao e mostra a quantidade de iteracoes
            timerUniverso.Start();
            btnIniciar.Visible = false;
            labelIteracoes.Visible = true;
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            //para o timer
            timerUniverso.Stop();

            GravadorDados gravador = new GravadorArquivoTexto();
            //pega o caminho completo da localizacao do arquivo txt
            string caminhoArquivo = Path.GetFullPath(
                Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "posicoes_iniciais.txt")
            );
            //grava a posicao inicial de cada corpo em arquivo texto usando a classe abstrata
            gravador.GravarPosicoesIniciais(copia, caminhoArquivo, qtdIteracoes, IntervaloIteracoes);

            //exibe ao usuario que a gravacao foi concluida
            MessageBox.Show("Dados gravados em: posicoes_iniciais.txt");

            //fecha o formulario de exibicao
            this.Close();

        }

        //evento acionado quando o usuario altera a velocidade da simulacao
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //calcula o deltaTempo com base no valor selecionado pelo usuario
            deltaTempo = 0.0005 * trackBar1.Value / 50.0;
        }

        //metodo para atualizar o dataGrid dinamicamente a cada iteracao
        public void AtualizarDataGrid()
        {
            dataGridView1.Rows.Clear();

            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                if (universo.Corpos[i] == null)
                {
                    continue;
                }

                Corpo corpo = universo.Corpos[i];

                dataGridView1.Rows.Add(
                    corpo.Nome,
                    corpo.Massa,
                    corpo.PosX.ToString("F2"),
                    corpo.PosY.ToString("F2"),
                    corpo.VelX.ToString("F4"),
                    corpo.VelY.ToString("F4"),
                    corpo.Densidade
                );
            }
        }

        private void btnDados_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Visible == true)
            {
                btnDados.Text = "Mostrar dados";
                dataGridView1.Visible = false;
                return;
            }

            btnDados.Text = "Ocultar dados";
            dataGridView1.Visible = true;
        }

        private void panelController_Paint(object sender, PaintEventArgs e)
        {
            //antialiasing para suavisar as bordas dos corpos
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
    }
}