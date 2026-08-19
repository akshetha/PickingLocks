using UnityEngine;
using TMPro;

public class Jiggle : MonoBehaviour
{
    public float speed = 40f;
    public float amount = 1.5f;

    TMP_Text text;

    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    void Update()
    {
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int matIndex = charInfo.materialReferenceIndex;
            int vertIndex = charInfo.vertexIndex;
            Vector3[] verts = textInfo.meshInfo[matIndex].vertices;

            Vector3 shake = new Vector3(
                Random.Range(-amount, amount),
                Random.Range(-amount, amount),
                0
            );

            for (int j = 0; j < 4; j++)
            {
                verts[vertIndex + j] += shake;
            }
        }

        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}