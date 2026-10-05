using UnityEngine;

/// <summary>
/// Orbe de luz coletável que vale pontos bônus. Pulsa e flutua para chamar atenção.
/// </summary>
public class Orb : MonoBehaviour
{
    public int points = 3;
    public float bobAmplitude = 0.2f;
    public float bobSpeed = 4f;
    public float pulseSpeed = 6f;
    public Transform visual;

    float phase;

    void Start()
    {
        phase = Random.value * Mathf.PI * 2f;
    }

    void Update()
    {
        if (!visual) return;
        float t = Time.time + phase;
        visual.localPosition = Vector3.up * Mathf.Sin(t * bobSpeed) * bobAmplitude;
        visual.localScale = Vector3.one * (1f + Mathf.Sin(t * pulseSpeed) * 0.12f);
    }

    public void Collect()
    {
        Destroy(gameObject);
    }
}
