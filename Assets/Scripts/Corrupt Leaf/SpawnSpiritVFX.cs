using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Gère le VFX complet : orb lumineuse lancée depuis le joueur + apparition de l’esprit.
/// Ce FX joue tout seul (pas de déplacement manuel). Une fois terminé, il exécute un callback.
/// </summary>
public class SpawnSpiritVFX : MonoBehaviour
{
    private Action onCompleteCallback;

    [Tooltip("Durée approximative du VFX avant la purification (doit correspondre à la timeline visuelle)")]
    [SerializeField] private float vfxDuration = 2.5f;

    /// <summary>
    /// Initialisé depuis CorruptedLeaf.
    /// </summary>
    public void Initialize(Action callback)
    {
        onCompleteCallback = callback;
        StartCoroutine(VFXSequence());
    }

    /// <summary>
    /// Coroutine pour attendre la fin du VFX puis déclencher le reste de la logique.
    /// </summary>
    private IEnumerator VFXSequence()
    {
        yield return new WaitForSeconds(vfxDuration);

        onCompleteCallback?.Invoke();
        Destroy(gameObject);
    }
}
