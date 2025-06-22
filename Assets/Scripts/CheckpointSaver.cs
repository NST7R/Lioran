using UnityEngine;

public class CheckpointSaver : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            Vector3 pos = transform.position;

            PlayerPrefs.SetFloat("SavedX", pos.x);
            PlayerPrefs.SetFloat("SavedY", pos.y);
            PlayerPrefs.SetString("SavedScene", UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            PlayerPrefs.Save();

            Debug.Log("Checkpoint saved at " + pos + " in scene: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

            GetComponent<Collider2D>().enabled = false;
        }
    }
}
