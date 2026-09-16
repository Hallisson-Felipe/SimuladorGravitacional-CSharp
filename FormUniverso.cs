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
			largura = panelUniverso.Width;
			altura = panelUniverso.Height;
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
						universo.GerarCorposAleatorios(largura, altura, universo);
					}
					float x = (float)corpo.PosX;
					float y = (float)corpo.PosY;

					e.Graphics.FillEllipse(
						Brushes.Blue,
						x,
						y,
						(float)((corpo.CalcularRaio() * 2) * universo.EscalaUniverso),
						(float)((corpo.CalcularRaio() * 2) * universo.EscalaUniverso)
					);


					MessageBox.Show(corpo.CalcularRaio().ToString());
					//MessageBox.Show("Raio infinito? " +
					//	double.IsInfinity(corpo.CalcularRaio()));

					//MessageBox.Show("Escala infinita? " +
					//	double.IsInfinity(universo.EscalaUniverso));

					//MessageBox.Show("Resultado infinito? " +
					//	double.IsInfinity(corpo.CalcularRaio() * 2 * universo.EscalaUniverso));

				}
			}
		}
	}
}
