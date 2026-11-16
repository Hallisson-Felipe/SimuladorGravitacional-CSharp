namespace SimuladorGavitacional
{
    public class SimulacaoSalva
    {
        public long Id { get; set; }
        public DateTime CriadaEm { get; set; }
        public int UltimaIteracao { get; set; }

        public SimulacaoSalva(long id, DateTime criadaEm, int ultimaIteracao)
        {
            Id = id;
            CriadaEm = criadaEm;
            UltimaIteracao = ultimaIteracao;
        }
    }
}

