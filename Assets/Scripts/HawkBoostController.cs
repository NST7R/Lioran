using UnityEngine;
using System.Collections;
//using Cinemachine;

/// <summary>
/// Donne un boost directionnel en plein saut, façon battement d’ailes du faucon.
/// Transforme temporairement le joueur et déclenche un dash horizontal.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class HawkBoostController : MonoBehaviour
{ [Header("Références")]
    [SerializeField] private Animator animator;
    [SerializeField] private CinemachineVirtualCamera dashCam; // Caméra spéciale pour le boost
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float upwardForce = 3f;
    [SerializeField] private float dashDuration = 0.5f;

    private Rigidbody2D rb;
    private bool hasDashed = false;
    private bool isDashing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (dashCam != null)
            dashCam.Priority = 0; // désactivée par défaut
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !hasDashed && !isDashing && IsInAir())
        {
            StartCoroutine(DoGlideDash());
        }
    }

    /// <summary>
    /// Lance le dash façon faucon.
    /// </summary>
    private IEnumerator DoGlideDash()
    {
        isDashing = true;
        hasDashed = true;

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        // Active caméra cinématique
        if (dashCam != null)
            dashCam.Priority = 10;

        // Animation de transformation
        if (animator != null)
            animator.SetTrigger("ToHawk");

        // Applique le dash vers la direction du regard
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rb.velocity = new Vector2(direction * dashSpeed, upwardForce);

        yield return new WaitForSeconds(dashDuration);

        // Reviens à l’état normal
        rb.gravityScale = 1f;
        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y); // amorti

        if (animator != null)
            animator.SetTrigger("ToDeer");

        if (dashCam != null)
            dashCam.Priority = 0;

        isDashing = false;
    }

    /// <summary>
    /// Utilisé pour empêcher l’activation au sol.
    /// </summary>
    private bool IsInAir()
    {
        return Mathf.Abs(rb.velocity.y) > 0.1f;
    }

    /// <summary>
    /// Réinitialise le dash (à appeler quand on touche le sol).
    /// </summary>
    public void ResetDash()
    {
        hasDashed = false;
    }
}