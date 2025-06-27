using UnityEngine;
using UnityEngine.Rendering;
using System;

public class LeafCounterManager : MonoBehaviour
{
    public static LeafCounterManager Instance;
    public int leafCount = 0;

    public Volume witheredVolume;
    public Volume colorfulVolume;
    public float transitionSpeed = 2f;
    public GameObject shockwaveEffectPrefab;
    public Transform shockwaveSpawnPoint;
    public int LeafTocollect;
    public GameObject leafRestorationObject; 

    private bool switched = false;

    public event Action OnAllLeavesCollected;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLeafCount();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        leafCount = 0;
    }
    public void AddLeaf()
    {
        leafCount++;
        Debug.Log(leafCount);
        SaveLeafCount();
        LeafCounterUI.UpdateLeafDisplay(leafCount);

        if (leafCount >= LeafTocollect && !switched)
        {
            switched = true;
            Debug.Log("All leaf pieces collected! Triggering joyful scene...");
            OnAllLeavesCollected?.Invoke();
            StartCoroutine(SwitchToJoyful());
        }
    }

    System.Collections.IEnumerator SwitchToJoyful()
    {
        // Play shockwave effect
        if (shockwaveEffectPrefab && shockwaveSpawnPoint)
            Instantiate(shockwaveEffectPrefab, shockwaveSpawnPoint.position, Quaternion.identity);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            if (witheredVolume)
                witheredVolume.weight = Mathf.Lerp(1f, 0f, t);
            if (colorfulVolume)
                colorfulVolume.weight = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
    }

    void SaveLeafCount()
    {
        PlayerPrefs.SetInt("LeafCount", leafCount);
        PlayerPrefs.Save();
    }

    void LoadLeafCount()
    {
        leafCount = PlayerPrefs.GetInt("LeafCount", 0);
    }

    public void ResetLeafCount()
    {
        leafCount = 0;
        PlayerPrefs.DeleteKey("LeafCount");
        LeafCounterUI.UpdateLeafDisplay(leafCount);
        switched = false;
        if (witheredVolume) witheredVolume.weight = 1f;
        if (colorfulVolume) colorfulVolume.weight = 0f;
        if (leafRestorationObject) leafRestorationObject.SetActive(false);
    }
}

