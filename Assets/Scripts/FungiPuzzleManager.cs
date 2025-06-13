using UnityEngine;

/// <summary>
/// FungiPuzzleManager
/// Gère la séquence à suivre pour ouvrir le chemin.
/// </summary>
public class FungiPuzzleManager : MonoBehaviour
{
    [SerializeField] private int[] correctSequence;
    [SerializeField] private GameObject doorToOpen;
    private int currentIndex = 0;

    public void PressButton(int index)
    {
        if (index == correctSequence[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= correctSequence.Length)
            {
                doorToOpen.SetActive(false); // ouvrir le chemin
                Debug.Log("Puzzle résolu");
            }
        }
        else
        {
            currentIndex = 0;
            Debug.Log("Mauvaise séquence");
        }
    }
}
