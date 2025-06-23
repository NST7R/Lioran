using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSaver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            string scene = SceneManager.GetActiveScene().name;
            float savedX = PlayerPrefs.GetFloat(scene + "_SavedX", float.MinValue);
            float savedY = PlayerPrefs.GetFloat(scene + "_SavedY", float.MinValue);

            if (savedX != transform.position.x || savedY != transform.position.y)
            {
                PlayerPrefs.SetFloat(scene + "_SavedX", transform.position.x);
                PlayerPrefs.SetFloat(scene + "_SavedY", transform.position.y);
                PlayerPrefs.Save();

                AudioManager.Instance?.PlaySFX(AudioManager.Instance.checkpointClip);

                Debug.Log($"Checkpoint saved at {transform.position} in scene: {scene}");

                // Force player health UI update
                var playerHealth = collision.GetComponent<LioranHealth>();
                if (playerHealth != null)
                {
                    playerHealth.LoadHealth();
                    playerHealth.UpdateHeartsUI();
                    Debug.Log("Player health UI updated immediately after checkpoint set.");
                }
            }
            else
            {
                Debug.Log("Checkpoint already active, no sound played.");
            }
        }
    }
}
