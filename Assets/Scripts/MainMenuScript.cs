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

        // 2. Stop all sounds
        AudioManager.Instance?.StopAllSounds();

        // 3. Show cutscene and fade in
        cutsceneScreen.gameObject.SetActive(true);
        cutscenePlayer.gameObject.SetActive(true);
        cutscenePlayer.Play();

        yield return FadeManager.Instance.FadeIn(1f);

        // 4. Wait until video finishes
        while (cutscenePlayer.isPlaying)
        {
            yield return null;
        }

        // 5. Fade out after video
        yield return FadeManager.Instance.FadeOut(1f);

        // 6. Hide cutscene visuals
        cutscenePlayer.gameObject.SetActive(false);
        cutsceneScreen.gameObject.SetActive(false);

        // 7. Load first scene and hook up callback to restore audio
        SceneManager.sceneLoaded += OnFirstSceneLoaded;
        SceneManager.LoadScene(firstScene);
    }

    // Called once after scene loads to restore music/ambience/run audio
    private void OnFirstSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnFirstSceneLoaded;
        StartCoroutine(DelayedAudioSetup());
    }

    private IEnumerator DelayedAudioSetup()
    {
        yield return null; // wait one frame

        if (AudioManager.Instance != null)
        {
            // Reinitialize audio properly
            AudioManager.Instance.StopAllSounds(); // clear old state

            // AudioManager.OnSceneLoaded will run automatically
            // Optionally start the run loop now if needed
            AudioManager.Instance.StartRunLoop();
        }
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
