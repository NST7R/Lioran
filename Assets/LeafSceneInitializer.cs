using UnityEngine;
using UnityEngine.Rendering;

public class LeafSceneInitializer : MonoBehaviour
{
    [Header("Scene-specific references")]
    public Volume witheredVolume;
    public Volume colorfulVolume;
    public Transform shockwaveSpawnPoint;
    public GameObject leafRestorationObject;

    void Start()
    {
        if (LeafCounterManager.Instance != null)
        {
            LeafCounterManager.Instance.InitializeSceneReferences(
                witheredVolume,
                colorfulVolume,
                shockwaveSpawnPoint,
                leafRestorationObject
            );
        }
        else
        {
            Debug.LogWarning("LeafCounterManager instance not found in scene!");
        }
    }
}
