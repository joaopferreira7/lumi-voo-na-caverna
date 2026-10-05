using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { Ready, Playing, GameOver }

/// <summary>
/// Controla o estado do jogo (Pronto, Jogando, Fim de Jogo), pontuação, vidas,
/// velocidade progressiva e a interface.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Regras")]
    public int maxLives = 3;
    public float speedIncreasePerPoint = 0.02f;
    public float maxSpeedMultiplier = 1.7f;
    public float restartDelay = 0.7f;

    [Header("UI")]
    public Text scoreText;
    public Image[] hearts;
    public Color heartFullColor = new Color(1f, 0.35f, 0.45f);
    public Color heartEmptyColor = new Color(1f, 1f, 1f, 0.15f);
    public GameObject hudPanel;
    public GameObject readyPanel;
    public GameObject gameOverPanel;
    public Text finalScoreText;
    public Text bestScoreText;
    public Text newRecordText;

    [Header("Áudio")]
    public AudioSource sfxSource;
    public AudioClip scoreClip;
    public AudioClip orbClip;
    public AudioClip hitClip;
    public AudioClip gameOverClip;

    public GameState State { get; private set; } = GameState.Ready;
    public int Score { get; private set; }
    public int Lives { get; private set; }

    /// <summary>Multiplicador de velocidade aplicado aos obstáculos e ao cenário.</summary>
    public float SpeedMultiplier => Mathf.Min(maxSpeedMultiplier, 1f + Score * speedIncreasePerPoint);

    const string BestKey = "Lumi_BestScore";
    float gameOverTime;

    void Awake()
    {
        Instance = this;
        Lives = maxLives;
    }

    void Start()
    {
        SetState(GameState.Ready);
    }

    void Update()
    {
        // Pequeno atraso evita reiniciar sem querer enquanto o jogador ainda aperta o botão
        if (State == GameState.GameOver && Time.time - gameOverTime > restartDelay && InputHelper.RestartPressed())
            Restart();
    }

    public void StartGame()
    {
        if (State != GameState.Ready) return;
        SetState(GameState.Playing);
    }

    public void AddScore(int amount, bool isBonus = false)
    {
        if (State != GameState.Playing) return;
        Score += amount;
        UpdateHud();
        PlaySfx(isBonus ? orbClip : scoreClip);
    }

    /// <summary>Remove uma vida. Retorna true se o jogador continua vivo.</summary>
    public bool LoseLife()
    {
        if (State != GameState.Playing) return false;

        Lives = Mathf.Max(0, Lives - 1);
        UpdateHud();
        PlaySfx(hitClip);
        if (CameraShake.Instance) CameraShake.Instance.Shake(0.25f, 0.25f);

        if (Lives <= 0)
        {
            GameOver();
            return false;
        }
        return true;
    }

    void GameOver()
    {
        int best = PlayerPrefs.GetInt(BestKey, 0);
        bool newRecord = Score > best;
        if (newRecord)
        {
            best = Score;
            PlayerPrefs.SetInt(BestKey, best);
            PlayerPrefs.Save();
        }

        if (finalScoreText) finalScoreText.text = "Pontos: " + Score;
        if (bestScoreText) bestScoreText.text = "Recorde: " + best;
        if (newRecordText) newRecordText.gameObject.SetActive(newRecord && Score > 0);

        gameOverTime = Time.time;
        PlaySfx(gameOverClip);
        if (CameraShake.Instance) CameraShake.Instance.Shake(0.4f, 0.4f);
        SetState(GameState.GameOver);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetState(GameState newState)
    {
        State = newState;
        if (readyPanel) readyPanel.SetActive(newState == GameState.Ready);
        if (gameOverPanel) gameOverPanel.SetActive(newState == GameState.GameOver);
        if (hudPanel) hudPanel.SetActive(newState == GameState.Playing);
        UpdateHud();
    }

    void UpdateHud()
    {
        if (scoreText) scoreText.text = Score.ToString();
        if (hearts == null) return;
        for (int i = 0; i < hearts.Length; i++)
            if (hearts[i]) hearts[i].color = i < Lives ? heartFullColor : heartEmptyColor;
    }

    void PlaySfx(AudioClip clip)
    {
        if (sfxSource && clip) sfxSource.PlayOneShot(clip);
    }
}
