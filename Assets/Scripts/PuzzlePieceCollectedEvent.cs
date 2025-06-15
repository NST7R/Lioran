using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Événement ScriptableObject déclenché lorsqu'une pièce est collectée.
/// Peut être écouté par tout système intéressé.
/// </summary>
[CreateAssetMenu(menuName = "Puzzle/OnPieceCollectedEvent")]
public class PuzzlePieceCollectedEvent : ScriptableObject
{
    public UnityAction<PuzzlePieceData> OnPieceCollected;

    public void Raise(PuzzlePieceData piece)
    {
        OnPieceCollected?.Invoke(piece);
    }
}
