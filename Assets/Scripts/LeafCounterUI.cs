using UnityEngine;
using TMPro;

/// <summary>
/// UI statique affichant le compteur de feuilles purifiées.
/// Attacher ce script à un TextMeshProUGUI dans un Canvas.
/// </summary>
public class LeafCounterUI : MonoBehaviour
{
    private static TextMeshProUGUI leafText;

    private void Awake()
    {
        leafText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        // Attend que LeafCounterManager soit initialisé
        if (LeafCounterManager.Instance != null)
            UpdateLeafDisplay(LeafCounterManager.Instance.leafCount);
    }

    /// <summary>
    /// Met à jour l’affichage du nombre de feuilles purifiées.
    /// </summary>
    public static void UpdateLeafDisplay(int count)
    {
        if (leafText != null)
            leafText.text = count.ToString();
        else
            Debug.LogWarning("LeafCounterUI: Aucun TextMeshProUGUI trouvé !");
    }
}
