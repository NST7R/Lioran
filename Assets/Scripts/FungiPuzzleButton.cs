using UnityEngine;

/// <summary>
/// FungiPuzzleButton
/// Champignon interactif dans un ordre défini.
/// </summary>
public class FungiPuzzleButton : MonoBehaviour
{
    public int buttonIndex;
    private FungiPuzzleManager manager;

    private void Start()
    {
        manager = FindObjectOfType<FungiPuzzleManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            manager?.PressButton(buttonIndex);
        }
    }
}
