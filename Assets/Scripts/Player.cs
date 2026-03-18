using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    private float horizontalInput;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded = false;
    

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private int maxJumps = 2; // 2 = double jump
    private int jumpCount = 0;
    private bool jumpRequested = false;

    // Animation
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        Animate();
    }

    void FixedUpdate()
    {
        Move();
        CheckGround();
        HandleJump();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Always request a jump if space pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }
    }

    private void Move()
    {
        // Move horizontally
        rb.linearVelocity = new Vector2(horizontalInput * speed, rb.linearVelocity.y);
    }

    private void CheckGround()
    {
        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position,groundCheckRadius,groundLayer);

        // Rest jump count when on ground
        if (isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void HandleJump()
    {
        if (jumpRequested)
        {
            if(jumpCount < maxJumps)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpCount++;
            }
            jumpRequested = false;
        }
    }

    private void Animate()
    {
        // Idle and Run Animation
        animator.SetFloat("X", horizontalInput);

        //Flip sprite (Left and Right movement)
        if(horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        } else if(horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }

        //Jump and Double Jump Animation
        if(jumpCount == 1)
        {
            animator.Play("Jump Animation");
        } else if(jumpCount == 2)
        {
            animator.Play("Double Jump Animation");
        }

        animator.SetBool("isGrounded", isGrounded);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
    }
}
