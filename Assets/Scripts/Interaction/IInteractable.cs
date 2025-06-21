using UnityEngine;
/// <summary>
/// Interface à implémenter par tous les objets interactifs dans le monde.
/// Sert à centraliser la détection et l'exécution de l'action contextuelle (ex: purifier, activer...).
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Retourne le texte du prompt affiché à l’écran (ex : "Purifier (E)").
    /// </summary>
    string GetInteractionPrompt();

    /// <summary>
    /// Appelé quand le joueur interagit avec l’objet.
    /// </summary>
    void Interact(Transform player);
}
