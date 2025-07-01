using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager Instance;

    [Header("UI Elements")]
    public Image fadeImage;
    public TextMeshProUGUI dialogueText;
    public float fadeDuration = 1.5f;
    public float textDisplayDuration = 5f;

    [Header("Scene Settings")]
    public string menuSceneName = "MainMenu";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
            Color c = dialogueText.color;
            c.a = 0f;
            dialogueText.color = c;
        }
        if (fadeImage != null)
            fadeImage.color = new Color(0, 0, 0, 0);
    }

    public void StartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        dialogueText.text = "";
        dialogueText.gameObject.SetActive(false);

        dialogueText.gameObject.SetActive(false);
        yield return Fade(0, 1);

        dialogueText.text =
            "With the barrier lifted, the hidden grove is revealed—untouched by decay.\n" +
            "Some spirits welcome Lioran with hope in their eyes.\n" +
            "Others turn away, hurt by his absence.\n" +
            "But as light returns to the forest, even doubt begins to fade.\n" +
            "At last, they are free.";
        dialogueText.gameObject.SetActive(true);
        yield return FadeTextAlpha(0, 1, 1f);
        yield return new WaitForSeconds(textDisplayDuration);
        yield return FadeTextAlpha(1, 0, 1f);
        dialogueText.gameObject.SetActive(false);

        yield return Fade(1, 0);
        yield return new WaitForSeconds(1f);

        dialogueText.gameObject.SetActive(false);
        yield return Fade(0, 1);

        dialogueText.text =
            "As the last petal returns to the Tree of Life, a warm light spreads across the land.\n" +
            "The decay retreats, and the spirits begin to awaken.\n" +
            "Lioran stands still…\n" +
            "and the forest breathes again.";
        dialogueText.gameObject.SetActive(true);
        yield return FadeTextAlpha(0, 1, 1f);
        yield return new WaitForSeconds(textDisplayDuration);
        yield return FadeTextAlpha(1, 0, 1f);
        dialogueText.gameObject.SetActive(false);

        yield return Fade(1, 0);
        yield return new WaitForSeconds(1f);

        dialogueText.gameObject.SetActive(false);
        yield return Fade(0, 1);

        dialogueText.text = "To be continued...";
        dialogueText.gameObject.SetActive(true);
        yield return FadeTextAlpha(0, 1, 1f);
        yield return new WaitForSeconds(3f);
        yield return FadeTextAlpha(1, 0, 1f);
        dialogueText.gameObject.SetActive(false);

        yield return Fade(1, 0);
        SceneManager.LoadScene(menuSceneName);
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        Color c = fadeImage.color;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            c.a = Mathf.Lerp(from, to, t);
            fadeImage.color = c;
            yield return null;
        }
        c.a = to;
        fadeImage.color = c;
    }

    private IEnumerator FadeTextAlpha(float from, float to, float duration)
    {
        float t = 0f;
        Color c = dialogueText.color;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            c.a = Mathf.Lerp(from, to, t);
            dialogueText.color = c;
            yield return null;
        }
        c.a = to;
        dialogueText.color = c;
    }
}
