/// <summary>
/// ThornTrap2D
/// Inflige des dégâts immédiats lors du contact (ex : ronces, pics).
/// </summary>

using UnityEngine;

public class ThornTrap2D : MonoBehaviour
{
    [SerializeField] private int damage = 15;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var health = collision.GetComponent<LioranHealth>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
