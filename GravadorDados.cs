namespace SimuladorGavitacional
{
    // Contrato abstrato de persistência das simulações.
    public abstract class GravadorDados
    {
        // Cria uma nova simulação e registra o estado inicial dos corpos.
        public abstract Task<long> CriarAsync(int intervaloMs, double deltaTempo, Corpo?[] corpos);

        // Grava os estados dos corpos em uma iteração da simulação.
        public abstract Task GravarAsync(long id, int iteracao, Corpo?[] corpos);

        // Lista as simulações armazenadas.
        public abstract Task<List<SimulacaoSalva>> ListarAsync();

        // Carrega os metadados e os quadros de uma simulação para reprodução.
        public abstract Task<(int IntervaloMs, double DeltaTempo, List<Quadro> Quadros)> CarregarAsync(long id);
    }
}
