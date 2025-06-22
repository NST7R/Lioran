using UnityEngine;
using TMPro;
using System.Collections.Generic;

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
        interactionPromptUI.SetActive(false);
        // Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void RefreshCurrentPrompt(IInteractable source)
    {
        if (source == null || followTarget == null)
            return;

        var actions = source.GetAvailableActions();
        ShowInteractionPrompt(actions, followTarget);
    }


public void ShowInteractionPrompt(Dictionary<KeyCode, string> actions, Transform target)
    {
        if (promptText == null || interactionPromptUI == null)
            return;

        // Génère le texte à partir du dictionnaire
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (var kvp in actions)
        {
            sb.AppendLine(kvp.Value);// Texte complet déjà formaté
        }

        promptText.text = sb.ToString().Trim(); // Trim pour éviter un retour à la ligne en trop
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
