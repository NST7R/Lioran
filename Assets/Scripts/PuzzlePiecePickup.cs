using UnityEngine;

/// <summary>
/// Déclencheur de ramassage de pièce.
/// Appelle PuzzlePieceManager pour placer la pièce automatiquement.
/// </summary>
public class PuzzlePiecePickup : MonoBehaviour
{
    [Tooltip("Identifiant unique de la pièce (doit correspondre au slot ciblé)")]
    public string pieceID;

    [Tooltip("Sprite à afficher une fois la pièce placée dans le puzzle")]
    public Sprite pieceSprite;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lioran")) // Assure-toi que le joueur a bien ce tag
        {
            // Utiliser la méthode recommandée par Unity 2023+
            PuzzlePieceManager ppm = Object.FindFirstObjectByType<PuzzlePieceManager>();

            if (ppm != null)
            {
                ppm.CollectPiece(pieceID, pieceSprite);
            }
            else
            {
                Debug.LogWarning("Aucun PuzzlePieceManager trouvé dans la scène.");
            }

            Destroy(gameObject); // Détruit la pièce une fois ramassée
        }
    }
}
