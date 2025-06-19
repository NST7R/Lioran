using UnityEngine;

public class LioranMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D body;
    private Animator animation;
    private BoxCollider2D boxCollider;

    private int jumpCount = 0;
    private int maxJumps = 1;

    private Vector3 originalScale;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        originalScale = transform.localScale;
    }

    void Update()
    {   
        // Horizontal Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Switch directions using initial scale
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            Jump();
        }

        // Reset jump count when grounded
        if (onGround())
        {
            jumpCount = 0;
        }

        // Animation
        animation.SetFloat("yVelocity", (body.velocity.y));
        animation.SetBool("Walk", horizontalInput != 0);
        animation.SetBool("onGround", onGround());
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        animation.SetTrigger("Jump");
        jumpCount++;
    }

    private bool onGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
}
