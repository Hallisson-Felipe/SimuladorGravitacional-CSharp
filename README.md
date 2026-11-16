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
- Salvamento das simulações no banco de dados MySQL.
- Listagem das simulações salvas.
- Carregamento e reprodução de simulações anteriores.

## 🛠️ Tecnologias

- C#
- .NET 8
- Windows Forms
- MySQL
- System.Drawing

## ⚙️ Funcionamento

A simulação calcula a força gravitacional entre os corpos utilizando a Lei da Gravitação Universal de Newton:

**F = G × (m₁ × m₂) / r²**

A partir da força resultante, são calculadas a aceleração, a velocidade e a nova posição de cada corpo a cada iteração.

O simulador também realiza o tratamento de colisões entre os corpos, atualizando o estado da simulação conforme os eventos detectados.

## 💾 Persistência de dados

As simulações podem ser armazenadas em um banco de dados MySQL.

São registrados os dados de cada simulação, incluindo suas configurações e os estados dos corpos ao longo das iterações.

Também é possível consultar as simulações salvas e carregar seus dados para reproduzir uma execução anterior.

## 💻 Como executar

Clone o repositório:

```bash
git clone https://github.com/Hallisson-Felipe/SimuladorGravitacional-CSharp.git
```

Abra a solução no Visual Studio 2022 ou superior, com suporte ao .NET 8.

Configure a conexão com o banco de dados MySQL e execute a aplicação.

## 📚 Sobre o projeto

Desenvolvido como trabalho acadêmico do curso de **Sistemas de Informação**, aplicando conceitos de programação orientada a objetos, estruturas de dados, física computacional, interfaces gráficas e persistência de dados.ca computacional e interfaces gráficas.
