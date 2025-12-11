using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class subScript : MonoBehaviour
{
    // movement
    [SerializeField] private float moveSpeed = 7f;        // how fast the sub moves
    private float rotationSpeed = 90f;  // how fast the sub rotates

    [SerializeField] private Vector2 velocity;

    // oxygen stats
    private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenDepletionRate = 0.5f;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    public Image oxygenRingUI;

    // references
    private Rigidbody2D rb;
    WeaponSystem lightningPulse;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;             // Submarine shouldn't fall
        rb.linearDamping = 0;            // No physics drag needed

        // **IMPORTANT:** Add this to prevent physics from rotating the sub on collision
        rb.freezeRotation = true;

        // Initialize oxygen
        currentOxygen = maxOxygen;
        currentHealth = maxHealth;

        lightningPulse = GetComponent<WeaponSystem>();

        UpdateOxygenUI();
    }

    void Update()
    {
        // firing weapons (spacebar)  
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (lightningPulse != null)
            {
                lightningPulse.Attack();
            }
        }
    }

    void FixedUpdate()
    {
        // oxygen depletion
        float dt = Time.deltaTime;

        if (rb != null)
        {
            velocity = rb.linearVelocity;
        }

        HandleMovement();

        UpdateOxygen(-oxygenDepletionRate * dt);
    }

    public void HandleMovement()
    {
        // movement input
        // --- Rotation (A/D or arrows) ---
        float turn = Input.GetAxis("Horizontal");

        // Calculate the new rotation amount
        float rotationAmount = -turn * rotationSpeed * Time.deltaTime;

        // Get the current rotation angle
        float newRotationAngle = rb.rotation + rotationAmount;

        // Use MoveRotation to apply rotation through the physics system
        rb.MoveRotation(newRotationAngle);

        // --- Movement (W/S or arrows) ---
        float move = Input.GetAxis("Vertical");

        if (Mathf.Abs(move) > 0.1f)
        {
            // Set the velocity in the current forward (right) direction
            rb.linearVelocity = transform.right * move * moveSpeed;
        }
        else
        {
            // Stop instantly
            rb.linearVelocity = Vector2.zero;
        }
    }

    // check if sub is stationary
    public bool isStationary()
    {
        return rb.linearVelocity.sqrMagnitude < 0.01f;
    }

    // update oxygen level
    public void UpdateOxygen(float amount)
    {
        currentOxygen += amount;
        currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

        UpdateOxygenUI();

        // This checks if Oxygen has run out
        if (currentOxygen <= 0)
        {
            // Trigger the Game Over sequence
            Die("Suffocated");
        }
    }

    // refill oxygen to max
    public void RefillOxygen()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenUI();
        Debug.Log("Oxygen refilled.");
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Hull Hit! Health: {currentHealth}/{maxHealth}");

        // Check for Hull Failure
        if (currentHealth <= 0)
        {
            // CALL THE DIE METHOD HERE INSTEAD OF DESTROY
            Die("Hull Destroyed");
        }
    }

    // --- Die Method to trigger Game Over Scene ---
    private void Die(string cause)
    {
        Debug.Log($"GAME OVER: {cause}");

        // Notify the Manager to load the Game Over Scene
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.TriggerLoss();
        }
        else
        {
            Debug.LogError("No GameStatsManager found! Cannot load Game Over scene.");
            // Fallback: Just destroy ship if manager is missing
            Destroy(gameObject);
        }
    }

    // New Helper Function to handle the UI
    private void UpdateOxygenUI()
    {
        if (oxygenRingUI != null)
        {
            // Converts current oxygen to a 0.0 to 1.0 percentage
            oxygenRingUI.fillAmount = currentOxygen / maxOxygen;

            // Optional: Change color to red if low
            if (currentOxygen < 30)
                oxygenRingUI.color = Color.red;
            else
                oxygenRingUI.color = Color.white;
        }
    }
}