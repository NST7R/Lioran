using UnityEngine;

/// <summary>
/// Contient tous les emplacements de pièces et vérifie la complétion du puzzle.
/// </summary>
public class PuzzlePlatform : MonoBehaviour
{
    [SerializeField] private PuzzleSlot[] slots; // Emplacements des pièces sur la plateforme
    [SerializeField] private GameObject triggerToActivate; // Événement (porte, effet) déclenché à la complétion

    /// <summary>
    /// Place une pièce dans le slot correspondant à son ID.
    /// </summary>
    public void PlacePiece(string pieceID, Sprite sprite)
    {
        foreach (var slot in slots)
        {
            if (slot.ExpectedPieceID == pieceID && !slot.IsFilled)
            {
                slot.Place(sprite); // Place la pièce visuellement
                break;
            }
        }

        // Vérifie si toutes les pièces ont été placées
        if (IsPuzzleComplete() && triggerToActivate != null)
        {
            triggerToActivate.SetActive(true); // Active l’événement lié au puzzle
        }
    }

    /// <summary>
    /// Vérifie si tous les slots sont remplis.
    /// </summary>
    private bool IsPuzzleComplete()
    {
        foreach (var slot in slots)
        {
            if (!slot.IsFilled)
                return false;
        }
        return true;
    }
}
