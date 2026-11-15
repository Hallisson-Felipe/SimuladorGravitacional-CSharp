namespace SimuladorGavitacional
{
    // Contrato abstrato de persistência das simulações.
    public abstract class GravadorDados
    {
        // Persiste a simulação completa no banco a partir do arquivo local temporário.
        public abstract Task<long> SalvarSimulacaoAsync(
            int intervaloMs,
            double deltaTempo,
            int ultimaIteracao,
            string caminhoArquivo);

        // Lista as simulações armazenadas.
        public abstract Task<List<SimulacaoSalva>> ListarAsync();

        // Carrega os metadados e os quadros de uma simulação para reprodução.
        public abstract Task<(int IntervaloMs, double DeltaTempo, List<Quadro> Quadros)> CarregarAsync(long id);
    }
}
