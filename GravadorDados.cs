namespace SimuladorGavitacional;

//classe abstrata de persistência das simulações.
internal abstract class GravadorDados
{
    //metodo para criar uma nova simulação e registra o estado inicial dos corpos.
    public abstract Task<long> CriarAsync(int intervaloMs, double deltaTempo, Corpo?[] corpos);

    //metodo para gravar os estados dos corpos em uma iteração da simulação.
    public abstract Task GravarAsync(long id, int iteracao, Corpo?[] corpos);

    //metodo para listar as simulações armazenadas.
    public abstract Task<List<SimulacaoSalva>> ListarAsync();

    //metodo para carregar os metadados e os quadros de uma simulação para reprodução.
    public abstract Task<(int IntervaloMs, double DeltaTempo, List<Quadro> Quadros)> CarregarAsync(long id);
}

