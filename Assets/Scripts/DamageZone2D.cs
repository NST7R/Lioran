/// <summary>
/// DamageZone2D
/// Zone qui inflige des dégâts continus (ex : eau stagnante, acide).
/// </summary>

using UnityEngine;
using System.Collections;

public class DamageZone2D : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private float tickRate = 1.5f;

    private Coroutine damageCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            damageCoroutine = StartCoroutine(DamageOverTime(collision.GetComponent<LioranHealth>()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran") && damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
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