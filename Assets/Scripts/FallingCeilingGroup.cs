using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gère la création procédurale d’objets tombants déclenchés dynamiquement par un trigger.
/// Instancie les objets au moment du déclenchement, répartis selon la largeur du trigger.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class FallingCeilingGroup : MonoBehaviour
{
    public enum FallPattern { LeftToRight, RightToLeft, CenterOut, Adaptive }

    [Header("Réglages")]
    [SerializeField] private GameObject fallingObjectPrefab;
    [SerializeField] private float spacing = 1.0f;
    [SerializeField] private float fallDelay = 0.2f;
    [SerializeField] private FallPattern fallPattern = FallPattern.LeftToRight;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    [SerializeField] private GameObject areaCollider;
    private bool hasTriggered = false;

    private void Awake()
    {
        //areaCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Déclenche la création et la chute des objets en fonction du pattern choisi.
    /// </summary>
    public void TriggerFall(Transform player)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        SpawnObjects();

        switch (fallPattern)
        {
            case FallPattern.LeftToRight:
                StartCoroutine(FallInOrder(0, 1));
                break;
            case FallPattern.RightToLeft:
                StartCoroutine(FallInOrder(spawnedObjects.Count - 1, -1));
                break;
            case FallPattern.CenterOut:
                StartCoroutine(FallFromCenter());
                break;
            case FallPattern.Adaptive:
                bool playerOnRight = player.position.x > transform.position.x;
                StartCoroutine(playerOnRight
                    ? FallInOrder(spawnedObjects.Count - 1, -1)
                    : FallInOrder(0, 1));
                break;
        }
    }

    /// <summary>
    /// Instancie dynamiquement les objets en fonction de la largeur du collider.
    /// </summary>
    private void SpawnObjects()
    {
        spawnedObjects.Clear();

        float width = areaCollider.transform.localScale.x * transform.localScale.x;
        int objectCount = Mathf.FloorToInt(width / spacing);

        if (objectCount <= 0)
        {
            Debug.LogWarning("Largeur du trigger insuffisante pour générer des objets tombants.");
            return;
        }

        Vector3 leftEdge = transform.position - new Vector3(width / 2f, 0f, 0f);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 spawnPos = leftEdge + new Vector3(i * spacing, 0f, 0f);
            GameObject clone = Instantiate(fallingObjectPrefab, spawnPos, Quaternion.identity);
            spawnedObjects.Add(clone);
        }
    }

    private IEnumerator FallInOrder(int startIndex, int step)
    {
        for (int i = 0; i < spawnedObjects.Count; i++)
        {
            int index = startIndex + i * step;
            if (index >= 0 && index < spawnedObjects.Count)
            {
                ActivateObject(index);
                yield return new WaitForSeconds(fallDelay);
            }
        }
    }

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
    /// Active un objet et déclenche sa chute en le passant en Rigidbody2D.Dynamic.
    /// </summary>
    private void ActivateObject(int index)
    {
        if (index >= 0 && index < spawnedObjects.Count)
        {
            GameObject obj = spawnedObjects[index];
            obj.SetActive(true);
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
        }
    }
}
