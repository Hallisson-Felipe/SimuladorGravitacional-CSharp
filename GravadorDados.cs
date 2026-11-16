namespace SimuladorGavitacional
{
    public abstract class GravadorDados
    {
        //salva a simulacao completa no banco a partir do txt temporario
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
