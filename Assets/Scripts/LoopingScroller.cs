using UnityEngine;

/// <summary>
/// Faz um elemento de cenário (chão, teto, fundo) rolar em loop infinito.
/// "segmentWidth" deve ser a largura de um bloco que se repete no sprite.
/// "parallax" permite camadas de fundo mais lentas.
/// </summary>
public class LoopingScroller : MonoBehaviour
{
    public float speed = 2.5f;
    public float segmentWidth = 2f;
    [Range(0f, 1f)] public float parallax = 1f;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm != null && gm.State == GameState.GameOver) return;

        float mult = gm != null && gm.State == GameState.Playing ? gm.SpeedMultiplier : 1f;
        transform.position += Vector3.left * speed * parallax * mult * Time.deltaTime;
        if (transform.position.x <= startPos.x - segmentWidth)
            transform.position += Vector3.right * segmentWidth;
    }
}
