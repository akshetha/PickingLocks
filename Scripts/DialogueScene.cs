using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueScene : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text dialogueText;
    //public AudioSource typingSound;

    public TMP_Text promptText;

    public float typingSpeed = 0.07f;


    private string[] speakers = {
        "???", "???", "???", "???", "???", "???", "???",
        "Stunning Woman", "Stunning Woman", "Stunning Woman", "Stunning Woman", "Stunning Woman", "Stunning Woman", "Stunning Woman", "Stunning Woman"
    };

    private string[] lines = {
        "*Rumbled Groan*",
        "...An explorer?",
        "How bold of you to step into my sleeping chambers.",
        "You know there is a price for disturbing my sleep...",
        "I haven't eaten since I last woke up...",
        "How ... long has it been?",
        "A thousand years...",
        "...",
        "Here, how about this?",
        "I'll let you go free if you can do something for me.",
        "This is going to sound a little funny...",
        "I'll put it in simple terms for you.",
        "It seems my hair's been getting all over the place.",
        "Can you fix my bedhead?",
        "...",
    };

    private int gameStartIndex = 17; //dia + 2
    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogueDone = false;
    private bool inputCooldown = false;
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
        typingCoroutine = StartCoroutine(TypeLine(currentLine));
    }

    void Update()
    {

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
                SceneManager.LoadScene("into_vid");
                return;
            }

            currentLine++;

            if (currentLine == gameStartIndex)
            {
                SceneManager.LoadScene("into_vid");
                return;
            }

            if (currentLine >= lines.Length)
            {
                dialogueDone = true;
                promptText.text = "Press any number to leave...";
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
            //if (c != ' ') typingSound.Play(); // don't play on spaces
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        promptText.text = "Press any number to continue...";
    }

    bool AnyNumberPressed()
    {
        foreach (KeyCode key in numberKeys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        return false;
    }

    IEnumerator InputCooldown()
    {
        inputCooldown = true;
        yield return new WaitForSeconds(0.2f);
        inputCooldown = false;
    }
}