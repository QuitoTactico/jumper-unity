using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour 
{ 
    // Jump & Physics 
    private Rigidbody2D rb; 
    public float jumpForce = 10f; 
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 2f; 
 
    // Ground Check 
    public Transform groundCheck; 
    public LayerMask groundLayer; 
    public float groundCheckRadius = 0.2f; 
    private bool isGrounded; 
 
    // Coyote Time 
    private float coyoteTime = 0.15f; 
    private float coyoteTimeCounter; 
 
    // Jump Buffering 
    private float jumpBufferTime = 0.15f; 
    private float jumpBufferCounter; 
 
    // Double Jump 
    public int extraJumps = 1; 
    private int extraJumpsValue;

    // Animation
    private PlayerAnimator playerAnimator;

    // Input System
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
        extraJumpsValue = extraJumps; // Set initial jumps 
        playerAnimator = GetComponent<PlayerAnimator>(); // Get animator reference
    } 
 
    void Update() 
    { 
        // --- Ground Check --- 
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); 
 
        // --- Coyote Time & Double Jump Reset --- 
        if (isGrounded) 
        { 
            coyoteTimeCounter = coyoteTime; 
            extraJumpsValue = extraJumps; // Reset double jumps 
        } 
        else 
        { 
            coyoteTimeCounter -= Time.deltaTime; 
        } 
 
        // --- Jump Buffering --- 
        // siempre usar inputActions.Player.Jump.WasPressedThisFrame()
        if (inputActions.Player.Jump.WasPressedThisFrame()) 
        { 
            jumpBufferCounter = jumpBufferTime; 
        } 
        else 
        { 
            jumpBufferCounter -= Time.deltaTime; 
        } 
 
        // --- COMBINED Jump Input Check --- 
        if (jumpBufferCounter > 0f) 
        { 
            if (coyoteTimeCounter > 0f) // Priority 1: Ground Jump (uses coyote time) 
            { 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); 
                coyoteTimeCounter = 0f; // Consume coyote time 
                jumpBufferCounter = 0f; // Consume buffer 
                
                // Trigger jump animation
                if (playerAnimator != null) playerAnimator.TriggerJump();
            } 
            else if (extraJumpsValue > 0) // Priority 2: Air Jump 
            { 
                rb.velocity = new Vector2(rb.velocity.x, jumpForce); // You could use a different jump force
                extraJumpsValue--; // Consume an air jump 
                jumpBufferCounter = 0f; // Consume buffer 
                
                // Trigger jump animation
                if (playerAnimator != null) playerAnimator.TriggerJump();
            } 
        } 
    } 
 
    void FixedUpdate() 
    { 
        // --- Better Falling Logic --- 
        if (rb.velocity.y < 0) 
        { 
            // We are falling - apply the fallMultiplier 
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime; 
        } 
        else if (rb.velocity.y > 0 && !inputActions.Player.Jump.IsPressed()) 
        { 
            // We are rising, but not holding Jump - apply the lowJumpMultiplier 
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime; 
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