using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float despawnX = -12f;
    public Transform player;
    public float hitRadius = 0.5f;

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (player != null)
        {
            float dist = Vector2.Distance(transform.position, player.position);
            if (dist < hitRadius)
            {
                PlayerTimerBar timerBar = player.GetComponent<PlayerTimerBar>();
                if (timerBar != null)
                    timerBar.LoseTime(15f); // loses 15 seconds on hit

                Destroy(gameObject);
                return;
            }
        }

        if (transform.position.x < despawnX)
            Destroy(gameObject);
    }
}