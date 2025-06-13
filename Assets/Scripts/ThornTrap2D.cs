/// <summary>
/// ThornTrap2D
/// Inflige des dégâts immédiats lors du contact (ex : ronces, pics).
/// </summary>

using UnityEngine;

public class ThornTrap2D : MonoBehaviour
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
