using MySqlConnector;

namespace SimuladorGavitacional
{
    public static class ConexaoBanco
    {
        public static GravadorDados? repositorio { get; set; }

        //solicita a senha do MySQL, inicializa o banco se necessario e retorna o gravador
        public static async Task<GravadorDados?> ObterAsync(IWin32Window dono)
        {
            if (repositorio != null) return repositorio;

            using Form janela = new Form
            {
                Text = "Senha do MySQL (root)",
                ClientSize = new Size(300, 110),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false
            };
            TextBox senha = new TextBox { Left = 20, Top = 18, Width = 260, UseSystemPasswordChar = true };
            Button entrar = new Button
            {
                Text = "Conectar",
                Left = 180,
                Top = 57,
                Width = 100,
                DialogResult = DialogResult.OK
            };
            janela.Controls.AddRange(new Control[] { senha, entrar });
            janela.AcceptButton = entrar;
            if (janela.ShowDialog(dono) != DialogResult.OK) return null;

            MySqlConnectionStringBuilder dados = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Port = 3306,
                UserID = "root",
                Password = senha.Text,
                Database = "simulador_gravitacional"
            };
            RepositorioSimulacoes banco = new RepositorioSimulacoes(dados.ConnectionString);
            await banco.InicializarBancoAsync();
            await banco.TestarAsync();
            repositorio = banco; // Somente em memoria durante esta execucao.
            return banco;
        }
    }
}
