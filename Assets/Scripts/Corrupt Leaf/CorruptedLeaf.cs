/// <summary>
/// Gère la purification d’une feuille corrompue.
/// Ce script doit être attaché à un GameObject racine représentant une feuille.
/// Le joueur peut interagir avec cette feuille (via touche E ou autre).
/// Lors de la purification :
/// - Un VFX 
/// - Une fois le VFX terminé, la purification visuelle est jouée (à la position d’origine du prefab)
/// - Le visuel corrompu est masqué et le visuel purifié est activé immédiatement
/// - Une fois purifiée, elle peut être collectée via une autre touche.
/// </summary>
using UnityEngine;
using System.Collections;

public class CorruptedLeaf : MonoBehaviour, IInteractable
{
    [Header("Références VFX")]
    [SerializeField] private GameObject spawnSpiritVFX;
    [SerializeField] private GameObject vfxPurification;
    [SerializeField] private GameObject corruptedLeafVFX;
    [SerializeField] private GameObject purifiedLeafVFX;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnSpiritVFXSpawnPoint;

    private bool purified = false;
    private bool collected = false;

     private GameObject currentSpiritVFXInstance;

    private void Awake()
    {
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(true);

        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(false);
    }

    public string GetInteractionPrompt()
    {
        if (!purified) return "Purifier (E)";
        if (!collected) return "Récolter (F)";
        return "";
    }

    public void Interact()
    {
        if (!purified)
        {
            Debug.Log("Start purification");
            StartCoroutine(PlayPurificationSequence());
        }
        else if (!collected)
        {
            Debug.Log("Collecting leaf");
            CollectLeaf();
        }
    }

    // private void LaunchPurification()
    // {
    //     if (spawnSpiritVFX == null) return;

    //     GameObject fx = Instantiate(spawnSpiritVFX, spawnSpiritVFXSpawnPoint.position, Quaternion.identity);

    //     var fxScript = fx.GetComponent<SpawnSpiritVFX>();
    //     fxScript.Initialize(transform.position,() =>
    //     {
    //         if (vfxPurification != null)
    //             Instantiate(vfxPurification, transform.position, Quaternion.identity);

    //         if (corruptedLeafVFX != null)
    //             corruptedLeafVFX.SetActive(false);

    //         if (purifiedLeafVFX != null)
    //             purifiedLeafVFX.SetActive(true);

    //         purified = true;
    //     });
    // }
    private IEnumerator PlayPurificationSequence()
    {Debug.Log("PlayPurificationSequence started");

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

    if (corruptedLeafVFX != null)
        corruptedLeafVFX.SetActive(false);
    if (purifiedLeafVFX != null)
        purifiedLeafVFX.SetActive(true);

    purified = true;

    if (spawnSpiritVFX != null && spawnSpiritVFXSpawnPoint != null)
    {
        if (currentSpiritVFXInstance == null)
        {
            currentSpiritVFXInstance = Instantiate(spawnSpiritVFX, spawnSpiritVFXSpawnPoint.position, spawnSpiritVFXSpawnPoint.rotation);
            var spawnSpiritScript = currentSpiritVFXInstance.GetComponent<SpawnSpiritVFX>();
            if (spawnSpiritScript != null)
                spawnSpiritScript.Initialize(Vector3.zero, null);
        }
    }

    yield return new WaitForSeconds(1.5f);
    Debug.Log("PlayPurificationSequence ended");
    }

    private void CollectLeaf()
    {
        collected = true;

        // Met à jour un compteur de collection
        LeafCounterManager.Instance?.AddLeaf();

        // Désactive proprement la feuille dans la scène
        gameObject.SetActive(false);
    }
}
