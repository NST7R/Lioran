using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportToCave : MonoBehaviour
{
    public string sceneToLoad = "CaveScene";     // Set this in the Inspector
    public GameObject fadeObject;                // Drag the Fade Panel GameObject here
    public GameObject interactPrompt;            // Optional: assign UI prompt

    private ScreenFader screenFader;
    private bool playerInRange = false;
    private bool isFading = false;

    private void Awake()
    {
        if (fadeObject != null)
        {
            screenFader = fadeObject.GetComponent<ScreenFader>();
            if (screenFader == null)
            {
                Debug.LogError("Fade object does not have a ScreenFader component.");
            }
        }
        else
        {
            Debug.LogError("Fade object not assigned.");
        }
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
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        isFading = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeOut());

        SceneManager.LoadScene(sceneToLoad);
    }
}
