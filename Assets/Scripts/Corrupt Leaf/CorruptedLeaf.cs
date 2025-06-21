using UnityEngine;

/// <summary>
/// Gère la purification d’une feuille corrompue.
/// Ce script doit être attaché à un GameObject racine représentant une feuille.
/// Le joueur peut interagir avec cette feuille (via touche E ou autre).
/// Lors de la purification :
/// - Un VFX est lancé depuis le joueur
/// - Une fois le VFX terminé, la purification visuelle est jouée
/// - Le visuel corrompu est masqué et le visuel purifié est activé
/// </summary>
public class CorruptedLeaf : MonoBehaviour, IInteractable
{
    [Header("Références VFX")]
    [Tooltip("VFX complet : orbe lumineuse + apparition de l’esprit (contient l’animation complète)")]
    [SerializeField] private GameObject spawnSpiritVFX;

    [Tooltip("FX de purification à jouer à l’emplacement de la feuille")]
    [SerializeField] private GameObject vfxPurification;

    [Tooltip("Mesh ou VFX visible avant purification (corrompu)")]
    [SerializeField] private GameObject corruptedLeafVFX;

    [Tooltip("Mesh ou VFX visible après purification (purifié)")]
    [SerializeField] private GameObject purifiedLeafVFX;

    private bool purified = false; // Empêche les répétitions de purification

    private void Awake()
    {
        // Active par défaut la version corrompue, désactive la purifiée
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(true);

        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(false);
    }

    /// <summary>
    /// Texte d’interaction affiché au joueur.
    /// </summary>
    public string GetInteractionPrompt()
    {
        return purified ? "" : "Purifier (E)";
    }

    /// <summary>
    /// Appelée par le système d’interaction du joueur.
    /// Lance le VFX d’orbe, puis purification après l’arrivée.
    /// </summary>
    public void Interact(Transform player)
    {
        if (purified || spawnSpiritVFX == null)
            return;

        // Instancie le VFX de transition (orbe + spirit)
        GameObject fx = Instantiate(spawnSpiritVFX, player.position, Quaternion.identity);

        // Une fois le FX terminé, on lance la purification
        SpawnSpiritVFX fxScript = fx.GetComponent<SpawnSpiritVFX>();
        fxScript.Initialize(() =>
        {
            // 1. FX de purification
            if (vfxPurification != null)
                Instantiate(vfxPurification, transform.position, Quaternion.identity);

            // 2. Transition visuelle
            if (corruptedLeafVFX != null)
                corruptedLeafVFX.SetActive(false);

            if (purifiedLeafVFX != null)
                purifiedLeafVFX.SetActive(true);

            // 3. Marque la feuille comme purifiée
            purified = true;
        });
    }
}
