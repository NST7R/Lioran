using UnityEngine;
using System.Collections;
/// <summary>
/// Contrôle l’activation de la capacité de boost faucon :
/// Transition → impulsion verticale vers la plateforme → retour à la forme normale.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class HawkBoostController : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private HawkBoostTriggerDetector detector;
    [SerializeField] private Animator animator;
    [SerializeField] private float ascendSpeed = 10f;
    [SerializeField] private float stopDistance = 0.5f;

    private Rigidbody2D rb;
    private bool isBoosting = false;
    private Transform targetPlatform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && detector.HasValidTarget() && !isBoosting)
        {
            targetPlatform = detector.GetTargetPlatform();
            if (targetPlatform != null)
            {
                StartCoroutine(DoHawkAscend());
            }
        }
    }

    private IEnumerator DoHawkAscend()
    {
        isBoosting = true;

        // 1. Bloque mouvement joueur
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        // 2. Joue animation de transformation
        if (animator != null)
            animator.SetTrigger("ToHawk");

        // 3. Monte jusqu’à la plateforme
        while (Vector2.Distance(transform.position, targetPlatform.position) > stopDistance)
        {
            Vector2 direction = (targetPlatform.position - transform.position).normalized;
            rb.velocity = direction * ascendSpeed;
            yield return null;
        }

        rb.velocity = Vector2.zero;

        // 4. Animation de retour à la forme normale
        if (animator != null)
            animator.SetTrigger("ToDeer");

        yield return new WaitForSeconds(0.2f); // petit délai de transition

        rb.gravityScale = 1f;
        isBoosting = false;
    }
}
