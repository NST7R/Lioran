using UnityEngine;
using UnityEngine.SceneManagement;



public class MainMenuScript : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("TheForgottenDepths");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
