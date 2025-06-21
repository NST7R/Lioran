using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TheForgottenDephth_GA");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
