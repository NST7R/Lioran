using UnityEngine;
using System.Collections;
public class RuneManager : MonoBehaviour
{
    public Transform[] runeSlots;         
    public GameObject[] runePrefabs;      // Prefabs for each rune
    public GameObject chargingVFXPrefab;
    private bool[] activatedRunes = new bool[8]; // Track which runes have appeared

    public Camera playerCamera;
    public Camera runeCamera;
    public float cameraShowTime = 2f;

    void OnEnable()
    {
        Rune2D.OnRuneCollected += AddRune;
    }

    void OnDisable()
    {
        Rune2D.OnRuneCollected -= AddRune;
    }

    public void AddRune(int index)
    {
        Debug.Log("Adding rune at index: " + index);
        if (index < 0 || index >= activatedRunes.Length) return;
        if (activatedRunes[index]) return;

        activatedRunes[index] = true;
        StartCoroutine(ShowRuneWithEffect(index));
    }
    private IEnumerator ShowRuneWithEffect(int index)
    {
        
        Transform slot = runeSlots[index];

        // --- Switch Cameras ---
        playerCamera.enabled = false;
        runeCamera.enabled = true;

        // Spawn the charging VFX at the slot
        GameObject vfx = Instantiate(chargingVFXPrefab, slot.position + new Vector3(0, 0, -5), Quaternion.identity);
        vfx.transform.SetParent(slot);

        // Wait before showing the rune
        yield return new WaitForSeconds(1.58f); // Adjust duration as needed
        // Spawn the actual rune
        GameObject rune = Instantiate(runePrefabs[index], slot);
        rune.transform.localPosition = Vector3.zero;
        rune.transform.localScale *= 0.5f;
        yield return new WaitForSeconds(cameraShowTime); // Adjust duration as needed
        Destroy(vfx, 3f);
        // --- Switch Back ---
        runeCamera.enabled = false;
        playerCamera.enabled = true;
    }

}
