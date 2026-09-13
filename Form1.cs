
namespace SimuladorGavitacional
{
    public partial class Form1 : Form
    {

        Universo universo { get; set; }
        public Form1()
        {
            InitializeComponent();
            universo = new Universo();
            universo.larguraUniverso = panelUniverso.Width;
            universo.AlturaUniverso = panelUniverso.Height;

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

            universo.GerarCorposAleatorios((int)numericQuant.Value);

            panelUniverso.Invalidate();
        }

        //metodo pra mostrar os corpos
        private void PanelUniverso_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < universo.C.Length; i++)
            {
                Corpo corpo = universo.C[i];

                if (corpo != null)
                {
                    float x = (float)corpo.PosX;
                    float y = (float)corpo.PosY;

                    e.Graphics.FillEllipse(
                        Brushes.Blue,
                        x,
                        y,
                        10,
                        10
                    );
                }
            }
        }
    }
}
