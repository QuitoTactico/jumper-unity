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
    
    // Movement threshold - velocidad mínima para considerar que está corriendo
    public float movementThreshold = 0.1f;
    
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
        
        // Determinar si está corriendo o idle
        float speed = Mathf.Abs(rb.velocity.x);
        bool isMoving = speed > movementThreshold;
        
        // Debug para ver los valores (puedes comentarlo después)
        // Debug.Log($"Speed: {speed}, IsMoving: {isMoving}, IsGrounded: {isGrounded}, VelocityY: {rb.velocity.y}");
        
        // Intentar con diferentes nombres de parámetros comunes
        // El Animator Controller puede tener diferentes nombres
        SetFloatIfExists("Speed", speed);
        SetFloatIfExists("MotionSpeed", speed);
        
        SetBoolIfExists("Grounded", isGrounded);
        SetBoolIfExists("IsGrounded", isGrounded);
        
        SetFloatIfExists("VerticalVelocity", rb.velocity.y);
        SetFloatIfExists("Jump", rb.velocity.y);
        
        SetBoolIfExists("FreeFall", !isGrounded && rb.velocity.y < -0.1f);
    }
    
    // Llamar este método cuando el jugador salte (desde PlayerJump)
    public void TriggerJump()
    {
        if (animator != null)
        {
            SetTriggerIfExists("Jump");
        }
    }
    
    // Animation Events - llamados por las animaciones
    public void OnFootstep()
    {
        // Aquí puedes agregar sonidos de pasos si quieres
        // Debug.Log("Footstep!");
    }
    
    public void OnLand()
    {
        // Aquí puedes agregar sonidos de aterrizaje si quieres
        // Debug.Log("Landed!");
    }
    
    // Helper methods para setear parámetros solo si existen
    private void SetFloatIfExists(string paramName, float value)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Float)
            {
                animator.SetFloat(paramName, value);
                return;
            }
        }
    }
    
    private void SetBoolIfExists(string paramName, bool value)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(paramName, value);
                return;
            }
        }
    }
    
    private void SetTriggerIfExists(string paramName)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.SetTrigger(paramName);
                return;
            }
        }
    }
}
