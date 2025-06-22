using UnityEngine;

/// <summary>
/// Gère les entrées du joueur (ex: touche E pour interaction).
/// Affiche les prompts d’interaction et déclenche les actions des objets à portée.
/// À attacher sur le GameObject du joueur.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Touche configurable pour interagir

    public Transform vfxLaunchPoint; // Assigné via l'inspecteur
    private IInteractable currentInteractable;     // Objet actuellement détecté
    private Transform interactableTransform;       // Pour positionner l’UI au bon endroit

    /// <summary>
    /// Appelé par un trigger de détection pour définir l’objet interactif courant.
    /// </summary>
    public void SetInteractable(IInteractable interactable, Transform origin)
    {
        currentInteractable = interactable;
        interactableTransform = origin;

        if (interactable != null)
        {
            string prompt = interactable.GetInteractionPrompt();
            UIManager.Instance.ShowInteractionPrompt(prompt, interactableTransform);
        }
        else
        {
            UIManager.Instance.HideInteractionPrompt();
        }
    }

    private void Update()
    {
        // Déclenche l’action de l’objet si la touche est pressée
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact(vfxLaunchPoint);
        }
    }
}
