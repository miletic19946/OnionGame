using UnityEngine;

// Controls player movement, jumping, and orientation based on input
public class MoveController : MonoBehaviour
{
    // Component references
    private Rigidbody2D rb;
    private Animator anim;

    // Movement parameters
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;



    [Header("Collision check")]
    [SerializeField] private Transform groundCheck;    // Position to check if player is grounded
    [SerializeField] private float groundCheckRadius;  // Radius of ground detection circle
    [SerializeField] private LayerMask whatIsGround;   // Layers that count as ground
    private bool isGrounded;                           // Tracks if player is touching the ground


    private bool facingRight = true;                   // Tracks which direction player is facing

    private void Start()
    {
        // Get required components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Handle all gameplay logic each frame
        AnimationControllers();
        CollisionChecks();
        FlipController();

        xInput = Input.GetAxisRaw("Horizontal");       // Get player's horizontal input (-1, 0, or 1)


        Movement();                                    // Apply movement based on input



        if (Input.GetKeyDown(KeyCode.Space))           // Check for jump input
            Jump();


    }

    private void AnimationControllers()
    {
        // Update animator parameters based on current state
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
    }


    private void Jump()
    {
        // Apply upward force if player is on the ground
        if (isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Movement()
    {
        // Apply horizontal movement while preserving vertical velocity
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }

    private void FlipController()
    {
        // Get mouse position in world coordinates
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Flip character based on mouse position relative to player
        if (mousePos.x < transform.position.x && facingRight)
            Flip();
        else if (mousePos.x > transform.position.x && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingRight = !facingRight; // works as a switcher
        transform.Rotate(0, 180, 0);  // Rotate character model 180 degrees
    }


    private void CollisionChecks()
    {
        // Check if player is in contact with ground using a circle cast
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    private void OnDrawGizmos()
    {
        // Visualize ground check radius in the editor
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
