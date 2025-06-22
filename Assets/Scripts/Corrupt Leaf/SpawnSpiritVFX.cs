/// <summary>
/// Gère le VFX complet : orb lumineuse lancée depuis le joueur + apparition de l’esprit.
/// Ce FX joue tout seul (pas de déplacement manuel). Une fois terminé, il exécute un callback.
/// </summary>
/// 
using UnityEngine;

public class SpawnSpiritVFX : MonoBehaviour
{
    public void Initialize(Vector3 target, System.Action onImpactCallback)
    {
        // On peut utiliser target ou callback plus tard, ici inutilisés
        PlayEffect();
    }

    private void PlayEffect()
    {
        var ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
        }
        else
        {
            Debug.LogWarning("SpawnSpiritVFX : Pas de ParticleSystem trouvé !");
        }
    }
}
