using UnityEngine;
using System.Collections;

public class UIFadePrompt : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    private Coroutine currentFade;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            Debug.LogError("UIFadePrompt requires a CanvasGroup component!");
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeCanvasGroup(1f));
    }

    public void FadeOut()
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeCanvasGroup(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return FadeCanvasGroup(0f);
        gameObject.SetActive(false);
    }
}
