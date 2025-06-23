using UnityEngine;

public class NewGameInitializer : MonoBehaviour
{
    void Start()
    {
        // Only clear saves if New Game was explicitly started
        if (PlayerPrefs.GetInt("NewGameStarted", 0) == 1)
        {
            // Optionally clear only certain keys here
            PlayerPrefs.DeleteKey("LioranHealth");
            PlayerPrefs.DeleteKey("LeafCount");
            PlayerPrefs.DeleteKey("SavedX");
            PlayerPrefs.DeleteKey("SavedY");
            PlayerPrefs.SetInt("NewGameStarted", 0); // Reset flag so it doesn't clear again
            PlayerPrefs.Save();
        }
    }
}
