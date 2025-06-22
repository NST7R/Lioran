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
using System.Collections.Generic;

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

    public Dictionary<KeyCode, string> GetAvailableActions()
    {
        var actions = new Dictionary<KeyCode, string>();

        if (!purified)
            actions.Add(KeyCode.E, "Press (E) to purify");
        else if (!collected)
            actions.Add(KeyCode.F, "Press (F) to collect");

        return actions;
    }

    public void Interact(KeyCode key)
    {
        if (key == KeyCode.E && !purified)
        {
            StartCoroutine(PlayPurificationSequence());
        }
        else if (key == KeyCode.F && purified && !collected)
        {
            CollectLeaf();
        }
        // ⚠️ Important : après chaque interaction, on rafraîchit le texte affiché
        UIManager.Instance?.RefreshCurrentPrompt(this);
    }

    
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

    // Détruire le VFX s'il existe encore
    if (currentSpiritVFXInstance != null)
    {
        Destroy(currentSpiritVFXInstance);
        currentSpiritVFXInstance = null;
    }

    // Met à jour un compteur de collection
    LeafCounterManager.Instance?.AddLeaf();

    // Désactive proprement la feuille dans la scène
    gameObject.SetActive(false);
}

}
