
/// <summary>
/// Interface à implémenter par tous les objets interactifs dans le monde.
/// Sert à centraliser la détection et l'exécution de l'action contextuelle (ex: purifier, activer...).
/// </summary>
using System.Collections.Generic;
using UnityEngine;
public interface IInteractable
{
   /// <summary>
    /// Retourne les actions disponibles, avec leur touche d’activation.
    /// Exemple : { [E] => "Purifier", [F] => "Récolter" }
    /// </summary>
    /// Clé = touche (KeyCode), Valeur = message à afficher (texte personnalisé complet)
    Dictionary<KeyCode, string> GetAvailableActions();

    /// <summary>
    /// Exécute l’action correspondant à la touche pressée.
    /// </summary>
    void Interact(KeyCode key);
}
