using UnityEngine;

public class LioranRespawn : MonoBehaviour
{
    [SerializeField] private Transform defaultSpawnPoint;
    private Transform currentCheckpoint;
    private LioranHealth lioranHealth;

    private void Awake()
    {
        lioranHealth = GetComponent<LioranHealth>();

        if (defaultSpawnPoint == null)
            Debug.LogWarning("Default spawn point is not assigned!");
    }

    public void Respawn()
    {
        lioranHealth.Respawn();

        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position;
            Debug.Log("Respawned at checkpoint: " + currentCheckpoint.name);
        }
        else if (defaultSpawnPoint != null)
        {
            transform.position = defaultSpawnPoint.position;
            Debug.Log("Respawned at default spawn point.");
        }
        else
        {
            Debug.LogWarning("No checkpoint or default spawn point set!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            Debug.Log("Checkpoint reached: " + currentCheckpoint.name);
        }
    }
}
