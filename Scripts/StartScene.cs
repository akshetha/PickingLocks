using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private KeyCode[] numberKeys = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7,
        KeyCode.Alpha8, KeyCode.Alpha9,
        KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7,
        KeyCode.Keypad8, KeyCode.Keypad9
    };

    void Update()
    {
        foreach (KeyCode key in numberKeys)
        {
            if (Input.GetKeyDown(key))
            {
                SceneManager.LoadScene("DialogueScene");
                return;
            }
        }
    }
}