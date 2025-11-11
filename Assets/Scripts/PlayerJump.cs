using UnityEngine; 
using UnityEngine.InputSystem;
 
public class PlayerJump : MonoBehaviour 
{ 
    public Rigidbody2D rb; 
    public float jumpForce = 10f; 
 
    // Ground Check variables 
    public Transform groundCheck; 
    public LayerMask groundLayer; 
    private bool isGrounded;
    public float groundCheckRadius = 0.2f; 
    
    
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    } 
 
    // siempre usar inputActions.Player.Jump.WasPressedThisFrame()
    void Update() 
    { 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); 

        if (inputActions.Player.Jump.WasPressedThisFrame() && isGrounded) 
        { 
            rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
        } 
 
        // --- Variable Jump Height --- 
        if (inputActions.Player.Jump.WasReleasedThisFrame() && rb.velocity.y > 0f) 
        { 
            // If the button is released while jumping, cut the upward velocity 
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); 
        } 
    }
 
    // Helper function to visualize the ground check radius in the Scene view 
    private void OnDrawGizmosSelected() 
    { 
        if (groundCheck == null) return; 
        Gizmos.color = Color.yellow; 
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius); 
    } 
}