using UnityEngine;
using System;
using System.Collections;

public class SpawnSpiritVFX : MonoBehaviour
{
    private Action onImpact;

    public void Initialize(Vector3 target, Action onImpactCallback)
    {
        onImpact = onImpactCallback;
        // Ici tu peux lancer la coroutine pour simuler la fin du VFX
        StartCoroutine(WaitAndImpact());
    }

    private IEnumerator WaitAndImpact()
    {
        // Attends 1 seconde (durée du VFX) avant d’appeler la callback
        yield return new WaitForSeconds(1f);

        onImpact?.Invoke();
        Destroy(gameObject);
    }
}
