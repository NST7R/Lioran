using UnityEngine;
using UnityEngine.UI;

public class CheckpointInteract : MonoBehaviour
{
    public GameObject interactPrompt;    // UI: "Press E to interact"
    public GameObject saveMenuPanel;     // UI panel with Yes/No buttons
    private bool playerInRange = false;

    private Transform checkpointTransform;

    private void Awake()
    {
        checkpointTransform = transform;
        interactPrompt.SetActive(false);
        saveMenuPanel.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactPrompt.SetActive(false);
            OpenSaveMenu();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            interactPrompt.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Lioran"))
        {
            interactPrompt.SetActive(false);
            saveMenuPanel.SetActive(false);
            playerInRange = false;
        }
    }

    public void OpenSaveMenu()
    {
        saveMenuPanel.SetActive(true);
    }

    public void ConfirmSave()
    {
        Debug.Log("Game saved at checkpoint: " + checkpointTransform.name);
        // Example save using PlayerPrefs (you can use a custom system instead)
        PlayerPrefs.SetFloat("SavedX", checkpointTransform.position.x);
        PlayerPrefs.SetFloat("SavedY", checkpointTransform.position.y);
        PlayerPrefs.SetString("LastCheckpoint", checkpointTransform.name);
        PlayerPrefs.Save();

        saveMenuPanel.SetActive(false);
    }

    public void CancelSave()
    {
        saveMenuPanel.SetActive(false);
    }
}
