using MySqlConnector;

namespace SimuladorGavitacional;

public partial class Form1
{
    private async void btnGerarBanco_Click(object sender, EventArgs e)
    {
        if (!ValidarValores()) return;
        try
        {
            RepositorioSimulacoes? banco = await ConexaoBanco.ObterAsync(this);
            if (banco is null) return;
            Universo novo = new()
            {
                MassaMin = (double)numericMassaMin.Value, MassaMax = (double)numericMassaMax.Value,
                VelocidadeMin = (double)numericVelMin.Value, VelocidadeMax = (double)numericVelMax.Value
            };
            novo.GerarCorposAleatorios((int)numericQuant.Value);
            new FormUniverso(novo, banco: banco).Show();
        }
        catch (Exception ex) when (ex is MySqlException or FormatException or IOException)
        {
            MessageBox.Show($"Não foi possível iniciar a simulação:\n{ex.Message}");
        }
    }

    private async void btnCarregar_Click(object sender, EventArgs e)
    {
        try
        {
            RepositorioSimulacoes? banco = await ConexaoBanco.ObterAsync(this);
            if (banco is null) return;
            List<SimulacaoSalva> lista = await banco.ListarAsync();
            if (lista.Count == 0)
            {
                MessageBox.Show("Ainda não há simulações salvas no MySQL.");
                return;
            }
            using Form escolha = new()
            {
                Text = "Selecionar simulação para replay", ClientSize = new Size(460, 330),
                StartPosition = FormStartPosition.CenterParent
            };
            ListBox itens = new() { Dock = DockStyle.Top, Height = 275 };
            itens.Items.AddRange(lista.Cast<object>().ToArray());
            itens.SelectedIndex = 0;
            Button abrir = new() { Text = "Reproduzir", Dock = DockStyle.Bottom,
                Height = 45, DialogResult = DialogResult.OK };
            escolha.Controls.AddRange([itens, abrir]);
            escolha.AcceptButton = abrir;
            if (escolha.ShowDialog(this) != DialogResult.OK || itens.SelectedItem is not SimulacaoSalva selecionada)
                return;
            var carregada = await banco.CarregarAsync(selecionada.Id);
            if (carregada.Quadros.Count == 0)
            {
                MessageBox.Show("Esta simulação não contém quadros para reprodução.");
                return;
            }
            Universo universoReplay = new() { Corpos = carregada.Quadros[0].Corpos! };
            new FormUniverso(universoReplay, posicoesCarregadas: true,
                intervaloIteracoes: carregada.IntervaloMs, quadrosReplay: carregada.Quadros).Show();
        }
        catch (Exception ex) when (ex is MySqlException or FormatException or InvalidOperationException)
        {
            MessageBox.Show($"Não foi possível carregar a simulação:\n{ex.Message}");
        }
    }
}
