using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WireManager1 : MonoBehaviour
{
    [Header("Panels in order left to right")]
    public PuzzlePanel1[] panels;

    [Header("Character")]
    public Transform character;
    public float runDuration = 0.4f;

    [Header("Camera")]
    public CameraFollow cameraFollow;

    [Header("End Goal")]
    public Transform endGoalSprite;
    public string nextSceneName = "lev3";

    private int currentPanel = 0;
    private int selectedSlot = -1;
    private bool locked = false;

    private Color colorOrange = new Color(1f, 0.55f, 0f);
    private Color colorGreen  = new Color(0.1f, 0.8f, 0.1f);
    private Color colorPurple = new Color(0.5f, 0.1f, 0.9f);
    private Color colorRed    = new Color(0.9f, 0.1f, 0.1f);
    private Color colorBlue   = new Color(0.1f, 0.4f, 0.9f);

    private Color[][] puzzleTargets;

    void Start()
    {
        puzzleTargets = new Color[][]
        {
            new Color[] { colorGreen,  colorOrange, colorPurple, colorRed,   colorBlue   },
            new Color[] { colorBlue,   colorPurple, colorRed,    colorGreen, colorOrange },
            new Color[] { colorRed,    colorBlue,   colorGreen,  colorOrange,colorPurple },
            new Color[] { colorOrange, colorRed,    colorBlue,   colorPurple,colorGreen  },
        };

        for (int i = 0; i < panels.Length; i++)
            panels[i].SetupTarget(puzzleTargets[i % puzzleTargets.Length]);

        panels[0].SetupLeft(Shuffle((Color[])puzzleTargets[0].Clone()));

        MovePlayerToMiddleBlock(panels[0]);
    }

    void Update()
    {
        if (locked) return;

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) OnNumberPressed(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) OnNumberPressed(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) OnNumberPressed(3);
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) OnNumberPressed(4);
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) OnNumberPressed(5);
    }

    void OnNumberPressed(int slotNumber)
    {
        if (locked) return;
        if (currentPanel >= panels.Length) return;

        int idx = slotNumber - 1;
        PuzzlePanel1 panel = panels[currentPanel];

        if (selectedSlot == -1)
        {
            selectedSlot = idx;
            panel.HighlightBlock(idx, true);
            MovePlayerToBlock(panel, idx);
        }
        else
        {
            if (selectedSlot == idx)
            {
                panel.HighlightBlock(idx, false);
                selectedSlot = -1;
                MovePlayerToMiddleBlock(panel);
                return;
            }

            panel.HighlightBlock(selectedSlot, false);
            int fromSlot = selectedSlot;
            selectedSlot = -1;

            StartCoroutine(SwapAndFollowPlayer(panel, fromSlot, idx));
        }
    }

    IEnumerator SwapAndFollowPlayer(PuzzlePanel1 panel, int from, int to)
    {
        locked = true;

        Vector3 blockDestination = panel.GetSlotPosition(to);
        Vector3 playerTarget = blockDestination + new Vector3(-2.8f, 0, 0);

        yield return StartCoroutine(panel.ShiftBlocksWithPlayer(from, to, character, playerTarget, OnSwapComplete));

        locked = false;
    }

    void OnSwapComplete()
    {
        if (panels[currentPanel].CheckWin(puzzleTargets[currentPanel % puzzleTargets.Length]))
            StartCoroutine(OnPuzzleSolved());
    }

    IEnumerator OnPuzzleSolved()
    {
        locked = true;

        currentPanel++;

        if (currentPanel >= panels.Length)
        {
            yield return StartCoroutine(RunToEndGoal());
            yield break;
        }

        Color[] prevTarget = puzzleTargets[(currentPanel - 1) % puzzleTargets.Length];
        panels[currentPanel].SetupLeft(Shuffle((Color[])prevTarget.Clone()));

        Vector3 runTarget = panels[currentPanel].GetMiddleSlotPosition();
        runTarget += new Vector3(-2.8f, 0, 0);

        float elapsed = 0f;
        Vector3 startPos = character.position;

        while (elapsed < runDuration)
        {
            float t = elapsed / runDuration;
            t = t * t * (3f - 2f * t);
            character.position = Vector3.Lerp(startPos, runTarget, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        character.position = runTarget;

        locked = false;
    }

    IEnumerator RunToEndGoal()
    {
        if (endGoalSprite == null) yield break;

        Vector3 startPos = character.position;
        Vector3 goalPos = endGoalSprite.position + new Vector3(-1f, 0, 0);
        float elapsed = 0f;
        float runToDoor = 0.8f;

        while (elapsed < runToDoor)
        {
            float t = elapsed / runToDoor;
            t = t * t * (3f - 2f * t);
            character.position = Vector3.Lerp(startPos, goalPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        character.position = goalPos;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(nextSceneName);
    }

    void MovePlayerToBlock(PuzzlePanel1 panel, int idx)
    {
        Vector3 blockPos = panel.GetSlotPosition(idx);
        character.position = blockPos + new Vector3(-2.8f, 0, 0);
    }

    void MovePlayerToMiddleBlock(PuzzlePanel1 panel)
    {
        character.position = panel.GetMiddleSlotPosition() + new Vector3(-2.8f, 0, 0);
    }

    Color[] Shuffle(Color[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
        return arr;
    }
}