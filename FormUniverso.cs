using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SimuladorGavitacional
{
    partial class FormUniverso : Form
    {
        private Universo universo;
        private double deltaTempo;
        private int IntervaloIteracoes;
        private int qtdIteracoes;
        private readonly bool posicoesCarregadas;
        private readonly GravadorDados? banco;
        private readonly List<Quadro>? quadrosReplay;
        private long simulacaoId;
        private int indiceReplay;
        private bool operacaoPendente;
        private bool pararSolicitado;

        public FormUniverso(
            Universo u,
            bool posicoesCarregadas = false,
            int iteracoesIniciais = 0,
            int intervaloIteracoes = 16,
            GravadorDados? banco = null,
            List<Quadro>? quadrosReplay = null)
        {
            //inicializa o formulario e o universo
            InitializeComponent();
            universo = u;

            //define o intervalo entre as iteracoes em ms
            this.posicoesCarregadas = posicoesCarregadas;
            this.banco = banco;
            this.quadrosReplay = quadrosReplay;
            IntervaloIteracoes = intervaloIteracoes;
            timerUniverso.Interval = IntervaloIteracoes;

            //define o deltaTempo e a escala do universo em pixels/metro
            deltaTempo = 0.0005;
            universo.EscalaUniverso = 300;
            universo.EscalaVisualCorpos = 30;

            //contador da quantidade de iteracoes
            qtdIteracoes = iteracoesIniciais;
            if (quadrosReplay is not null) qtdIteracoes = quadrosReplay[0].Iteracao;

            this.WindowState = FormWindowState.Maximized;
        }


        private void FormUniverso_Load(object sender, EventArgs e)
        {
            if (!posicoesCarregadas)
            {
                Graphics graphics = panelUniverso.CreateGraphics();
                universo.GerarPosicoes(panelUniverso.Height, panelUniverso.Width, universo, graphics);
                graphics.Dispose();
            }

            //faz um reload no panelUniverso
            labelIteracoes.Text = $"{qtdIteracoes} Iterações";
            labelIteracoes.Visible = true;
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
                    float raioPixels = (float)(corpo.CalcularRaio() * universo.EscalaVisualCorpos);

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

        // Executa uma iteração conforme o intervalo configurado na simulação.
        private async void timerUniverso_Tick(object sender, EventArgs e)
        {
            // Evita ticks sobrepostos enquanto o quadro atual é gravado no banco.
            timerUniverso.Stop();
            if (pararSolicitado) return;
            if (quadrosReplay is not null)
            {
                // Replay usa os estados gravados; não recalcula a física.
                indiceReplay++;
                if (indiceReplay >= quadrosReplay.Count)
                {
                    btnIniciar.Visible = true;
                    return;
                }
                universo.Corpos = quadrosReplay[indiceReplay].Corpos!;
                qtdIteracoes = quadrosReplay[indiceReplay].Iteracao;
                labelIteracoes.Text = $"{qtdIteracoes} Iterações — replay";
                panelUniverso.Invalidate();
                timerUniverso.Start();
                return;
            }
            
            //incrementa a quantidade de iteracoes
            qtdIteracoes++;
            labelIteracoes.Text = $"{qtdIteracoes} Iteracoes";

            // Calcula e exibe o estado que será persistido nesta iteração.
            universo.ExecutarIteracao(deltaTempo);
            panelUniverso.Invalidate();
            operacaoPendente = true;
            bool gravou = false;
            try
            {
                if (banco is not null)
                    await banco.GravarAsync(simulacaoId, qtdIteracoes, universo.Corpos);
                gravou = true;
            }
            catch (MySqlConnector.MySqlException ex)
            {
                MessageBox.Show($"A gravação falhou na iteração {qtdIteracoes}. " +
                    $"Os quadros anteriores continuam no banco; feche esta simulação.\n{ex.Message}");
            }
            finally
            {
                operacaoPendente = false;
                if (!IsDisposed)
                {
                    if (pararSolicitado) Close();
                    else if (gravou) timerUniverso.Start();
                }
            }
        }

        private async void btnIniciar_Click(object sender, EventArgs e)
        {
            if (quadrosReplay is not null && indiceReplay >= quadrosReplay.Count - 1)
            {
                indiceReplay = 0;
                universo.Corpos = quadrosReplay[0].Corpos!;
                qtdIteracoes = quadrosReplay[0].Iteracao;
                panelUniverso.Invalidate();
            }
            if (banco is not null && simulacaoId == 0)
            {
                btnIniciar.Enabled = false;
                operacaoPendente = true;
                try
                {
                    // A iteração zero captura o estado antes do primeiro movimento.
                    simulacaoId = await banco.CriarAsync(IntervaloIteracoes, deltaTempo, universo.Corpos);
                }
                catch (MySqlConnector.MySqlException ex)
                {
                    MessageBox.Show($"Não foi possível criar a simulação:\n{ex.Message}");
                    return;
                }
                finally
                {
                    operacaoPendente = false;
                    if (!IsDisposed)
                    {
                        btnIniciar.Enabled = true;
                        if (pararSolicitado) Close();
                    }
                }
            }
            if (IsDisposed) return;
            if (pararSolicitado)
            {
                Close();
                return;
            }
            //inicia o timer, desabilita o botao e mostra a quantidade de iteracoes
            timerUniverso.Start();
            btnIniciar.Visible = false;
            labelIteracoes.Visible = true;
        }

        private void btnParar_Click(object sender, EventArgs e)
        {
            //para o timer
            pararSolicitado = true;
            timerUniverso.Stop();

            // Se uma escrita está em andamento, o retorno do await fechará a janela.
            if (operacaoPendente)
            {
                btnParar.Text = "Parando...";
                return;
            }

            this.Close();
        }

        //evento acionado quando o usuario altera a velocidade da simulacao
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //calcula o deltaTempo com base no valor selecionado pelo usuario
            deltaTempo = 0.0005 * trackBar1.Value / 50.0;
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
