using UnityEngine;

/// <summary>
/// Move o objeto para a esquerda via Transform enquanto o jogo não terminou.
/// Usado pelos cristais e orbes (destruídos ao sair da tela).
/// </summary>
public class ScrollMover : MonoBehaviour
{
    public float speed = 2.5f;
    public float destroyX = -14f;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm != null && gm.State == GameState.GameOver) return;

        float mult = gm != null ? gm.SpeedMultiplier : 1f;
        transform.position += Vector3.left * speed * mult * Time.deltaTime;
        if (transform.position.x < destroyX) Destroy(gameObject);
    }
}
