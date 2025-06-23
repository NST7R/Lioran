using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private int healthValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            var health = collision.GetComponent<LioranHealth>();

            // Only add health if currentHealth is less than startingHealth (max health)
            if (health.currentHealth < health.startingHealth)
            {
                health.AddHealth(healthValue);
                AudioManager.Instance?.PlaySFX(AudioManager.Instance.healthRestoreClip);
                gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("[HealthCollectible] Player health full, cannot pick up.");
            }
        }
    }
}