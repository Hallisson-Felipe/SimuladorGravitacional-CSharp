namespace SimuladorGavitacional
{
    //contrato abstrato de persistencia das simulacoes
    public abstract class GravadorDados
    {
        //persiste a simulacao completa no banco a partir do arquivo local temporario
        public abstract Task<long> SalvarSimulacaoAsync(
            int intervaloMs,
            double deltaTempo,
            int ultimaIteracao,
            string caminhoArquivo);

        //lista as simulacoes armazenadas
        public abstract Task<List<SimulacaoSalva>> ListarAsync();

        //carrega os metadados e os quadros de uma simulacao para reproducao
        public abstract Task<(int IntervaloMs, double DeltaTempo, List<Quadro> Quadros)> CarregarAsync(long id);
    }
}
