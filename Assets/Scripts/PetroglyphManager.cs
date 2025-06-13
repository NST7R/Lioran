/// <summary>
/// PetroglyphManager
/// Gère le suivi des fragments récoltés.
/// </summary>

using UnityEngine;
using System.Collections.Generic;

public class PetroglyphManager : MonoBehaviour
{
    public static PetroglyphManager Instance { get; private set; }

    [SerializeField] private int totalFragments = 5;
    private List<PetroglyphFragment> collected = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public void CollectFragment(PetroglyphFragment fragment)
    {
        collected.Add(fragment);

        // Feedback UI ou condition d'activation
        if (collected.Count >= totalFragments)
        {
            Debug.Log("Tous les fragments collectés !");
            // Débloquer zone ou énigme
        }
    }
}
