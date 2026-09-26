# 🌌 Simulador Gravitacional

Simulador gravitacional bidimensional desenvolvido em **C# com Windows Forms e .NET 8**, que representa a interação entre corpos celestes utilizando a Lei da Gravitação Universal de Newton.

## 🚀 Funcionalidades

- Geração de corpos com massa, densidade e velocidade aleatórias.
- Distribuição dos corpos sem sobreposição inicial.
- Cálculo da força gravitacional e atualização do movimento.
- Detecção e tratamento de colisões.
- Visualização gráfica dos corpos em tempo real.
- Controle de execução e velocidade da simulação.
- Exibição das informações dos corpos a cada iteração.
- Persistência dos dados iniciais em arquivo de texto.

## 🛠️ Tecnologias

- C#
- .NET 8
- Windows Forms
- System.Drawing

## ⚙️ Funcionamento

A simulação calcula a força gravitacional entre os corpos utilizando a equação:

**F = G × (m₁ × m₂) / r²**

A partir da força resultante, são calculadas a aceleração, a velocidade e a nova posição de cada corpo a cada iteração.

## 💻 Como executar

Clone o repositório:

    git clone https://github.com/Hallisson-Felipe/SimuladorGravitacional-CSharp.git

Abra o projeto no Visual Studio 2022 ou superior, com suporte ao .NET 8, e execute a aplicação.

## 📚 Sobre o projeto

Desenvolvido como trabalho acadêmico do curso de **Sistemas de Informação**, aplicando conceitos de programação orientada a objetos, estruturas de dados, física computacional e interfaces gráficas.
