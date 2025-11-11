using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour 
{ 
    // Movement 
    private Rigidbody2D rb; 
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    private float horizontalInput;

    // Life
    public float respawnDelay = 0.5f;
 
    // Ground Check (optional - for different air/ground movement) 
    public Transform groundCheck; 
    public LayerMask groundLayer; 
    public float groundCheckRadius = 0.2f; 
    private bool isGrounded;

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
    } 
 
    void Update() 
    { 
        // --- Ground Check (optional) --- 
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }
        else
        {
            isGrounded = true; // Always allow movement if no ground check
        }

        // --- Read Movement Input --- 
        // siempre usar inputActions.Player.Move.ReadValue<Vector2>()
        Vector2 moveInput = inputActions.Player.Move.ReadValue<Vector2>();
        horizontalInput = moveInput.x;
    } 
 
    void FixedUpdate() 
    { 
        // --- Apply Horizontal Movement --- 
        float targetSpeed = horizontalInput * moveSpeed;
        float speedDifference = targetSpeed - rb.velocity.x;
        
        // Choose acceleration or deceleration
        float accelerationRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        
        // Calculate force to apply
        float movement = speedDifference * accelerationRate;
        
        // Apply movement
        rb.velocity = new Vector2(rb.velocity.x + movement * Time.fixedDeltaTime, rb.velocity.y);
    }

    // Helper function to visualize the ground check radius in the Scene view 
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private void RestartLevel()
    {
        // Restart the level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void Die()
    {
        // Implement death logic here (e.g., play animation, sound, etc.)
        Debug.Log("Player has died.");

        // Deactivate the player
        gameObject.SetActive(false);

        // Reset velocity to prevent carrying momentum
        rb.velocity = Vector2.zero;

        // Wait and then reactivate
        Invoke("Respawn", respawnDelay);
    }

    private void Respawn()
    {
        // Move player back to spawn point
        transform.position = Vector3.zero;

        // Reactivate the player
        gameObject.SetActive(true);
        
        Debug.Log("Player respawned.");
    }

    void OnTriggerEnter2D(Collider2D other)  // Cambié Collider a Collider2D
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("OutOfBounds"))
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)  // Cambié Collision a Collision2D
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collided with Obstacle");
            
            Die();
        }
    }
}
