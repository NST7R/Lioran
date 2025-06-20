using UnityEngine;

public class LioranMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator animation;
    private BoxCollider2D boxCollider;

    private int jumpCount = 0;
    private int maxJumps = 1;

    private Vector3 originalScale;

    private MovingPlatform currentPlatform;
    private Vector2 platformVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
    }

    void Update()
    {
        // Update platform velocity from current platform
        if (currentPlatform != null)
        {
            Rigidbody2D platformRb = currentPlatform.GetComponent<Rigidbody2D>();
            if (platformRb != null)
            {
                platformVelocity = platformRb.velocity;
            }
            else
            {
                platformVelocity = Vector2.zero;
            }
        }
        else
        {
            platformVelocity = Vector2.zero;
        }

        // Flip sprite based on input
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // Reset jump count if on ground
        if (onGround())
        {
            jumpCount = 0;
        }

        // Update animator parameters
        animation.SetFloat("yVelocity", rb.velocity.y);
        animation.SetBool("onGround", onGround());
        animation.SetBool("Walk", Mathf.Abs(horizontalInput) > 0.01f);
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // Base player horizontal velocity
        Vector2 inputVelocity = new Vector2(horizontalInput * speed, rb.velocity.y);

        // Add platform horizontal velocity if standing on a platform
        Vector2 totalVelocity = inputVelocity + new Vector2(platformVelocity.x, 0);

        rb.velocity = totalVelocity;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animation.SetTrigger("Jump");
        jumpCount++;
    }

    private bool onGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("platform"))
        {
            // Only assign platform if player is landing on top
            if (transform.position.y > collision.transform.position.y)
            {
                currentPlatform = collision.collider.GetComponent<MovingPlatform>();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("platform"))
        {
            if (currentPlatform == collision.collider.GetComponent<MovingPlatform>())
            {
                currentPlatform = null;
                platformVelocity = Vector2.zero;
            }
        }
    }
}
