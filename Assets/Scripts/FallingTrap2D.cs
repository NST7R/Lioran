/// <summary>
/// FallingTrap2D
/// Active un objet suspendu (roche, pic) qui tombe sur le joueur.
/// </summary>

using UnityEngine;

public class FallingTrap2D : MonoBehaviour
{
    [SerializeField] private Rigidbody2D fallingObject;
    [SerializeField] private float delay = 0.5f;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered || !collision.CompareTag("Player")) return;

        triggered = true;
        Invoke(nameof(ActivateFall), delay);
    }

    private void ActivateFall()
    {
        if (fallingObject != null)
            fallingObject.bodyType = RigidbodyType2D.Dynamic;
    }
}
