using UnityEngine;

/// <summary>
/// Controla a Lumi (o vaga-lume): impulso via física (Rigidbody2D), inclinação conforme a
/// velocidade, dano ao tocar cristais (com invencibilidade temporária), quique no chão,
/// coleta de orbes e pontuação ao atravessar os obstáculos.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float flapForce = 6.5f;
    public float maxUpAngle = 30f;
    public float maxDownAngle = -70f;
    public float rotationSpeed = 8f;
    public float ceilingY = 4.2f;
    public float groundBounce = 7f;

    [Header("Tela inicial")]
    public float idleBobAmplitude = 0.25f;
    public float idleBobSpeed = 3f;

    [Header("Dano")]
    public float invulnerableTime = 1.5f;
    public float blinkInterval = 0.1f;

    [Header("Visual / Áudio")]
    public SpriteRenderer[] renderers;
    public AudioSource audioSource;
    public AudioClip flapClip;

    Rigidbody2D rb;
    Vector3 startPos;
    float invulnerableUntil;

    bool IsInvulnerable => Time.time < invulnerableUntil;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Start()
    {
        // Parada enquanto aguarda o início da partida
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        switch (gm.State)
        {
            case GameState.Ready:
                // Flutua suavemente usando o Transform
                transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * idleBobSpeed) * idleBobAmplitude;
                if (InputHelper.FlapPressed())
                {
                    gm.StartGame();
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    Flap();
                }
                break;

            case GameState.Playing:
                if (InputHelper.FlapPressed()) Flap();
                // Teto da caverna: não deixa sair pelo topo
                if (transform.position.y > ceilingY && rb.linearVelocity.y > 0f)
                    rb.linearVelocity = new Vector2(0f, 0f);
                RotateByVelocity();
                break;

            case GameState.GameOver:
                RotateByVelocity();
                break;
        }

        UpdateBlink();
    }

    void Flap()
    {
        rb.linearVelocity = new Vector2(0f, flapForce);
        if (audioSource && flapClip) audioSource.PlayOneShot(flapClip);
    }

    void RotateByVelocity()
    {
        float target = Mathf.Clamp(rb.linearVelocity.y * 6f, maxDownAngle, maxUpAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, target), rotationSpeed * Time.deltaTime);
    }

    void UpdateBlink()
    {
        bool visible = !IsInvulnerable || Mathf.FloorToInt(Time.time / blinkInterval) % 2 == 0;
        foreach (var r in renderers)
            if (r) r.enabled = visible;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Chão da caverna: sólido. Causa dano e devolve a Lumi para cima.
        if (!collision.collider.CompareTag("Ground")) return;
        var gm = GameManager.Instance;
        if (gm.State != GameState.Playing) return;

        if (!IsInvulnerable) TakeHit();
        if (gm.State == GameState.Playing) rb.linearVelocity = new Vector2(0f, groundBounce);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        if (other.CompareTag("ScoreZone"))
        {
            gm.AddScore(1);
        }
        else if (other.CompareTag("Orb"))
        {
            var orb = other.GetComponent<Orb>();
            gm.AddScore(orb ? orb.points : 3, true);
            if (orb) orb.Collect(); else Destroy(other.gameObject);
        }
        else if (other.CompareTag("Obstacle") && !IsInvulnerable)
        {
            TakeHit();
        }
    }

    /// <summary>Aplica dano. Retorna true se ainda há vidas.</summary>
    bool TakeHit()
    {
        bool alive = GameManager.Instance.LoseLife();
        if (alive) invulnerableUntil = Time.time + invulnerableTime;
        else invulnerableUntil = 0f;
        return alive;
    }
}
