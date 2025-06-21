using UnityEngine;
using TMPro;

/// <summary>
/// Affiche les messages d’interaction (ex: "Appuyez sur E pour purifier") à l’écran.
/// Singleton accessible via UIManager.Instance.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject interactionPromptUI;   // UI contenant le texte
    [SerializeField] private TextMeshProUGUI promptText;       // Texte TMP à modifier dynamiquement

    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowInteractionPrompt(string text)
    {
        promptText.text = text;
        interactionPromptUI.SetActive(true);
    }

    public void HideInteractionPrompt()
    {
        interactionPromptUI.SetActive(false);
    }
}
