using UnityEngine;
using System.Collections;

public class RuneManager : MonoBehaviour
{
    public Transform[] runeSlots;
    public GameObject[] runePrefabs;
    public GameObject chargingVFXPrefab;

    private bool[] activatedRunes = new bool[8];

    public Camera lioranCamera;
    public Camera runeCamera;
    public float cameraShowTime = 2f;
    public ScreenFader screenFader;

    [Header("Assign Lioran Components")]
    public Rigidbody2D lioranRigidbody;
    public Animator lioranAnimator;
    public LioranHealth lioranHealth;

    private RigidbodyConstraints2D originalConstraints;

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

        // --- Enable silent invulnerability before transition ---
        if (lioranHealth != null)
            lioranHealth.EnableSilentInvulnerability();

        // --- Freeze Rigidbody ---
        if (lioranRigidbody != null)
        {
            originalConstraints = lioranRigidbody.constraints;
            lioranRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // --- Play idle animation ---
        if (lioranAnimator != null)
        {
            lioranAnimator.Play("LioranIdle");
        }

        yield return StartCoroutine(screenFader.FadeOut());

        lioranCamera.enabled = false;
        runeCamera.enabled = true;

        yield return StartCoroutine(screenFader.FadeIn());

        GameObject vfx = Instantiate(chargingVFXPrefab, slot.position + new Vector3(0, 0, -5), Quaternion.identity);
        vfx.transform.SetParent(slot);

        yield return new WaitForSeconds(1.58f);

        GameObject rune = Instantiate(runePrefabs[index], slot);
        rune.transform.localPosition = Vector3.zero;
        rune.transform.localScale *= 0.5f;

        yield return new WaitForSeconds(cameraShowTime);

        yield return StartCoroutine(screenFader.FadeOut());

        Destroy(vfx, 3f);

        runeCamera.enabled = false;
        lioranCamera.enabled = true;

        yield return StartCoroutine(screenFader.FadeIn());

        // --- Disable silent invulnerability after transition ---
        if (lioranHealth != null)
            lioranHealth.DisableSilentInvulnerability();

        // --- Restore Rigidbody ---
        if (lioranRigidbody != null)
            lioranRigidbody.constraints = originalConstraints;
    }
}
