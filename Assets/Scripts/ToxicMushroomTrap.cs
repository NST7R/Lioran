using UnityEngine;
using System.Collections;

/// <summary>
/// Inflige des dégâts continus tant que le joueur reste dans le champ toxique.
/// Peut déclencher une particule de poison.
/// </summary>
public class ToxicMushroomTrap : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 1;           // Dégâts infligés à chaque tick
    [SerializeField] private float tickRate = 2f;           // Intervalle entre deux dégâts
    [SerializeField] private ParticleSystem poisonFX;         // Effet visuel du gaz toxique (optionnel)

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            if (poisonFX != null && !poisonFX.isPlaying)
                poisonFX.Play();

            LioranHealth health = collision.GetComponent<LioranHealth>();
            if (health != null)
                damageCoroutine = StartCoroutine(DamageOverTime(health));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            if (poisonFX != null && poisonFX.isPlaying)
                poisonFX.Stop();
        }
    }

    private IEnumerator DamageOverTime(LioranHealth health)
    {
        while (health != null)
        {
            health.TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
