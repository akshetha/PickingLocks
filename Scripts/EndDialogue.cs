using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndDialogue : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    public TMP_Text promptText;
    public GameObject bloc;
    public float dialogueDelay = 2f; // how many seconds before dialogue starts
    //public AudioSource typingSound;

    public float typingSpeed = 0.07f;

    private string[] speakers = {
        "Goddess", "Goddess", "Goddess", "Goddess", "Goddess", "Goddess"
    };

    private string[] lines = {
        "You actually did it...",
        "I didn't think you'd manage that.",
        "Perhaps you're worth keeping around after all.",
        "...",
        "What are you doing, still standing there?",
        "You can leave now."
    };

    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogueDone = false;
    private bool inputCooldown = false;
    private bool dialogueStarted = false;
    private Coroutine typingCoroutine;

    private KeyCode[] numberKeys = {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3,
        KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7,
        KeyCode.Alpha8, KeyCode.Alpha9,
        KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3,
        KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7,
        KeyCode.Keypad8, KeyCode.Keypad9
    };

    void Start()
    {
        promptText.text = "";
        dialogueText.text = "";
        if (bloc != null) bloc.SetActive(false);

        // start dialogue after a delay to let animation play first
        StartCoroutine(StartDialogueAfterDelay());
    }

    IEnumerator StartDialogueAfterDelay()
    {
        yield return new WaitForSeconds(dialogueDelay);
        if (bloc != null) bloc.SetActive(true);
        dialogueStarted = true;
        typingCoroutine = StartCoroutine(TypeLine(currentLine));
    }

    void Update()
    {
        if (!dialogueStarted) return;
        if (inputCooldown) return;

        if (AnyNumberPressed())
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                isTyping = false;
                dialogueText.text = $"{speakers[currentLine]}:  {lines[currentLine]}";
                promptText.text = "Press any number to continue...";
                StartCoroutine(InputCooldown());
                return;
            }

            if (dialogueDone)
            {
                SceneManager.LoadScene("credits");
                return;
            }

            currentLine++;

            if (currentLine >= lines.Length)
            {
                dialogueDone = true;
                promptText.text = "Press any number to exit...";
            }
            else
            {
                promptText.text = "";
                typingCoroutine = StartCoroutine(TypeLine(currentLine));
                StartCoroutine(InputCooldown());
            }
        }
    }

    IEnumerator TypeLine(int index)
    {
        isTyping = true;
        dialogueText.text = "";
        string fullText = $"{speakers[index]}:  {lines[index]}";
        string displayed = "";

        foreach (char c in fullText)
        {
            displayed += c;
            dialogueText.text = displayed;
            //if (typingSound != null && c != ' ') typingSound.Play();
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        promptText.text = "Press any number to continue...";
    }

    bool AnyNumberPressed()
    {
        foreach (KeyCode key in numberKeys)
            if (Input.GetKeyDown(key)) return true;
        return false;
    }

    IEnumerator InputCooldown()
    {
        inputCooldown = true;
        yield return new WaitForSeconds(0.2f);
        inputCooldown = false;
    }
}