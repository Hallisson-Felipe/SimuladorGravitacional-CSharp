using System.Diagnostics;

namespace SimuladorGavitacional
{
    partial class FormUniverso : Form
    {

        private Universo universo;
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

            //faz um reload no panelUniverso
            panelUniverso.Invalidate(); 
        }


        //evento que "pinta" o panelUniverso assim que ele e gerado
        private void panelUniverso_Paint(object sender, PaintEventArgs e)
        {
            //loop para percorrer os corpos e desenha-los na tela
            for (int i = 0; i < universo.Corpos.Length; i++)
            {
                Corpo corpo = universo.Corpos[i];

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
    }
}