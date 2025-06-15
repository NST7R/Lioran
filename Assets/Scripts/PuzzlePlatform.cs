using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Gère la plateforme de puzzle.
/// Place automatiquement les pièces collectées dans les bons slots.
/// </summary>
public class PuzzlePlatform : MonoBehaviour
{
    [SerializeField] private PuzzlePieceCollectedEvent onPieceCollectedEvent;
    [SerializeField] private List<PuzzleSlot> slots; // Tous les slots du puzzle

    private HashSet<string> collectedPieceIDs = new();

    private void OnEnable()
    {
        onPieceCollectedEvent.OnPieceCollected += HandlePieceCollected;
    }

    private void OnDisable()
    {
        onPieceCollectedEvent.OnPieceCollected -= HandlePieceCollected;
    }

    /// <summary>
    /// Appelé automatiquement quand une pièce est collectée.
    /// </summary>
    private void HandlePieceCollected(PuzzlePieceData data)
    {
        if (collectedPieceIDs.Contains(data.id)) return; // Évite les doublons
        collectedPieceIDs.Add(data.id);

        foreach (var slot in slots)
        {
            if (slot.Matches(data.id))
            {
                slot.PlacePiece(data);
                break;
            }
        }
    }
}
