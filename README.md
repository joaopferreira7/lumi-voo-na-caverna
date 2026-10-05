# Lumi — Voo na Caverna

Jogo 2D feito em **Unity 6 (6000.4.1f1)** para o Trabalho Individual de Desenvolvimento de Jogos Digitais.
A jogabilidade é **inspirada em Flappy Bird**, com identidade e mecânicas próprias.

Você controla **Lumi**, um vaga-lume que atravessa uma caverna escura cheia de cristais. A cada impulso Lumi sobe,
e a gravidade a puxa de volta. Passe pelos vãos entre os cristais, colete orbes de luz e sobreviva o máximo possível.

## Como jogar

| Ação | Controle |
|------|----------|
| Voar (impulso) | `Espaço`, `W`, `↑`, clique esquerdo ou toque |
| Reiniciar após o fim | `Espaço`, clique ou `R` |

- **+1 ponto** ao atravessar cada par de cristais.
- **+3 pontos** ao coletar um orbe de luz.
- **3 vidas**: tocar um cristal ou o chão tira uma vida e deixa Lumi invencível (piscando) por 1,5 s.
- A velocidade aumenta conforme a pontuação. O recorde fica salvo entre as sessões.

## Diferenças em relação ao Flappy Bird

- Sistema de **vidas com invencibilidade temporária**, em vez de morte instantânea.
- **Orbes de luz** coletáveis que dão pontos bônus.
- **Dificuldade progressiva**: o cenário e os obstáculos aceleram com a pontuação.
- O chão faz Lumi **quicar** em vez de encerrar o jogo.
- Tema próprio: caverna com **parallax**, partículas de poeira luminosa, rastro de luz e tremor de câmera.

## Conceitos da disciplina aplicados

| Conceito | Onde |
|----------|------|
| Controle de entradas (teclado, mouse, toque) | `InputHelper.cs` |
| Movimentação com **física** (Rigidbody2D, gravidade, impulso) | `PlayerController.cs` |
| Movimentação via **Transform** | `ScrollMover.cs`, `LoopingScroller.cs`, `Orb.cs`, flutuação na tela inicial |
| Colisões e gatilhos (Collision2D / Trigger2D, tags) | `PlayerController.cs` (cristais, chão, zona de pontuação, orbes) |
| Pontuação, vidas, vitória/derrota, recorde (PlayerPrefs) | `GameManager.cs` |
| Máquina de estados (Pronto → Jogando → Fim de jogo) | `GameManager.cs` (`GameState`) |
| Geração procedural de obstáculos (Instantiate / Destroy, prefabs) | `ObstacleSpawner.cs` |
| Interface e feedback (HUD, corações, telas, sons, partículas, tremor de câmera) | Canvas da cena, `CameraShake.cs` |

## Estrutura

```
Assets/
  Scenes/Game.unity          Cena principal
  Scripts/                   Código C#
  Prefabs/                   CrystalPair (obstáculo) e LightOrb (orbe)
  Art/                       Sprites gerados proceduralmente
  Audio/                     Efeitos sonoros gerados proceduralmente
```

Todos os sprites e sons foram gerados por código dentro do projeto. Não há assets de terceiros.

## Executar

1. Abra a pasta do projeto no **Unity Hub** (Unity 6000.4.1f1 ou superior).
2. Abra `Assets/Scenes/Game.unity`.
3. Clique em **Play**.
