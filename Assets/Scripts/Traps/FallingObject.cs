using UnityEngine;

/// <summary>
/// Représente un objet tombant : inflige des dégâts au joueur.
/// Peut être détruit après un court délai.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class FallingObject : MonoBehaviour
{
    [Tooltip("Dégâts infligés au joueur")]
    [SerializeField] private int damage = 1;

    [Tooltip("Délai avant suppression après contact")]
    [SerializeField] private float destroyDelay = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lioran"))
        {
            LioranHealth health = collision.collider.GetComponent<LioranHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }

        Destroy(gameObject, destroyDelay); // Option : pooling à la place
    }
}
