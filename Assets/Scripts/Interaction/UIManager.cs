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

    private Transform followTarget;
    private void Awake()
    {
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowInteractionPrompt(string text, Transform target)
    {
        promptText.text = text;
        interactionPromptUI.SetActive(true);
        followTarget = target;
    }

public void HideInteractionPrompt()
{
    if (interactionPromptUI != null)
    {
        interactionPromptUI.SetActive(false);
    }
}


    private void LateUpdate()
    {
        if (followTarget != null && interactionPromptUI != null && interactionPromptUI.activeSelf)
        {
            // Position UI légèrement au-dessus de l’objet (ex : +1 en Y)
            Vector3 worldPos = followTarget.position + Vector3.up * 1.5f;

            // Convertit en position de monde (si le Canvas est World Space)
            interactionPromptUI.transform.position = worldPos;

            // Optionnel : faire face à la caméra
            interactionPromptUI.transform.forward = Camera.main.transform.forward;
        }
    }

}
