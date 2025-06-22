using UnityEngine;
using System;

/// <summary>
/// Gère le VFX complet : orb lumineuse lancée depuis le joueur + apparition de l’esprit.
/// Ce FX joue de manière autonome (animation intégrée au prefab). Une fois terminé, il exécute un callback.
/// Ce script doit être attaché au prefab du VFX.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class SpawnSpiritVFX : MonoBehaviour
{
    private Action onImpactCallback;
    private ParticleSystem particleSystemInstance;

    private void Awake()
    {
        // Récupère la référence au ParticleSystem au lancement
        particleSystemInstance = GetComponent<ParticleSystem>();
        if (particleSystemInstance == null)
        {
            Debug.LogWarning("SpawnSpiritVFX : Aucun ParticleSystem trouvé sur le prefab !");
        }
    }

    /// <summary>
    /// Initialise le VFX : position cible (inutile ici mais gardé pour compatibilité future), et callback à exécuter à la fin.
    /// </summary>
    /// <param name="target">Position visée (non utilisée ici, déplacement visuel géré par le VFX lui-même)</param>
    /// <param name="onImpact">Callback appelé à la fin de l’effet</param>
    public void Initialize(Vector3 target, Action onImpact)
    {
        onImpactCallback = onImpact;

        if (particleSystemInstance != null)
        {
            particleSystemInstance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particleSystemInstance.Play();

            // Lance la coroutine pour attendre la fin de l’effet
            StartCoroutine(WaitForVFXToEnd());
        }
        else
        {
            Debug.LogWarning("SpawnSpiritVFX : Aucun système de particules actif !");
            onImpactCallback?.Invoke();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Attend que le système de particules ait fini de jouer, puis exécute la suite.
    /// </summary>
    private System.Collections.IEnumerator WaitForVFXToEnd()
    {
        // Attend jusqu’à ce que les particules soient terminées
        yield return new WaitUntil(() => !particleSystemInstance.IsAlive(true));

        // Callback de purification
        onImpactCallback?.Invoke();

        // Destruction du VFX
        Destroy(gameObject);
    }
}
