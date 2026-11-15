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
        private int indiceReplay;
        private RegistroLocalSimulacao? registroLocal;
        private bool salvandoNoBanco;
        private bool simulacaoFinalizada;

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
            this.FormClosing += FormUniverso_FormClosing;
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

            if (universo.Corpos == null)
            {
                return;
            }

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
        private void timerUniverso_Tick(object sender, EventArgs e)
        {
            if (salvandoNoBanco) return;

            if (quadrosReplay is not null)
            {
                // Replay usa os estados gravados; não recalcula a física.
                indiceReplay++;
                if (indiceReplay >= quadrosReplay.Count)
                {
                    timerUniverso.Stop();
                    btnIniciar.Visible = true;
                    return;
                }
                universo.Corpos = quadrosReplay[indiceReplay].Corpos!;
                qtdIteracoes = quadrosReplay[indiceReplay].Iteracao;
                labelIteracoes.Text = $"{qtdIteracoes} Iterações — replay";
                panelUniverso.Invalidate();
                return;
            }
            
            //incrementa a quantidade de iteracoes
            qtdIteracoes++;
            labelIteracoes.Text = $"{qtdIteracoes} Iteracoes";

            // Calcula, exibe e registra localmente o estado desta iteração, sem acessar o MySQL.
            universo.ExecutarIteracao(deltaTempo);
            panelUniverso.Invalidate();

            if (registroLocal != null)
            {
                registroLocal.RegistrarIteracao(qtdIteracoes, universo.Corpos);
            }
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            if (salvandoNoBanco) return;

            if (quadrosReplay is not null && indiceReplay >= quadrosReplay.Count - 1)
            {
                indiceReplay = 0;
                universo.Corpos = quadrosReplay[0].Corpos!;
                qtdIteracoes = quadrosReplay[0].Iteracao;
                panelUniverso.Invalidate();
            }

            if (banco is not null && (registroLocal == null || !registroLocal.EstaAberto))
            {
                try
                {
                    if (registroLocal == null)
                    {
                        registroLocal = new RegistroLocalSimulacao();
                    }

                    // Limpa/sobrescreve o arquivo temporário e grava a iteração zero (estado inicial).
                    registroLocal.Iniciar();
                    registroLocal.RegistrarIteracao(0, universo.Corpos);
                    simulacaoFinalizada = false;
                }
                catch (IOException ex)
                {
                    registroLocal?.Fechar();
                    registroLocal = null;
                    MessageBox.Show($"Não foi possível iniciar o registro local da simulação:\n{ex.Message}");
                    return;
                }
            }

            //inicia o timer, desabilita o botao e mostra a quantidade de iteracoes
            timerUniverso.Start();
            btnIniciar.Visible = false;
            labelIteracoes.Visible = true;
        }

        private async void btnParar_Click(object sender, EventArgs e)
        {
            // Para novas iterações imediatamente.
            timerUniverso.Stop();

            if (salvandoNoBanco) return;

            if (banco == null || registroLocal == null || simulacaoFinalizada)
            {
                this.Close();
                return;
            }

            await FinalizarEPersistirAsync();

            if (!IsDisposed)
            {
                this.Close();
            }
        }

        private async void FormUniverso_FormClosing(object? sender, FormClosingEventArgs e)
        {
            timerUniverso.Stop();

            // Impede o fechamento enquanto a persistência no MySQL estiver em andamento.
            if (salvandoNoBanco)
            {
                e.Cancel = true;
                return;
            }

            // Se o usuário fechar a janela diretamente pelo X, tenta persistir no MySQL antes de encerrar.
            if (banco != null && registroLocal != null && !simulacaoFinalizada)
            {
                e.Cancel = true;
                await FinalizarEPersistirAsync();

                if (!IsDisposed)
                {
                    this.Close();
                }
                return;
            }

            registroLocal?.Fechar();
        }

        private async Task FinalizarEPersistirAsync()
        {
            if (banco == null || registroLocal == null)
            {
                return;
            }

            salvandoNoBanco = true;
            btnParar.Enabled = false;
            btnIniciar.Enabled = false;
            btnParar.Text = "Salvando...";

            try
            {
                // Fecha o escritor local para liberar o arquivo para leitura.
                registroLocal.Fechar();

                // Persiste toda a simulação no MySQL em lotes dentro de uma transação.
                await banco.SalvarSimulacaoAsync(
                    IntervaloIteracoes,
                    deltaTempo,
                    qtdIteracoes,
                    registroLocal.CaminhoArquivo);
            }
            catch (Exception ex) when (ex is MySqlConnector.MySqlException or IOException)
            {
                MessageBox.Show($"Não foi possível salvar a simulação no MySQL:\n{ex.Message}");
            }
            finally
            {
                // Encerra o ciclo desta janela mesmo em caso de falha, preservando o TXT.
                simulacaoFinalizada = true;
                salvandoNoBanco = false;
                if (!IsDisposed)
                {
                    btnParar.Enabled = true;
                    btnIniciar.Enabled = true;
                    btnParar.Text = "Parar";
                }
            }
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
