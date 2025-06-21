using UnityEngine;

/// <summary>
/// Déclencheur attaché à une zone : lance la chute des objets du groupe.
/// </summary>
public class FallingCeilingTrigger : MonoBehaviour
{
    [SerializeField] private FallingCeilingGroup parentGroup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            parentGroup.TriggerFall(collision.transform);
        }
    }
}
