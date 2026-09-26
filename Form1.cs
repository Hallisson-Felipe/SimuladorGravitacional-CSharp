
namespace SimuladorGavitacional
{
    public partial class Form1 : Form
    {

        Universo universo { get; set; }
        public Form1()
        {
            InitializeComponent();
            universo = new Universo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //evento ativado quando btnGerar e clicado
        private void btnGerar_Click(object sender, EventArgs e)
        {
            //loop que garante que os corpos nao serao gerados ate serem validados
            while (!ValidarValores())
            {
                return;
            }
            //define os limites do corpo
            universo.MassaMin = (double)numericMassaMin.Value;
            universo.MassaMax = (double)numericMassaMax.Value;

            universo.VelocidadeMin = (double)numericVelMin.Value;
            universo.VelocidadeMax = (double)numericVelMax.Value;

            //chama o metodo que gera os corpos
            universo.GerarCorposAleatorios((int)numericQuant.Value);

            //instancia e mostra o formulario onde o universo sera exibido
            FormUniverso formUniverso = new FormUniverso(universo);
            formUniverso.Show();

        }

        //metodo que valida os valores
        public bool ValidarValores()
        {
            if (numericMassaMin.Value > numericMassaMax.Value)
            {
                MessageBox.Show("A massa mínima não pode ser maior que a massa máxima.");
                return false;
            }

            if (numericVelMin.Value > numericVelMax.Value)
            {
                MessageBox.Show("A velocidade mínima não pode ser maior que a velocidade máxima.");
                return false;
            }

            return true;
        }
    }
}
