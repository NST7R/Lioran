using UnityEngine;
using UnityEngine.SceneManagement;

public class LioranRespawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;
    private LioranHealth lioranHealth;

    private void Awake()
    {
        lioranHealth = GetComponent<LioranHealth>();

        LoadCheckpointPosition();
    }

    public void Respawn()
    {
        LoadCheckpointPosition();

        // Restore health and reset state
        lioranHealth.ResetSavedHealth();
        lioranHealth.Respawn();
    }

    private void LoadCheckpointPosition()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey(scene + "_SavedX") && PlayerPrefs.HasKey(scene + "_SavedY"))
        {
            float x = PlayerPrefs.GetFloat(scene + "_SavedX");
            float y = PlayerPrefs.GetFloat(scene + "_SavedY");
            transform.position = new Vector3(x, y, transform.position.z);

            Debug.Log($"[Respawn] Loaded checkpoint position {transform.position} for scene '{scene}'");
        }
        else if (defaultSpawnPoint != null)
        {
            transform.position = defaultSpawnPoint.position;
            Debug.Log($"[Respawn] No saved checkpoint for scene '{scene}', using default spawn point {transform.position}.");
        }
        else
        {
            Debug.LogWarning("[Respawn] No checkpoint or default spawn point set!");
        }
    }
}
