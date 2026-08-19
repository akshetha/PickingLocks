using UnityEngine;
using System.Collections;
using System;

public class PuzzlePanel1 : MonoBehaviour
{
    [Header("Left slot anchor positions")]
    public Transform slot1, slot2, slot3, slot4, slot5;

    [Header("Left moveable blocks")]
    public SpriteRenderer block1, block2, block3, block4, block5;

    [Header("Right static targets")]
    public SpriteRenderer target1, target2, target3, target4, target5;

    private Transform[] slots;
    private SpriteRenderer[] blocks;
    private Color[] trueColors = new Color[5];

    void Awake()
    {
        slots  = new Transform[]      { slot1, slot2, slot3, slot4, slot5 };
        blocks = new SpriteRenderer[] { block1, block2, block3, block4, block5 };
    }

    public void SetupTarget(Color[] colors)
    {
        target1.color = colors[0];
        target2.color = colors[1];
        target3.color = colors[2];
        target4.color = colors[3];
        target5.color = colors[4];
    }

    public void SetupLeft(Color[] colors)
    {
        for (int i = 0; i < 5; i++)
        {
            trueColors[i] = colors[i];
            blocks[i].color = colors[i];
            blocks[i].transform.position = slots[i].position;
        }
    }

    public void HighlightBlock(int idx, bool on)
    {
        blocks[idx].color = on ? Color.white : trueColors[idx];
    }

    public Vector3 GetSlotPosition(int idx)
    {
        return slots[idx].position;
    }

    public Vector3 GetMiddleSlotPosition()
    {
        return slots[2].position;
    }

    public IEnumerator ShiftBlocksWithPlayer(int from, int to, Transform player, Vector3 playerTarget, Action onComplete)
    {
        // Capture start positions by instance ID before any shuffling
        var startPos = new System.Collections.Generic.Dictionary<int, Vector3>();
        for (int i = 0; i < 5; i++)
            startPos[blocks[i].GetInstanceID()] = blocks[i].transform.position;

        // Do the shuffle
        SpriteRenderer movingBlock = blocks[from];
        Color movingColor = trueColors[from];

        if (from < to)
        {
            for (int i = from; i < to; i++)
            {
                blocks[i] = blocks[i + 1];
                trueColors[i] = trueColors[i + 1];
            }
        }
        else
        {
            for (int i = from; i > to; i--)
            {
                blocks[i] = blocks[i - 1];
                trueColors[i] = trueColors[i - 1];
            }
        }

        blocks[to] = movingBlock;
        trueColors[to] = movingColor;

        Vector3 playerStart = player.position;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = 1f - Mathf.Pow(1f - t, 3f);

            for (int i = 0; i < 5; i++)
            {
                float blockT = Mathf.Clamp01((elapsed - i * 0.03f) / duration);
                blockT = 1f - Mathf.Pow(1f - blockT, 3f);
                blocks[i].transform.position = Vector3.Lerp(
                    startPos[blocks[i].GetInstanceID()],
                    slots[i].position,
                    blockT
                );
            }

            player.position = Vector3.Lerp(playerStart, playerTarget, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < 5; i++)
            blocks[i].transform.position = slots[i].position;

        player.position = playerTarget;

        onComplete?.Invoke();
    }

    public bool CheckWin(Color[] target)
    {
        for (int i = 0; i < 5; i++)
            if (!ColorsMatch(trueColors[i], target[i])) return false;
        return true;
    }

    bool ColorsMatch(Color a, Color b) =>
        Mathf.Abs(a.r - b.r) < 0.05f &&
        Mathf.Abs(a.g - b.g) < 0.05f &&
        Mathf.Abs(a.b - b.b) < 0.05f;
}