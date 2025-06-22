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
        {
            Debug.LogError("UIFadePrompt: CanvasGroup component missing!");
        }
        else
        {
            Debug.Log("UIFadePrompt: CanvasGroup found, alpha = " + canvasGroup.alpha);
        }
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn called");
        if (currentFade != null)
            StopCoroutine(currentFade);

        gameObject.SetActive(true);
        currentFade = StartCoroutine(FadeCanvasGroup(1f));
    }

    public void FadeOut()
    {
        Debug.Log("FadeOut called");
        if (currentFade != null)
            StopCoroutine(currentFade);

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
            Debug.Log($"Fading alpha: {canvasGroup.alpha}");
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
        Debug.Log($"Fade complete, alpha: {canvasGroup.alpha}");
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return FadeCanvasGroup(0f);
        gameObject.SetActive(false);
        currentFade = null;
        Debug.Log("FadeOut complete and object deactivated");
    }
}
