using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    [SerializeField] private string TheSilentGlade_GA;
    [SerializeField] private float fadeDuration = 1f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Lioran"))
        {
            triggered = true;
            StartCoroutine(FadeAndLoad());
        }
    }

    private System.Collections.IEnumerator FadeAndLoad()
    {
        yield return FadeManager.Instance.FadeOut(fadeDuration);
        SceneManager.LoadScene(TheSilentGlade_GA);
    }
}
