using UnityEngine;

/// <summary>
/// Gera pares de cristais em intervalos regulares com altura aleatória.
/// O intervalo acompanha a velocidade do jogo, mantendo a distância entre os obstáculos.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public GameObject orbPrefab;
    public float spawnInterval = 1.8f;
    public float minY = -1.8f;
    public float maxY = 1.8f;
    [Range(0f, 1f)] public float orbChance = 0.35f;

    float timer;

    void Update()
    {
        var gm = GameManager.Instance;
        if (gm == null || gm.State != GameState.Playing) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            Spawn();
            timer = spawnInterval / gm.SpeedMultiplier;
        }
    }

    void Spawn()
    {
        Vector3 pos = transform.position + Vector3.up * Random.Range(minY, maxY);
        Instantiate(obstaclePrefab, pos, Quaternion.identity);

        // Orbe de luz bônus entre um obstáculo e o próximo
        if (orbPrefab && Random.value < orbChance)
        {
            float halfGap = spawnInterval * 0.5f * 2.5f; // metade da distância entre obstáculos
            Vector3 orbPos = transform.position + new Vector3(halfGap, Random.Range(minY, maxY), 0f);
            Instantiate(orbPrefab, orbPos, Quaternion.identity);
        }
    }
}
