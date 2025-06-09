/// <summary>
/// ToxicMushroomTrap2D
/// Déclenche un nuage toxique et inflige des dégâts tant que le joueur reste dans la zone.
/// </summary>

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ToxicMushroomTrap2D : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 5;
    [SerializeField] private float damageInterval = 1f;
    [SerializeField] private ParticleSystem gasEffect;

    private bool isPlayerInside = false;
    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        isPlayerInside = true;
        timer = 0f;

        gasEffect?.Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) isPlayerInside = false;
    }

    private void Update()
    {
        if (!isPlayerInside) return;

        timer += Time.deltaTime;
        if (timer >= damageInterval)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
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
