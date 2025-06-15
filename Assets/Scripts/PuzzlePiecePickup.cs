using UnityEngine;

/// <summary>
/// Rattaché à une pièce présente dans le monde. 
/// Déclenche l’événement global quand le joueur entre en contact avec l’objet.
/// </summary>
public class PuzzlePiecePickup : MonoBehaviour
{
    [SerializeField] private PuzzlePieceData pieceData; // Données de la pièce (ScriptableObject)
    [SerializeField] private PuzzlePieceCollectedEvent onPieceCollectedEvent; // Événement à déclencher

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lioran")) // Le joueur doit avoir ce tag
        {
            onPieceCollectedEvent.Raise(pieceData);
            Destroy(gameObject); // Supprime la pièce du monde
        }
    }
}
