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

        // ✅ Play respawn sound here
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.respawnClip);

        Debug.Log("[Respawn] Player respawned.");
    }
}
