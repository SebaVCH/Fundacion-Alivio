using UnityEngine;
using TMPro;
using System.Collections;

public class TextAnimator : MonoBehaviour
{
    public TextMeshProUGUI textMesh; // Para UI
                                     // public TextMeshPro textMesh; // Para texto 3D

    public float fadeSpeed = 1f;
    public float scaleSpeed = 0.5f;

    void Start()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        // Fade In
        Color color = textMesh.color;
        color.a = 0;
        textMesh.color = color;

        float elapsed = 0;
        while (elapsed < fadeSpeed)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsed / fadeSpeed);
            textMesh.color = color;
            yield return null;
        }
    }
}