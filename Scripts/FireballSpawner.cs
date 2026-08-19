using UnityEngine;

public class FireballSpawner : MonoBehaviour
{
    [Header("Fireball")]
    public GameObject fireballPrefab;
    public float spawnX = 12f;        // right side off screen
    public float despawnX = -12f;     // left side off screen
    public float moveSpeed = 5f;

    [Header("Timing")]
    public float minInterval = 3f;
    public float maxInterval = 7f;

    [Header("Player")]
    public Transform player;

    private float timer;

    void Start()
    {
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SpawnFireball();
            ResetTimer();
        }
    }

    void SpawnFireball()
    {
        if (fireballPrefab == null) return;

        Vector3 spawnPos = new Vector3(spawnX, player.position.y, 0f);
        GameObject fb = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        Fireball fireball = fb.GetComponent<Fireball>();
        if (fireball != null)
        {
            fireball.moveSpeed = moveSpeed;
            fireball.despawnX = despawnX;
            fireball.player = player;
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minInterval, maxInterval);
    }
}