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
 
    void Update() 
    { 
        // Check for ground 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        Debug.Log($"Is Grounded: {isGrounded}");
        Debug.Log($"Ground Check Position: {groundCheck.position}");
    
        // --- Basic Jump Input --- 
        if (inputActions.Player.Jump.WasPressedThisFrame())
        {
            Debug.Log("Jump button pressed!");
            
            if (isGrounded)
            {
                Debug.Log($"Jumping! Current velocity: {rb.velocity}, Jump force: {jumpForce}");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                Debug.Log($"New velocity after jump: {rb.velocity}");
            }
            else
            {
                Debug.Log("Can't jump - not grounded!");
            }
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