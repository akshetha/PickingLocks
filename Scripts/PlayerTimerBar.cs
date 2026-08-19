using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerTimerBar : MonoBehaviour
{
    [Header("Timer Settings")]
    public float maxTime = 60f;
    private float currentTime;

    [Header("UI")]
    public Image timerBarFill;

    void Start()
    {
        currentTime = maxTime;
        if (timerBarFill != null)
            timerBarFill.color = Color.red;
    }


    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(currentTime, 0f);

        if (timerBarFill != null)
        {
            timerBarFill.fillAmount = currentTime / maxTime;
            float ratio = currentTime / maxTime;
            timerBarFill.color = Color.Lerp(Color.black, Color.red, ratio);
        }

        if (currentTime <= 0f)
            SceneManager.LoadScene("end1");
    }

    public void LoseTime(float seconds)
    {
        currentTime = Mathf.Max(currentTime - seconds, 0f);
    }
}