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
    [SerializeField] private GameObject spawnSpiritVFX;
    [SerializeField] private GameObject vfxPurification;
    [SerializeField] private GameObject corruptedLeafVFX;
    [SerializeField] private GameObject purifiedLeafVFX;

  

    private bool purified = false;

    private void Awake()
    {
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(true);
        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(false);
    }

    public string GetInteractionPrompt() => purified ? "" : "Purifier (E)";

   public void Interact(Transform player)
    {
        if (purified || spawnSpiritVFX == null)
            return;

        // Calcule dynamique du point d’impact sous la feuille (ex: raycast au sol)
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        Vector3 impactPoint = player.position; // par défaut : 2 unités sous la feuille

        // Raycast pour détecter un vrai sol si nécessaire
        if (Physics.Raycast(origin, direction, out RaycastHit hit, 5f))
        {
            impactPoint = transform.position;
        }

        // Instancie le VFX de transition (orbe + spirit)
        GameObject fx = Instantiate(spawnSpiritVFX, player.position, spawnSpiritVFX.transform.rotation);

        // Initialise le FX avec la position d’impact calculée
        SpawnSpiritVFX fxScript = fx.GetComponent<SpawnSpiritVFX>();
        fxScript.Initialize(impactPoint, () =>
        {
            Debug.Log("in");
            if (vfxPurification != null)
                Instantiate(vfxPurification, impactPoint, Quaternion.identity);
                 Debug.Log("purification");
            if (corruptedLeafVFX != null)
                corruptedLeafVFX.SetActive(false);
                 Debug.Log("deactiv corruption");
            if (purifiedLeafVFX != null)
                purifiedLeafVFX.SetActive(true);
                 Debug.Log("purified");
            purified = true;
        });
    }

}
