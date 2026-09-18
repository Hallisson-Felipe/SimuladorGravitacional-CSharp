
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

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }


        //quando o usuario apertar o botao ativa o evento
        private void btnGerar_Click(object sender, EventArgs e)
        {
            //seta os limites do corpo
            universo.MassaMin = (double)numericMassaMin.Value;
            universo.MassaMax = (double)numericMassaMax.Value;

            universo.VelocidadeMin = (double)numericVelMin.Value;
            universo.VelocidadeMax = (double)numericVelMax.Value;

            universo.DensidadeMax = 22590.0;
            universo.DensidadeMin = 0.0899;

            universo.EscalaUniverso = 500;


            universo.GerarCorposAleatorios((int)numericQuant.Value);

            FormUniverso formUniverso = new FormUniverso(universo);
            formUniverso.Show();

        }

        //metodo pra mostrar os		
        private void PanelUniverso_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Titulo_Click(object sender, EventArgs e)
        {

        }
    }
}
