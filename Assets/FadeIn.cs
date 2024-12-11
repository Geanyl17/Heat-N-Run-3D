using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Required for CanvasGroup

public class SceneFadeIn : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup; // Reference to the CanvasGroup for fade effect (set in the inspector)
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        // Start the fade-in effect as soon as the scene starts
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        // Ensure the CanvasGroup is fully opaque at the start
        fadeCanvasGroup.alpha = 1f;

        // Fade from opaque (alpha = 1) to transparent (alpha = 0)
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the CanvasGroup is fully transparent at the end
        fadeCanvasGroup.alpha = 0f;
    }
}
