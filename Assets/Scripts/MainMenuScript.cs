using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    private const string HealthKey = "LioranHealth";
    private const string CheckpointX = "SavedX";
    private const string CheckpointY = "SavedY";
    private const string LastScene = "LastScene";
    private const string LeafKey = "LeafCount";

    public string firstScene = "TheForgottenDephth_GA";

    public void NewGame()
    {
        PlayerPrefs.DeleteAll(); // or delete specific keys
        PlayerPrefs.SetInt("NewGameStarted", 1); // <- Add this
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstScene);
    }


    public void ContinueGame()
    {
        string sceneToLoad = PlayerPrefs.GetString(LastScene, firstScene);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
