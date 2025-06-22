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
    [SerializeField] private GameObject vfxPurification;   // Déjà présent dans la scène, désactivé initialement
    [SerializeField] private GameObject corruptedLeafVFX;
    [SerializeField] private GameObject purifiedLeafVFX;

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

    public string GetInteractionPrompt() => purified ? "" : "Purifier (E)";

    public void Interact(Transform vfxLaunchPoint)
    {
        if (purified || spawnSpiritVFXPrefab == null)
            return;

        StartCoroutine(PlaySpiritThenActivatePurification(vfxLaunchPoint.position));
    }

    private IEnumerator PlaySpiritThenActivatePurification(Vector3 spawnPos)
    {
        // Instanciation et lancement spawnSpiritVFX
        if (currentSpiritVFXInstance == null)
        {
            currentSpiritVFXInstance = Instantiate(spawnSpiritVFXPrefab, spawnPos, Quaternion.identity);
            var psSpirit = currentSpiritVFXInstance.GetComponent<ParticleSystem>();
            if (psSpirit != null)
                psSpirit.Play();
        }

        // Attente fixe de 1.5 secondes avant de lancer vfxPurification
        yield return new WaitForSeconds(1.5f);

        // Activation et lancement du vfxPurification
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

        // Switch visuels feuille (corrompue --> purifiée)
        if (corruptedLeafVFX != null)
            corruptedLeafVFX.SetActive(false);

        if (purifiedLeafVFX != null)
            purifiedLeafVFX.SetActive(true);

        purified = true;

        // Optionnel : tu peux attendre la fin du vfxPurification si besoin ici

        // Détruire spawnSpiritVFX à la fin de la purification
        if (currentSpiritVFXInstance != null)
        {
            Destroy(currentSpiritVFXInstance);
            currentSpiritVFXInstance = null;
        }
    }
}
