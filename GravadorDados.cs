using System;

namespace SimuladorGavitacional
{
    // Contrato de persistência do estado: outra implementação pode trocar
    // o arquivo texto por banco de dados sem alterar quem salva ou carrega.
    internal abstract class GravadorDados
    {
        public abstract void GravarPosicoesIniciais(Corpo?[] corpos, string caminhoArquivo, int qtdIteracoes, int intervaloIteracoes);

        public abstract Corpo[] CarregarPosicoesIniciais(
            string caminhoArquivo,
            out int qtdIteracoes,
            out int intervaloIteracoes);
    }
}
