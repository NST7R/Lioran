/// <summary>
/// FallingTrap2D
/// Active un objet suspendu (roche, pic) qui tombe sur le joueur.
/// </summary>

using UnityEngine;

public class FallingTrap2D : MonoBehaviour
{[SerializeField] private int damage = 2;
    [SerializeField] private float dropDelay = 0.2f;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered) return;

        if (collision.CompareTag("Lioran"))
        {
            triggered = true;
            Invoke(nameof(EnableGravity), dropDelay);
        }
    }

    private void EnableGravity()
    {
        GetComponent<Rigidbody2D>().gravityScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lioran"))
        {
            LioranHealth health = collision.collider.GetComponent<LioranHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }

        // Optionnel : détruire après l'impact
        Destroy(gameObject, 1f);
    }
}