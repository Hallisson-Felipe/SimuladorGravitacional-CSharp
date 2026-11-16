namespace SimuladorGavitacional
{
    public class Quadro
    {
        public int Iteracao { get; set; }
        public Corpo?[] Corpos { get; set; }

        public Quadro(int iteracao, Corpo?[] corpos)
        {
            Iteracao = iteracao;
            Corpos = corpos;
        }
    }
}

