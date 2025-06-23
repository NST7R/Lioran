using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TeleportToCave : MonoBehaviour
{
    public string sceneToLoad = "CaveScene";     
    public GameObject fadeObject;                
    public UIFadePrompt fadePrompt;               

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
            fadePrompt?.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInRange = false;
            fadePrompt?.FadeOut();
        }
    }

    private IEnumerator FadeAndTeleport()
    {
        isFading = true;

        fadePrompt?.FadeOut();

        if (screenFader != null)
            yield return StartCoroutine(screenFader.FadeOut());

        // Save checkpoint BEFORE teleporting
        SaveCheckpointBeforeSceneChange();

        SceneManager.LoadScene(sceneToLoad);
    }

    private void SaveCheckpointBeforeSceneChange()
    {
        GameObject lioranObj = GameObject.FindGameObjectWithTag("Lioran");
        if (lioranObj == null) return;

        Vector3 pos = lioranObj.transform.position;
        string scene = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetFloat(scene + "_SavedX", pos.x);
        PlayerPrefs.SetFloat(scene + "_SavedY", pos.y);
        PlayerPrefs.SetString("SavedScene", scene);
        PlayerPrefs.Save();

        Debug.Log($"Saved checkpoint before teleport at {pos} in scene '{scene}'");
    }
}
