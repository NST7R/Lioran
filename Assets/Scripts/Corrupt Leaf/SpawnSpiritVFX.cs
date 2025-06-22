using UnityEngine;
using System;

public class SpawnSpiritVFX : MonoBehaviour
{
    private Vector3 targetPosition;
    private Action onImpact;
    private float speed = 5f;

    public void Initialize(Vector3 target, Action onImpactCallback)
    {
        targetPosition = target;
        onImpact = onImpactCallback;
    }

    private void Update()
    {
        /* if (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            onImpact?.Invoke();
            Destroy(gameObject);
        } */
    }
}
