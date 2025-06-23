using UnityEngine;
using UnityEngine.SceneManagement;

public class LioranRespawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;
    private LioranHealth lioranHealth;

    private void Awake()
    {
        lioranHealth = GetComponent<LioranHealth>();
        LoadPositionAndHealth();
    }

    private void LoadPositionAndHealth()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (PlayerPrefs.HasKey(currentScene + "_SavedX") && PlayerPrefs.HasKey(currentScene + "_SavedY"))
        {
            float x = PlayerPrefs.GetFloat(currentScene + "_SavedX");
            float y = PlayerPrefs.GetFloat(currentScene + "_SavedY");
            Vector3 savedPos = new Vector3(x, y, transform.position.z);
            transform.position = savedPos;
            Debug.Log($"[Respawn] Loaded checkpoint position {savedPos} for scene '{currentScene}'.");
        }
        else
        {
            Debug.Log("[Respawn] No saved checkpoint position for current scene, spawning at default.");
            SpawnAtDefault();
        }

        // ✅ Force health load and UI update after repositioning
        if (lioranHealth != null)
        {
            lioranHealth.LoadHealth();
            lioranHealth.UpdateHeartsUI();
            Debug.Log("[Respawn] Forced UI sync after scene load.");
        }
    }

    private void SpawnAtDefault()
    {
        if (defaultSpawnPoint != null)
            transform.position = defaultSpawnPoint.position;
        else
            Debug.LogWarning("[Respawn] Default spawn point not set.");
    }

    public void Respawn()
    {
        if (lioranHealth != null)
        {
            lioranHealth.Respawn();
        }
        LoadPositionAndHealth();
        Debug.Log("[Respawn] Player respawned.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            Vector3 checkpointPos = collision.transform.position;
            string currentScene = SceneManager.GetActiveScene().name;

            PlayerPrefs.SetFloat(currentScene + "_SavedX", checkpointPos.x);
            PlayerPrefs.SetFloat(currentScene + "_SavedY", checkpointPos.y);
            PlayerPrefs.Save();

            AudioManager.Instance?.PlaySFX(AudioManager.Instance.checkpointClip);

            Debug.Log($"[Respawn] Checkpoint saved at {checkpointPos} in scene '{currentScene}'.");

            // Optional: disable collider to prevent multiple triggers
            // collision.GetComponent<Collider2D>().enabled = false;
        }
    }
}
