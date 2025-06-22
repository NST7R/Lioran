using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    public Transform vfxLaunchPoint;
    private IInteractable currentInteractable;
    private Transform interactableTransform;

    public void SetInteractable(IInteractable interactable, Transform origin)
    {
        currentInteractable = interactable;
        interactableTransform = origin;

        if (interactable != null)
            UIManager.Instance.ShowInteractionPrompt(interactable.GetInteractionPrompt(), interactableTransform);
        else
            UIManager.Instance.HideInteractionPrompt();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey) && currentInteractable != null)
            currentInteractable.Interact();
    }
}
