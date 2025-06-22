using UnityEngine;
using UnityEngine.SceneManagement;

public class LioranRespawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;
    private Transform currentCheckpoint;
    private LioranHealth lioranHealth;

    private bool hasLoadedPosition = false;

    private void Awake()
    {
        lioranHealth = GetComponent<LioranHealth>();

        string scene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey(scene + "_SavedX") && PlayerPrefs.HasKey(scene + "_SavedY"))
        {
            float x = PlayerPrefs.GetFloat(scene + "_SavedX");
            float y = PlayerPrefs.GetFloat(scene + "_SavedY");
            transform.position = new Vector3(x, y, transform.position.z);
            Debug.Log($"[Respawn] Loaded checkpoint position {transform.position} for scene '{scene}'");
            hasLoadedPosition = true;
        }
        else if (defaultSpawnPoint != null)
        {
            transform.position = defaultSpawnPoint.position;
            Debug.Log($"[Respawn] No saved checkpoint for scene '{scene}', using default spawn point {transform.position}.");
            hasLoadedPosition = true;
        }
        else
        {
            Debug.LogWarning("[Respawn] No checkpoint or default spawn point set!");
        }
    }

    public void Respawn()
    {
        lioranHealth.Respawn();

        if (!hasLoadedPosition)
        {
            if (currentCheckpoint != null)
            {
                transform.position = currentCheckpoint.position;
                Debug.Log($"[Respawn] Respawning at current checkpoint {transform.position}");
            }
            else if (defaultSpawnPoint != null)
            {
                transform.position = defaultSpawnPoint.position;
                Debug.Log($"[Respawn] Respawning at default spawn point {transform.position}");
            }
        }
        else
        {
            Debug.Log("[Respawn] Position already loaded at Awake, not overriding.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            currentCheckpoint = collision.transform;

            string scene = SceneManager.GetActiveScene().name;

            PlayerPrefs.SetFloat(scene + "_SavedX", currentCheckpoint.position.x);
            PlayerPrefs.SetFloat(scene + "_SavedY", currentCheckpoint.position.y);
            PlayerPrefs.Save();

            Debug.Log($"[Respawn] Checkpoint saved at {currentCheckpoint.position} in scene '{scene}'");

            // Comment this out for testing multiple saves at same checkpoint
            // collision.GetComponent<Collider2D>().enabled = false;
        }
    }
}
