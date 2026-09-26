using System;

namespace SimuladorGavitacional
{
    internal abstract class GravadorDados
    {
        public abstract void GravarPosicoesIniciais(Corpo?[] corpos, string caminhoArquivo, int qtdIteracoes, int intervaloIteracoes);
    }
}
