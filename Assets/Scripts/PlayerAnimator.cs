using UnityEngine;

public class PlayerAnimator : MonoBehaviour 
{ 
    // References
    private Animator animator;
    private Rigidbody2D rb;
    
    // Ground Check
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;
    
    // Animation Parameters (nombres de los parámetros en el Animator Controller)
    private readonly string SPEED_PARAM = "Speed";
    private readonly string GROUNDED_PARAM = "Grounded";
    private readonly string VERTICAL_VELOCITY_PARAM = "VerticalVelocity";
    private readonly string JUMP_PARAM = "Jump";
    
    void Start() 
    { 
        // Get Animator from child (Robot Kyle)
        animator = GetComponentInChildren<Animator>();
        
        if (animator == null)
        {
            Debug.LogError("No Animator found in children! Make sure Robot Kyle has an Animator component.");
        }
        
        // Get Rigidbody2D from parent (Player)
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("No Rigidbody2D found! Make sure the Player has a Rigidbody2D component.");
        }
    } 
 
    void Update() 
    { 
        if (animator == null || rb == null) return;
        
        // --- Ground Check --- 
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = true;
        }
        
        // --- Update Animation Parameters ---
        
        // Speed (for run animation) - usa el valor absoluto de la velocidad horizontal
        float speed = Mathf.Abs(rb.velocity.x);
        animator.SetFloat(SPEED_PARAM, speed);
        
        // Grounded state
        animator.SetBool(GROUNDED_PARAM, isGrounded);
        
        // Vertical velocity (for jump/fall animations)
        animator.SetFloat(VERTICAL_VELOCITY_PARAM, rb.velocity.y);
    }
    
    // Llamar este método cuando el jugador salte (desde PlayerJump)
    public void TriggerJump()
    {
        if (animator != null)
        {
            animator.SetTrigger(JUMP_PARAM);
        }
    }
}
