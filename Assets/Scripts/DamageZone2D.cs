/// <summary>
/// DamageZone2D
/// Zone qui inflige des dégâts continus (ex : eau stagnante, acide).
/// </summary>

using UnityEngine;

public class DamageZone2D : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 5;
    [SerializeField] private float damageInterval = 1f;

    private float timer = 0f;
    private bool playerInside = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInside = false;
    }

    private void Update()
    {
        if (!playerInside) return;

        timer += Time.deltaTime;
        if (timer >= damageInterval)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var health = player.GetComponent<LioranHealth>();
                if (health != null)
                    health.TakeDamage(damagePerSecond);
            }
            timer = 0f;
        }
    }
}
