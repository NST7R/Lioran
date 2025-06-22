using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportToCave : MonoBehaviour
{
    public string sceneToLoad = "CaveScene";     // Set this in Inspector
    public GameObject fadeObject;                // Assign the Fade Panel GameObject with ScreenFader component
    public UIFadePrompt fadePrompt;               // Assign your TMP prompt GameObject with UIFadePrompt component

    private ScreenFader screenFader;
    private bool playerInRange = false;
    private bool isFading = false;

    private void Awake()
    {
        if (fadeObject != null)
        {
            screenFader = fadeObject.GetComponent<ScreenFader>();
            if (screenFader == null)
                Debug.LogError("TeleportToCave: Fade object doesn't have ScreenFader!");
        }
        else
        {
            Debug.LogError("TeleportToCave: Fade object not assigned!");
        }

        if (fadePrompt != null)
            fadePrompt.gameObject.SetActive(false); // start hidden
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isFading)
        {
            StartCoroutine(FadeAndTeleport());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInRange = true;
            if (fadePrompt != null)
                fadePrompt.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInRange = false;
            if (fadePrompt != null)
                fadePrompt.FadeOut();
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        isFading = true;

        if (fadePrompt != null)
            fadePrompt.FadeOut();

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeOut());

        SceneManager.LoadScene(sceneToLoad);
    }
}
