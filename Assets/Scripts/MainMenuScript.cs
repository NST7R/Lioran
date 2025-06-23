using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public string firstScene = "TheForgottenDephth_GA";
    public CanvasGroup mainMenuUI; // Assign in Inspector

    [Header("Cutscene Settings")]
    public VideoPlayer cutscenePlayer; // Assign via Inspector
    public RawImage cutsceneScreen;    // Assign the RawImage showing the video

    public void NewGame()
    {
        StartCoroutine(PlayIntroCutscene());
    }

    private IEnumerator PlayIntroCutscene()
    {
        // Disable main menu UI interaction
        if (mainMenuUI != null)
        {
            mainMenuUI.interactable = false;
            mainMenuUI.blocksRaycasts = false;
        }

        // Clear saved data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("NewGameStarted", 1);
        PlayerPrefs.Save();

        // 1. Fade out UI
        yield return FadeManager.Instance.FadeOut(1f);

        // 2. Stop all music
        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllSounds();

        // 3. Show video screen and fade in
        cutsceneScreen.gameObject.SetActive(true);
        cutscenePlayer.gameObject.SetActive(true);
        cutscenePlayer.Play();

        // Fade in cutscene (from black to visible)
        yield return FadeManager.Instance.FadeIn(1f);

        // 4. Wait for video to finish
        while (cutscenePlayer.isPlaying)
        {
            yield return null;
        }

        // 5. Fade out after video
        yield return FadeManager.Instance.FadeOut(1f);

        // 6. Hide video screen
        cutscenePlayer.gameObject.SetActive(false);
        cutsceneScreen.gameObject.SetActive(false);

        // 7. Load the next scene
        SceneManager.LoadScene(firstScene);
    }



    public void ContinueGame()
    {
        string sceneToLoad = PlayerPrefs.GetString("LastScene", firstScene);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
