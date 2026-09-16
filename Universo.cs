namespace SimuladorGavitacional
{
	class Universo
	{
		//array de corpos
		public Corpo[] C;
		//array de nomes
		public string[] Nomes = new string[100];

		//limites dos corpos
		public double EscalaUniverso { get; set; }

		public double MassaMin { get; set; }
		public double MassaMax { get; set; }

		public double DensidadeMin { get; set; }
		public double DensidadeMax { get; set; }

		public double VelocidadeMin { get; set; }
		public double VelocidadeMax { get; set; }

		public Universo()
		{
			C = new Corpo[1000];

			//lê o arquivo com os nomes aleatórios e coloca todos no array de nomes
			string aux = File.ReadAllText("nomes_corpos_simulador.txt");
			Nomes = aux.Split(';');
		}


		//gera os corpos aleatórios com base na quantidade e nos limites estabelecidos pelo usuario
		public void GerarCorposAleatorios(int quantidade)
		{
			Random random = new Random();

			for (int i = 0; i < quantidade; i++)
			{
				Corpo corpo = new Corpo();

				corpo.Nome = Nomes[random.Next(Nomes.Length)];

				corpo.Massa = MassaMin + (random.NextDouble() * (MassaMax - MassaMin));

				corpo.Densidade = DensidadeMin + (random.NextDouble() * (DensidadeMax - DensidadeMin));

				corpo.VelX = VelocidadeMin + (random.NextDouble() * (VelocidadeMax - VelocidadeMin));
				corpo.VelY = VelocidadeMin + (random.NextDouble() * (VelocidadeMax - VelocidadeMin));



				C[i] = corpo;

			}

		}

		public void GerarCorposAleatorios(int altura, int largura, Universo u)
		{
			Random random = new Random();
			for (int i = 0; i < u.C.Length; i++)
			{
				if (C[i] == null)
				{
					return;
				}
				C[i].PosX = random.NextInt64(altura);
				C[i].PosY = random.NextInt64(largura);
			}
		}




	}
}
