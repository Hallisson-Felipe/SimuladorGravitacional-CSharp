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
            universo.EscalaUniverso = 500;
            this.WindowState = FormWindowState.Maximized;
        }


        private void FormUniverso_Load(object sender, EventArgs e)
        {
            panelUniverso.Invalidate();
        }

        private void panelUniverso_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < universo.C.Length; i++)
            {
                Corpo corpo = universo.C[i];
                if (corpo != null)
                {
                    if (corpo.PosX == 0 || corpo.PosY == 0)
                    {
                        universo.GerarPosicoes(panelUniverso.Width, panelUniverso.Height, universo);
                    }
                    float raioPixels = (float)(corpo.CalcularRaio() * universo.EscalaUniverso);

                    float x = (float)corpo.PosX - raioPixels;
                    float y = (float)corpo.PosY - raioPixels;

                    float diametro = raioPixels * 2;
                    using (SolidBrush brush = new SolidBrush(corpo.Cor))
                    {
                        //pinta o corpo
                        e.Graphics.FillEllipse(
                            brush,
                            x,
                            y,
                            diametro,
                            diametro
                        );

                        //faz um ponto no centro do corpo
                        e.Graphics.FillEllipse(
                            Brushes.Black,
                            (float)corpo.PosX - 2,
                            (float)corpo.PosY - 2,
                            4,
                            4
                        );


                        //escreve o nome do corpo
                        e.Graphics.DrawString(
                            corpo.Nome,
                            this.Font,
                            Brushes.Black,
                            x,
                            y - 20
                        );
                                        }
                }
            }
        }
    }
}