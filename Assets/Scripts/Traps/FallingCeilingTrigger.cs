using UnityEngine;

/// <summary>
/// Déclencheur de chute attaché à une zone 2D.
/// Lorsque le joueur entre ou reste dans la zone, déclenche périodiquement une chute.
/// Repose sur le groupe parent `FallingCeilingGroup` pour instancier les objets.
/// </summary>
public class FallingCeilingTrigger : MonoBehaviour
{
    [Tooltip("Référence au groupe de plafond qui génère et fait tomber les objets.")]
    [SerializeField] private FallingCeilingGroup parentGroup;

    private bool playerInside = false;      // Indique si le joueur est dans la zone
    private float nextTriggerTime = 0f;     // Prochaine autorisation de déclenchement

    /// <summary>
    /// Quand le joueur entre dans la zone, on autorise la première chute immédiatement.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInside = true;
            nextTriggerTime = Time.time; // Autorise un déclenchement immédiat
        }
    }

    /// <summary>
    /// Si le joueur quitte la zone, on stoppe la répétition.
    /// </summary>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            playerInside = false;
        }
    }

    /// <summary>
    /// Si le joueur reste dans la zone, déclenche la chute à intervalles réguliers.
    /// Le délai est basé sur la durée estimée de chute du groupe.
    /// </summary>
    private void Update()
    {
        if (!playerInside) return;

        if (Time.time >= nextTriggerTime)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Lioran");

            if (player != null)
            {
                parentGroup.TriggerFall(player.transform);

                // Calcul dynamique du prochain délai de déclenchement
                nextTriggerTime = Time.time + parentGroup.GetFallDuration();
            }
        }
    }
}
