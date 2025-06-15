using UnityEngine;

/// <summary>
/// Déclencheur de ramassage de pièce.
/// Appelle PuzzlePieceManager pour placer la pièce automatiquement.
/// </summary>
public class PuzzlePiecePickup : MonoBehaviour
{
    public string pieceID; // Identifiant de la pièce (doit correspondre à ExpectedPieceID)
    public Sprite pieceSprite; // Sprite de la pièce à afficher dans le slot

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lioran")) // Remplace par le tag de ton joueur
        {
            PuzzlePieceManager ppm = FindObjectOfType<PuzzlePieceManager>();
            ppm.CollectPiece(pieceID, pieceSprite);

            Destroy(gameObject); // Supprime la pièce de la scène après collecte
        }
    }
}
