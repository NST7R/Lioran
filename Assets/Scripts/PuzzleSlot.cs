using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Slot dans lequel une pièce est automatiquement placée si l’ID correspond.
/// </summary>
public class PuzzleSlot : MonoBehaviour
{
    [SerializeField] private string expectedID;       // ID attendu
    [SerializeField] private Image visualImage;       // Image affichant le sprite
    [SerializeField] private GameObject glowEffect;   // Feedback visuel optionnel

    /// <summary>
    /// Vérifie si cette slot accepte la pièce donnée.
    /// </summary>
    public bool Matches(string id)
    {
        return id == expectedID;
    }

    /// <summary>
    /// Place visuellement la pièce dans le slot.
    /// </summary>
    public void PlacePiece(PuzzlePieceData piece)
    {
        visualImage.sprite = piece.sprite;
        visualImage.enabled = true;

        if (glowEffect != null)
            glowEffect.SetActive(true);
    }
}
