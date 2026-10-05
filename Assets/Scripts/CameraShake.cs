using UnityEngine;

/// <summary>
/// Tremor de câmera como feedback ao sofrer dano.
/// </summary>
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    Vector3 origin;
    float timeLeft;
    float duration;
    float strength;

    void Awake()
    {
        Instance = this;
        origin = transform.localPosition;
    }

    public void Shake(float shakeDuration, float shakeStrength)
    {
        duration = timeLeft = shakeDuration;
        strength = shakeStrength;
    }

    void LateUpdate()
    {
        if (timeLeft <= 0f) return;
        timeLeft -= Time.deltaTime;
        float k = Mathf.Clamp01(timeLeft / duration);
        transform.localPosition = timeLeft > 0f ? origin + (Vector3)Random.insideUnitCircle * strength * k : origin;
    }
}
