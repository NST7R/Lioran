using UnityEngine;

/// <summary>
/// Gère la logique d’une feuille corrompue que le joueur peut purifier.
/// Ce script doit être attaché au prefab de la feuille.
/// </summary>
public class CorruptedLeaf : MonoBehaviour, IInteractable
{
    [Header("VFX et éléments visuels")]
    [SerializeField] private GameObject spawnSpiritVFX;   // FX lumineux entre le joueur et la feuille
    [SerializeField] private GameObject spiritPrefab;     // L’esprit à faire apparaître
    [SerializeField] private GameObject vfxPurification;  // FX de purification (à l’endroit de la feuille)
    [SerializeField] private GameObject corruptedMesh;    // Feuille corrompue visible au départ
    [SerializeField] private GameObject purifiedMesh;     // Feuille purifiée (affichée après purification)
    [SerializeField] private Transform spiritSpawnPoint;  // Position où le spirit est instancié

    private bool purified = false; // Empêche de purifier plusieurs fois

    public string GetInteractionPrompt()
    {
        return purified ? "" : "Purifier (E)";
    }

    public void Interact(Transform player)
    {
        if (purified) return;

        // Instancie un FX de lumière qui se dirige vers la feuille
        GameObject fx = Instantiate(spawnSpiritVFX, player.position, Quaternion.identity);
        var fxScript = fx.GetComponent<SpawnSpiritVFX>();
        fxScript.Initialize(transform.position, () =>
        {
            // Une fois le FX arrivé : on instancie le spirit + purification
            Instantiate(spiritPrefab, spiritSpawnPoint.position, Quaternion.identity);

            if (vfxPurification != null)
                Instantiate(vfxPurification, transform.position, Quaternion.identity);

            // Cache la version corrompue et affiche la version purifiée
            corruptedMesh.SetActive(false);
            purifiedMesh.SetActive(true);

            purified = true;
        });
    }
}
