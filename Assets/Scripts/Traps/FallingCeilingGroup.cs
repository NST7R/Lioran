using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gère la génération et la chute d'objets à partir du plafond.
/// Le joueur peut déclencher ces chutes en entrant dans une zone.
/// Les objets sont instanciés dynamiquement, alignés selon un pattern.
/// Ce système peut être déclenché plusieurs fois.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class FallingCeilingGroup : MonoBehaviour
{
    public enum FallPattern { LeftToRight, RightToLeft, CenterOut, Adaptive }

    [Header("Réglages")]
    [Tooltip("Prefab de l’objet à faire tomber (avec Rigidbody2D)")]
    [SerializeField] private GameObject fallingObjectPrefab;

    [Tooltip("Espace horizontal entre chaque objet instancié")]
    [SerializeField] private float spacing = 1.0f;

    [Tooltip("Délai entre chaque chute")]
    [SerializeField] private float fallDelay = 0.2f;

    [Tooltip("Schéma de chute (gauche à droite, etc.)")]
    [SerializeField] private FallPattern fallPattern = FallPattern.LeftToRight;

    [Tooltip("Zone de déclenchement (doit avoir une largeur définie)")]
    [SerializeField] private GameObject areaCollider;

    // Liste des objets instanciés dans la scène
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // Évite de lancer plusieurs coroutines simultanément
    private bool isFalling = false;

    /// <summary>
    /// Appelée par le trigger : commence la chute selon le pattern défini.
    /// </summary>
    public void TriggerFall(Transform player)
    {
        if (isFalling) return;

        float width = areaCollider.transform.localScale.x * transform.localScale.x;
        int objectCount = Mathf.FloorToInt(width / spacing);

        if (objectCount <= 0)
        {
            Debug.LogWarning("Zone trop petite pour générer des objets tombants.");
            return;
        }

        isFalling = true;
        StartCoroutine(SpawnAndFallCoroutine(objectCount, player));
    }

    /// <summary>
    /// Coroutine principale de génération + activation progressive.
    /// </summary>
    private IEnumerator SpawnAndFallCoroutine(int objectCount, Transform player)
    {
        spawnedObjects.Clear();

        float width = areaCollider.transform.localScale.x * transform.localScale.x;
        Vector3 leftEdge = transform.position - new Vector3(width / 2f, 0f, 0f);

        // Instanciation des objets inactifs
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 spawnPos = leftEdge + new Vector3(i * spacing, 0f, 0f);
            GameObject clone = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
            clone.SetActive(false); // Inactif jusqu'à déclenchement
            spawnedObjects.Add(clone);
        }

        yield return new WaitForEndOfFrame(); // Sécurité : attendre une frame

        // Déclenche le bon pattern
        switch (fallPattern)
        {
            case FallPattern.LeftToRight:
                yield return StartCoroutine(FallInOrder(0, 1));
                break;

            case FallPattern.RightToLeft:
                yield return StartCoroutine(FallInOrder(spawnedObjects.Count - 1, -1));
                break;

            case FallPattern.CenterOut:
                yield return StartCoroutine(FallFromCenter());
                break;

            case FallPattern.Adaptive:
                bool playerOnRight = player.position.x > transform.position.x;
                yield return StartCoroutine(playerOnRight
                    ? FallInOrder(spawnedObjects.Count - 1, -1)
                    : FallInOrder(0, 1));
                break;
        }

        yield return new WaitForSeconds(1f); // Anti-spam simple
        isFalling = false;
    }

    /// <summary>
    /// Chute linéaire dans un ordre (ex: gauche à droite).
    /// </summary>
    private IEnumerator FallInOrder(int startIndex, int step)
    {
        int count = spawnedObjects.Count;

        for (int i = 0; i < count; i++)
        {
            int index = startIndex + (i * step);
            if (index >= 0 && index < count)
            {
                ActivateObject(index);
                yield return new WaitForSeconds(fallDelay);
            }
        }
    }

    /// <summary>
    /// Chute symétrique à partir du centre.
    /// </summary>
    private IEnumerator FallFromCenter()
    {
        int center = spawnedObjects.Count / 2;
        int offset = 0;

        while (center - offset >= 0 || center + offset < spawnedObjects.Count)
        {
            if (center - offset >= 0)
            {
                ActivateObject(center - offset);
                yield return new WaitForSeconds(fallDelay);
            }

            if (offset != 0 && center + offset < spawnedObjects.Count)
            {
                ActivateObject(center + offset);
                yield return new WaitForSeconds(fallDelay);
            }

            offset++;
        }
    }
/// <summary>
/// Calcule la durée totale de chute basée sur le nombre d’objets et le délai entre chaque.
/// </summary>
public float GetFallDuration()
{
    if (areaCollider == null) return 0f;

    float width = areaCollider.transform.localScale.x * transform.localScale.x;
    int objectCount = Mathf.FloorToInt(width / spacing);

    return objectCount * fallDelay;
}


    /// <summary>
    /// Active un objet et déclenche sa chute (Rigidbody2D dynamique).
    /// </summary>
    private void ActivateObject(int index)
    {
        if (index >= 0 && index < spawnedObjects.Count)
        {
            GameObject obj = spawnedObjects[index];
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.simulated = true;
                }
            }
        }
    }
}
