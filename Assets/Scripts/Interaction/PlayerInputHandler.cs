using UnityEngine;
using System.Collections.Generic;

public class PlayerInputHandler : MonoBehaviour
{

    private IInteractable currentInteractable;
    private Transform interactableTransform;

    public void SetInteractable(IInteractable interactable, Transform origin)
{
    currentInteractable = interactable;
    interactableTransform = origin;

    if (interactable != null)
        UIManager.Instance.ShowInteractionPrompt(interactable.GetAvailableActions(), interactableTransform);
    else
        UIManager.Instance.HideInteractionPrompt();
}

    private void Update()
    {
        if (currentInteractable == null)
            return;

        Dictionary<KeyCode, string> actions = currentInteractable.GetAvailableActions();

        foreach (var kvp in actions)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                currentInteractable.Interact(kvp.Key);
                break; // Une seule action à la fois
            }
        }
    }
}
