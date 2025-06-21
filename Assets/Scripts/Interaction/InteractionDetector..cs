using UnityEngine;

/// <summary>
/// Détecte les objets interactifs à proximité du joueur grâce à un collider 2D.
/// À attacher sur un enfant du joueur contenant un Collider2D (trigger).
/// </summary>
public class InteractionDetector : MonoBehaviour
{
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        inputHandler = GetComponentInParent<PlayerInputHandler>(); // Cherche le script d'input sur le joueur
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            inputHandler.SetInteractable(interactable);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
            inputHandler.SetInteractable(null); // Efface le prompt quand on quitte la zone
    }
}
