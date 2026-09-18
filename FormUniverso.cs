using System.Diagnostics;

namespace SimuladorGavitacional
{
    partial class FormUniverso : Form
    {

        private Universo universo;
        private int altura;
        private int largura;
        public FormUniverso(Universo u)
        {
            InitializeComponent();
            universo = u;
            universo.EscalaUniverso = 300;
            this.WindowState = FormWindowState.Maximized;
        }


        private void FormUniverso_Load(object sender, EventArgs e)
        {
            using (Graphics graphics = panelUniverso.CreateGraphics())
            {
                universo.GerarPosicoes(panelUniverso.Height, panelUniverso.Width, universo, graphics);
            }

            panelUniverso.Invalidate(); 
        }

        private void panelUniverso_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                Corpo corpo = universo.Corpos[i];

                if (corpo != null)
                {
                    float raioPixels = (float)(corpo.CalcularRaio() * universo.EscalaUniverso);

                    float x = (float)corpo.PosX - raioPixels;
                    float y = (float)corpo.PosY - raioPixels;
                    float diametro = raioPixels * 2;

                    using (SolidBrush brush = new SolidBrush(corpo.Cor))
                    {
                        e.Graphics.FillEllipse(brush, x, y, diametro, diametro);

                        e.Graphics.FillEllipse(Brushes.Black, (float)corpo.PosX - 2, (float)corpo.PosY - 2, 4, 4);

                        e.Graphics.DrawString(corpo.Nome, this.Font, Brushes.Black, x, y - 20);
                    }
                }
            }
        }
    }
}