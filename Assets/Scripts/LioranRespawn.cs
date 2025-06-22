using UnityEngine;
using UnityEngine.SceneManagement;

public class LioranRespawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;
    private Transform currentCheckpoint;
    private LioranHealth lioranHealth;

    private void Awake()
    {
        lioranHealth = GetComponent<LioranHealth>();

        Vector2 savedPosition = LoadCheckpointPosition();
        if (savedPosition != Vector2.zero)
        {
            transform.position = savedPosition;
        }
        else if (defaultSpawnPoint != null)
        {
            transform.position = defaultSpawnPoint.position;
        }
        else
        {
            Debug.LogWarning("No checkpoint or default spawn point found!");
        }
    }

    public void Respawn()
    {
        lioranHealth.Respawn();

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
        }
        else
        {
            Vector2 savedPosition = LoadCheckpointPosition();
            if (savedPosition != Vector2.zero)
                transform.position = savedPosition;
            else if (defaultSpawnPoint != null)
                transform.position = defaultSpawnPoint.position;
        }
    }

    private Vector2 LoadCheckpointPosition()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        string savedScene = PlayerPrefs.GetString("SavedScene", "");

        if (savedScene == currentScene &&
            PlayerPrefs.HasKey("SavedX") &&
            PlayerPrefs.HasKey("SavedY"))
        {
            float x = PlayerPrefs.GetFloat("SavedX");
            float y = PlayerPrefs.GetFloat("SavedY");
            return new Vector2(x, y);
        }

        return Vector2.zero;
    }
}
