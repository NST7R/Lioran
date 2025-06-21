using UnityEngine;

/// <summary>
/// Détecte les plateformes marquées au-dessus du joueur, pour activer l'UI de boost.
/// </summary>
public class HawkBoostTriggerDetector : MonoBehaviour
{
    public float detectionDistance = 5f;
    public LayerMask platformLayer;
    public GameObject contextIcon; // UI flottante à activer

    private Transform player;

    private void Start()
    {
        player = transform;
        contextIcon.SetActive(false);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.up, detectionDistance, platformLayer);

        if (hit.collider != null && hit.collider.GetComponent<PlatformTarget>())
        {
            contextIcon.SetActive(true);
        }
        else
        {
            contextIcon.SetActive(false);
        }
    }

    /// <summary>
    /// Renvoie la position de la plateforme détectée, ou null.
    /// </summary>
    public Transform GetTargetPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.position, Vector2.up, detectionDistance, platformLayer);
        if (hit.collider != null && hit.collider.GetComponent<PlatformTarget>())
            return hit.collider.transform;

        return null;
    }

    public bool HasValidTarget() => GetTargetPlatform() != null;
}
