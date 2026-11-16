using MySqlConnector;

namespace SimuladorGavitacional
{
    public partial class Form1
    {
        //conecta ao banco, gera os corpos aleatorios e abre a janela da simulacao
        private async void btnGerarBanco_Click(object sender, EventArgs e)
        {
            if (!ValidarValores()) return;
            try
            {
                GravadorDados? banco = await ConexaoBanco.ObterAsync(this);
                if (banco == null) return;
                Universo novoUniverso = new Universo
                {
                    MassaMin = (double)numericMassaMin.Value,
                    MassaMax = (double)numericMassaMax.Value,
                    VelocidadeMin = (double)numericVelMin.Value,
                    VelocidadeMax = (double)numericVelMax.Value
                };
                novoUniverso.GerarCorposAleatorios((int)numericQuant.Value);
                new FormUniverso(novoUniverso, banco: banco).Show();
            }
            catch (Exception ex) when (ex is MySqlException or FormatException or IOException)
            {
                MessageBox.Show($"Não foi possível iniciar a simulação:\n{ex.Message}");
            }
        }

        //lista as simulacoes salvas no MySQL e abre a selecionada em modo replay
        private async void btnCarregar_Click(object sender, EventArgs e)
        {
            try
            {
                GravadorDados? banco = await ConexaoBanco.ObterAsync(this);
                if (banco == null) return;
                List<SimulacaoSalva> lista = await banco.ListarAsync();
                if (lista.Count == 0)
                {
                    MessageBox.Show("Ainda não há simulações salvas no MySQL.");
                    return;
                }
                using Form escolha = new Form
                {
                    Text = "Selecionar simulação para replay",
                    ClientSize = new Size(460, 330),
                    StartPosition = FormStartPosition.CenterParent
                };
                ListBox itens = new ListBox
                {
                    Dock = DockStyle.Top,
                    Height = 275,
                    FormattingEnabled = true
                };
                itens.Format += (_, argumentos) =>
                {
                    if (argumentos.ListItem is SimulacaoSalva simulacao)
                    {
                        argumentos.Value = $"#{simulacao.Id} — {simulacao.CriadaEm:dd/MM/yyyy HH:mm} — {simulacao.UltimaIteracao} iterações";
                    }
                };
                foreach (SimulacaoSalva simulacao in lista)
                {
                    itens.Items.Add(simulacao);
                }
                itens.SelectedIndex = 0;
                Button abrir = new Button
                {
                    Text = "Reproduzir",
                    Dock = DockStyle.Bottom,
                    Height = 45,
                    DialogResult = DialogResult.OK
                };
                escolha.Controls.AddRange(new Control[] { itens, abrir });
                escolha.AcceptButton = abrir;
                if (escolha.ShowDialog(this) != DialogResult.OK || itens.SelectedItem is not SimulacaoSalva selecionada)
                    return;

                (int IntervaloMs, double DeltaTempo, List<Quadro> Quadros) carregada = await banco.CarregarAsync(selecionada.Id);
                if (carregada.Quadros.Count == 0)
                {
                    MessageBox.Show("Esta simulação não contém quadros para reprodução.");
                    return;
                }
                //abre o universo apenas para reproduzir os quadros gravados
                Universo universoReplay = new Universo { Corpos = carregada.Quadros[0].Corpos };
                new FormUniverso(universoReplay, posicoesCarregadas: true,
                    intervaloIteracoes: carregada.IntervaloMs, quadrosReplay: carregada.Quadros).Show();
            }
            catch (Exception ex) when (ex is MySqlException or FormatException or InvalidOperationException)
            {
                MessageBox.Show($"Não foi possível carregar a simulação:\n{ex.Message}");
            }
        }
    }
}
