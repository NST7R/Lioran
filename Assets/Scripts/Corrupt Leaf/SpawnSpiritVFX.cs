using UnityEngine;
using System;

/// <summary>
/// Gère un effet lumineux (ex: sphère de lumière) qui part du joueur et va vers la feuille.
/// Une fois arrivé, il appelle une fonction de callback.
/// </summary>
public class SpawnSpiritVFX : MonoBehaviour
{
    private Vector3 targetPosition; // Destination finale
    private Action onImpact;        // Action à exécuter quand le FX atteint la cible
    private float speed = 5f;       // Vitesse de déplacement du FX

    /// <summary>
    /// Initialisation externe depuis CorruptedLeaf (destination + callback)
    /// </summary>
    public void Initialize(Vector3 target, Action onImpactCallback)
    {
        targetPosition = target;
        onImpact = onImpactCallback;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Déplace le FX vers la position cible
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            // Une fois arrivé : exécute la logique puis détruit le FX
            onImpact?.Invoke();
            Destroy(gameObject);
        }
    }
}
