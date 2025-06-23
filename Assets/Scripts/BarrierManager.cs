// --------------------------
// BarrierManager.cs (SINGLE BARRIER VERSION)
// --------------------------
using UnityEngine;
using System.Collections;

public class BarrierManager : MonoBehaviour
{
    public GameObject auraEffect;
    public GameObject runeSpawnEffect;
    public Material dissolveMaterial; // Uses _Dissolve float: 5 = visible, 0 = dissolved
    public AnimationCurve dissolveCurve;
    public float dissolveDuration = 1f;

    private Collider2D barrierCollider;
    private bool hasDissolved = false;

    void Awake()
    {
        barrierCollider = GetComponent<Collider2D>();

        if (dissolveMaterial != null)
        {
            dissolveMaterial.SetFloat("_Dissolve", 5f); // Start fully visible
        }
    }

    public void StartDissolve()
    {
        if (!hasDissolved)
        {
            hasDissolved = true;
            StartCoroutine(DissolveRoutine());
        }
    }

    IEnumerator DissolveRoutine()
    {
        float timer = 0f;

        while (timer < dissolveDuration)
        {
            timer += Time.deltaTime;
            float t = dissolveCurve.Evaluate(timer / dissolveDuration);
            float dissolveValue = Mathf.Lerp(5f, 0f, t);
            dissolveMaterial.SetFloat("_Dissolve", dissolveValue);
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        if (barrierCollider != null) barrierCollider.enabled = false;
        if (runeSpawnEffect != null) runeSpawnEffect.SetActive(false);
        if (auraEffect != null) auraEffect.SetActive(false);
    }
}