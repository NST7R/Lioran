using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class FallingObject : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float destroyDelay = 0.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Lioran"))
        {
            LioranHealth health = collision.collider.GetComponent<LioranHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }

        Destroy(gameObject, destroyDelay);
    }
}
