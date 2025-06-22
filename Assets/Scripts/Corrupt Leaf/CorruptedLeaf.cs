/// <summary>
/// Gère la purification d’une feuille corrompue.
/// Ce script doit être attaché à un GameObject racine représentant une feuille.
/// Le joueur peut interagir avec cette feuille (via touche E ou autre).
/// Lors de la purification :
/// - Un VFX est lancé depuis le joueur
/// - Une fois le VFX terminé, la purification visuelle est jouée (à la position d’origine du prefab)
/// - Le visuel corrompu est masqué et le visuel purifié est activé immédiatement
/// </summary>
using UnityEngine;
using System.Collections;

public class CorruptedLeaf : MonoBehaviour, IInteractable
{
    [Header("Références VFX")]
    [SerializeField] private GameObject spawnSpiritVFXPrefab;
    [SerializeField] private GameObject vfxPurification;
    [SerializeField] private GameObject corruptedLeafVFX;
    [SerializeField] private GameObject purifiedLeafVFX;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnSpiritVFXSpawnPoint;

    private bool purified = false;
    private GameObject currentSpiritVFXInstance;

    private void Awake()
    {
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(true);

        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(false);

        if (vfxPurification != null)
            vfxPurification.SetActive(false);
    }

    public string GetInteractionPrompt() => purified ? "" : "Press (E) to purify";

    public void Interact(Transform player)
    {
        if (purified)
            return;

        StartCoroutine(PlayPurificationSequence());
    }

    private IEnumerator PlayPurificationSequence()
    {
        // Active vfx purification
        if (vfxPurification != null)
        {
            vfxPurification.SetActive(true);
            var psPurif = vfxPurification.GetComponent<ParticleSystem>();
            if (psPurif != null)
            {
                psPurif.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                psPurif.Play();
            }
        }

        // Switch visuels
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(false);
        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(true);

        purified = true;

        // Instancie spawnSpiritVFX (si pas déjà)
        if (spawnSpiritVFXPrefab != null && spawnSpiritVFXSpawnPoint != null)
        {
            if (currentSpiritVFXInstance == null)
            {
                currentSpiritVFXInstance = Instantiate(spawnSpiritVFXPrefab, spawnSpiritVFXSpawnPoint.position, spawnSpiritVFXSpawnPoint.rotation);
                var spawnSpiritScript = currentSpiritVFXInstance.GetComponent<SpawnSpiritVFX>();
                if (spawnSpiritScript != null)
                    spawnSpiritScript.Initialize(Vector3.zero, null);
            }
        }

        yield return new WaitForSeconds(1.5f);
    }
}
