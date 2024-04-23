using TMPro;
using UnityEngine;

public class rainbowText : MonoBehaviour
{
    public TMP_Text textMesh;
    public float colorChangeSpeed = 1.0f;
    public float saturation = 1.0f;
    public float startDelay = 1.0f;
    public bool isPerText = false;
    private float timeElapsed;

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= startDelay)
        {
            if(isPerText) perCharacter();
            else fullTextAnim();
        }
    }

    void perCharacter() {
        TMP_TextInfo textInfo = textMesh.textInfo;
        int characterCount = textInfo.characterCount;

        for (int i = 0; i < characterCount; i++) {
            if (textInfo.characterInfo[i].isVisible) {
                float hue = Mathf.Repeat(Time.time * colorChangeSpeed + (float)i / characterCount, 1);
                Color32 color = Color.HSVToRGB(hue, saturation, 1);
                int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;
            }
        }

        // Update the mesh with the new colors
        textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
    }

    void fullTextAnim() {
        float hue = Mathf.Repeat(Time.time * colorChangeSpeed, 1);
        Color color = Color.HSVToRGB(hue, saturation, 1);
        textMesh.color = color;
    }
}
