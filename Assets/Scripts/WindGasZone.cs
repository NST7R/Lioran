using UnityEngine;

/// <summary>
/// WindGasZone
/// Zone de gaz où il faut se déplacer selon une séquence de vent.
/// </summary>
public class WindGasZone : MonoBehaviour
{
    [SerializeField] private string[] correctDirections; // "Up", "Right", etc.
    private int currentStep = 0;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            string input = GetInput();
            if (input == correctDirections[currentStep])
            {
                currentStep++;
                if (currentStep >= correctDirections.Length)
                {
                    Debug.Log("Séquence correcte ! Passage autorisé.");
                }
            }
            else
            {
                currentStep = 0;
                Debug.Log("Séquence incorrecte");
            }
        }
    }

    private string GetInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) return "Up";
        if (Input.GetKeyDown(KeyCode.RightArrow)) return "Right";
        if (Input.GetKeyDown(KeyCode.LeftArrow)) return "Left";
        if (Input.GetKeyDown(KeyCode.DownArrow)) return "Down";
        return "";
    }
}

