using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SimuladorGavitacional
{
	internal class Corpo
	{

		public Color Cor { get; set; }
		public Region? AreaOcupada { get; set; }
		public string Nome { get; set; } = string.Empty;
		public double Massa { get; set; }
		public double Densidade { get; set; }
		public double PosX { get; set; }
		public double PosY { get; set; }
		public double VelX { get; set; }
		public double VelY { get; set; }

		//metodo para calcular o raio a partir da massa e densidade
		public double CalcularRaio()
		{
			if (Densidade <= 0)
			{
				return 0;
			}
			double raio = Math.Cbrt((3 * Massa) / (4 * Math.PI * Densidade));
			return raio;
		}

		//metodo que trata a colisao e funde dois corpos
		public void FundirCom(Corpo outroCorpo, double densidadeMax)
		{
			//soma as massas dos corpos
			double massaTotal = this.Massa + outroCorpo.Massa;

			if (massaTotal <= 0)
			{
				return;
			}

			//quantidade de movimento dos corpos nos eixos X e Y antes da colisao: Q = m * v
			double q1x = this.Massa * this.VelX;
			double q1y = this.Massa * this.VelY;

			double q2x = outroCorpo.Massa * outroCorpo.VelX;
			double q2y = outroCorpo.Massa * outroCorpo.VelY;

			//conservacao da quantidade de movimento: Q_total = Q1 + Q2
			double qTotalX = q1x + q2x;
			double qTotalY = q1y + q2y;

			//calcula as velocidades nos eixos X e Y apos a colisao: V = Q / M
			double novaVelX = qTotalX / massaTotal;
			double novaVelY = qTotalY / massaTotal;

			//media ponderada da densidade pelas massas dos corpos anteriores
			double novaDensidade = ((this.Massa * this.Densidade) + (outroCorpo.Massa * outroCorpo.Densidade)) / massaTotal;

			//limite fisico maximo para a densidade
			if (novaDensidade > densidadeMax)
			{
				novaDensidade = densidadeMax;
			}

			//calcula a posicao resultante pelo centro de massa dos dois corpos
			double novaPosX = ((this.Massa * this.PosX) + (outroCorpo.Massa * outroCorpo.PosX)) / massaTotal;
			double novaPosY = ((this.Massa * this.PosY) + (outroCorpo.Massa * outroCorpo.PosY)) / massaTotal;

			//mistura as cores com base na massa dos corpos
			int r = (int)(((this.Cor.R * this.Massa) + (outroCorpo.Cor.R * outroCorpo.Massa)) / massaTotal);
			int g = (int)(((this.Cor.G * this.Massa) + (outroCorpo.Cor.G * outroCorpo.Massa)) / massaTotal);
			int b = (int)(((this.Cor.B * this.Massa) + (outroCorpo.Cor.B * outroCorpo.Massa)) / massaTotal);
			Color novaCor = Color.FromArgb(Math.Clamp(r, 0, 255), Math.Clamp(g, 0, 255), Math.Clamp(b, 0, 255));

			//mantem o nome do corpo mais massivo
			if (outroCorpo.Massa > this.Massa)
			{
				this.Nome = outroCorpo.Nome;
			}

			//atualiza os atributos do corpo
			this.Massa = massaTotal;
			this.Densidade = novaDensidade;
			this.VelX = novaVelX;
			this.VelY = novaVelY;
			this.PosX = novaPosX;
			this.PosY = novaPosY;
			this.Cor = novaCor;
		}
	}
}

