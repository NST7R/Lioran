using UnityEngine;

<<<<<<< HEAD

=======
>>>>>>> NeoIha
/// <summary>
/// Gère les entrées du joueur (ex: touche E pour interaction).
/// Affiche les prompts d’interaction et déclenche les actions des objets à portée.
/// À attacher sur le GameObject du joueur.
/// </summary>
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Touche configurable pour interagir

<<<<<<< HEAD
    private IInteractable currentInteractable; // Objet en cours de détection

    /// <summary>
    /// Détermine quel objet est actuellement interactif et affiche son prompt si nécessaire.
    /// </summary>
    public void SetInteractable(IInteractable interactable)
    {
        currentInteractable = interactable;

        if (interactable != null)
            UIManager.Instance.ShowInteractionPrompt(interactable.GetInteractionPrompt());
        else
            UIManager.Instance.HideInteractionPrompt();
=======
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
>>>>>>> NeoIha
    }

    private void Update()
    {
<<<<<<< HEAD
        // Si le joueur appuie sur la touche d’interaction
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact(transform); // Appelle l'interaction de l'objet en cours
=======
        // Déclenche l’action de l’objet si la touche est pressée
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
        {
            currentInteractable.Interact(transform);
>>>>>>> NeoIha
        }
    }
}
