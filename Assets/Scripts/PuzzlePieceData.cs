using UnityEngine;

/// <summary>
/// Données de chaque pièce de puzzle.
/// Un asset ScriptableObject est créé par pièce.
/// </summary>
[CreateAssetMenu(menuName = "Puzzle/Piece Data")]
public class PuzzlePieceData : ScriptableObject
{
    public string id;            // Identifiant unique de la pièce
    public Sprite sprite;        // Sprite affiché dans le slot
}
