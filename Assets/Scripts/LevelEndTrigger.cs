using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lioran"))
        {
            CutsceneManager.Instance?.StartCutscene();
            gameObject.SetActive(false); // Prevent retriggering
        }
    }
}
