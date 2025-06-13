using UnityEngine;

/// <summary>
/// PetroglyphFragment
/// Représente un fragment de glyphe à collecter pour débloquer une zone.
/// </summary>
public class PetroglyphFragment : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject visual;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // Ajout à un manager global (non inclus ici)
        PetroglyphManager.Instance?.CollectFragment(this);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        if (visual != null)
            visual.SetActive(false);

        Destroy(gameObject, 0.5f); // délai si son joué
    }
}
