using UnityEngine;
using TMPro;

public class LeafCounterUI : MonoBehaviour
{
    public static TextMeshProUGUI leafText;

    void Awake()
    {
        leafText = GetComponent<TextMeshProUGUI>();
        UpdateLeafDisplay(LeafCounterManager.Instance.leafCount);
    }

    public static void UpdateLeafDisplay(int count)
    {
        if (leafText != null)
            leafText.text = count.ToString(); // Just show the number
    }
}
