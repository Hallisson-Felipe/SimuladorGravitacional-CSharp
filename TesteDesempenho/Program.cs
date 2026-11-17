using System.Diagnostics;
using SimuladorGavitacional;

Environment.CurrentDirectory = AppContext.BaseDirectory;

foreach (int quantidade in new[] { 30, 50 })
{
    // Os dois programas recebem os mesmos corpos e executam o mesmo trabalho.
    var aquecimento = CriarUniverso(quantidade);
    for (int i = 0; i < 1000; i++) aquecimento.ExecutarIteracao(0.0005);

    double[] tempos = new double[10];
    for (int teste = 0; teste < tempos.Length; teste++)
    {
        var universo = CriarUniverso(quantidade);
        var relogio = Stopwatch.StartNew();
        for (int i = 0; i < 1000; i++) universo.ExecutarIteracao(0.0005);
        relogio.Stop();
        tempos[teste] = relogio.Elapsed.TotalMilliseconds;
    }

    Array.Sort(tempos);
    double mediana = (tempos[4] + tempos[5]) / 2;
    Console.WriteLine($"{quantidade} corpos: {mediana:F2} ms em 1.000 iterações");
}

static Universo CriarUniverso(int quantidade)
{
    var universo = new Universo { EscalaUniverso = 300, EscalaVisualCorpos = 30 };
    universo.Corpos = Enumerable.Range(0, quantidade).Select(i => new Corpo
    {
        Nome = $"Corpo{i}",
        Massa = 0.001 + (i % 9) * 0.0001,
        Densidade = 1000,
        PosX = 100 + (i % 20) * 100,
        PosY = 100 + (i / 20) * 100,
        VelX = 0,
        VelY = 0
    }).ToArray();
    return universo;
}
