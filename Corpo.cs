using System.Drawing.Drawing2D;

namespace SimuladorGavitacional
{
	internal class Corpo
	{

		public Color Cor { get; set; }
		public Region AreaOcupada { get; set; }
		public string Nome { get; set; }
		public double Massa { get; set; }
		public double Densidade { get; set; }
		public double PosX { get; set; }
		public double PosY { get; set; }
		public double VelX { get; set; }
		public double VelY { get; set; }

		public double CalcularRaio()
		{
			double raio = Math.Cbrt((3 * Massa) / (4 * Math.PI * Densidade));
			return raio;
		}
	}
}
