using UnityEngine;

public class Rune2D : MonoBehaviour
{
    public static int totalRunesCollected = 0;
    public static int totalRunesRequired = 8;

    public float attractionRadius = 5f;
    public float attractionDuration = 1f;
    public AnimationCurve attractionCurve;

    private Transform player;
    private bool isAttracting = false;
    private float attractionTimer = 0f;
    private Vector2 startPosition;
    private bool hasCollected = false;
    public ParticleSystem pickupEffect;

    public delegate void RuneCollected(int runeIndex);
    public static event RuneCollected OnRuneCollected;
    public int runeIndex;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Lioran").transform;
    }

    void Update()
    {
        if (hasCollected) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (!isAttracting && distance < attractionRadius)
        {
            startPosition = transform.position;
            attractionTimer = 0f;
            isAttracting = true;
        }

        if (isAttracting)
        {
            attractionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(attractionTimer / attractionDuration);
            float curveValue = attractionCurve.Evaluate(t);

            Vector2 targetPosition = Vector2.Lerp(startPosition, player.position, curveValue);
            transform.position = targetPosition;

            if (t >= 1f || Vector2.Distance(transform.position, player.position) < 0.1f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        hasCollected = true;

        // Optional: disable visual or play animation here
        GetComponent<Collider2D>().enabled = false;
        totalRunesCollected++;
        OnRuneCollected?.Invoke(runeIndex);

        if (pickupEffect != null)
        {
            pickupEffect.transform.parent = null; // Detach so it's not destroyed early
            pickupEffect.Play();
            Destroy(pickupEffect.gameObject, 1f);
        }

        // Optional: delay to show effects before destroying
        Destroy(gameObject, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttracting && other.CompareTag("Lioran"))
        {
            isAttracting = true;
            startPosition = transform.position;
            attractionTimer = 0f;
        }
    }


}
