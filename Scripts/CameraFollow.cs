using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 1f;
    private bool isPanning = false;

    void Start()
    {
        Camera.main.orthographicSize = 5; // wider view
    }

    void LateUpdate()
    {
        if (!isPanning && target != null)
        {
            Vector3 desired = new Vector3(target.position.x + 7, 0, -10);
            transform.position = Vector3.Lerp(transform.position, desired, 1.5f * Time.deltaTime);
        }
    }

    public IEnumerator PanToPosition(Vector3 targetPos, float duration)
    {
        isPanning = true;
        Vector3 start = transform.position;
        Vector3 end = new Vector3(targetPos.x, 0, -10);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            transform.position = Vector3.Lerp(start, end, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        isPanning = false;
    }
}