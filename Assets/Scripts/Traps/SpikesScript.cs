using UnityEngine;

public class SpikesScript : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            LioranHealth health = collision.GetComponent<LioranHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
