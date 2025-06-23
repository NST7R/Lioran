using UnityEngine;

public class CheckpointSaver : MonoBehaviour
{
    private bool checkpointUsed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Lioran") || checkpointUsed) return;

        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        Vector3 pos = transform.position;
        string checkpointKey = scene + "_Checkpoint_" + pos.x + "_" + pos.y;

        if (PlayerPrefs.HasKey(checkpointKey))
        {
            checkpointUsed = true;
            GetComponent<Collider2D>().enabled = false;
            return; // Already saved, don't repeat
        }

        PlayerPrefs.SetFloat(scene + "_SavedX", pos.x);
        PlayerPrefs.SetFloat(scene + "_SavedY", pos.y);
        PlayerPrefs.SetString("SavedScene", scene);
        PlayerPrefs.SetInt(checkpointKey, 1);
        PlayerPrefs.Save();

        AudioManager.Instance?.PlaySFX(AudioManager.Instance.checkpointClip);

        checkpointUsed = true;
        GetComponent<Collider2D>().enabled = false; // Disable so no retrigger

        Debug.Log($"Checkpoint saved at {pos} in scene: {scene}");
    }
}
