using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointSaver : MonoBehaviour
{
    private string checkpointID;
    private string sceneName;
    private string lastCheckpointKey;

    private void Awake()
    {
        sceneName = SceneManager.GetActiveScene().name;

        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        checkpointID = $"{sceneName}_CP_{Mathf.Round(pos.x * 100f) / 100f}_{Mathf.Round(pos.y * 100f) / 100f}";

        lastCheckpointKey = sceneName + "_LastCheckpointID";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Lioran")) return;

        string lastCheckpoint = PlayerPrefs.GetString(lastCheckpointKey, "");

        if (lastCheckpoint != checkpointID)
        {
            // ✅ New checkpoint reached

            // Save new position
            PlayerPrefs.SetFloat(sceneName + "_SavedX", transform.position.x);
            PlayerPrefs.SetFloat(sceneName + "_SavedY", transform.position.y);
            PlayerPrefs.SetString(lastCheckpointKey, checkpointID);
            PlayerPrefs.Save();

            // Play checkpoint sound
            AudioManager.Instance?.PlaySFX(AudioManager.Instance.checkpointClip);
            Debug.Log($"✅ New checkpoint activated: {checkpointID} in scene {sceneName}");

            // ✅ Only update UI on new checkpoint
            var playerHealth = collision.GetComponent<LioranHealth>();
            if (playerHealth != null)
            {
                playerHealth.LoadHealth();
                playerHealth.UpdateHeartsUI();
            }
        }
        else
        {
            Debug.Log($"Checkpoint {checkpointID} already active, skipping sound & health UI.");
        }
    }
}
