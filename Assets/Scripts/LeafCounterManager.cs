using UnityEngine;

public class LeafCounterManager : MonoBehaviour
{
    public static LeafCounterManager Instance;
    public int leafCount = 0;

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

    public void AddLeaf()
    {
        leafCount++;
        SaveLeafCount();
        LeafCounterUI.UpdateLeafDisplay(leafCount);
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
    }
}
