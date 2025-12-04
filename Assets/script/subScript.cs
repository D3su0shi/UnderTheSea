using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class subScript : MonoBehaviour
{
    // movement
    private float moveSpeed = 7f;        // how fast the sub moves
    private float rotationSpeed = 90f;  // how fast the sub rotates


    [SerializeField] private Vector2 velocity;

    // oxygen stats
    private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenDepletionRate = 0.5f;

    public Image oxygenRingUI;


    // references
    private Rigidbody2D rb;
    public WeaponSystem lightningPulse;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;             // Submarine shouldn't fall
        rb.linearDamping = 0;            // No physics drag needed

        // **IMPORTANT:** Add this to prevent physics from rotating the sub on collision
        rb.freezeRotation = true;

        // Initialize oxygen
        currentOxygen = maxOxygen;
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

        if (currentOxygen <= 0)
        {
            // handle out of oxygen 
            Debug.Log("Oxygen depleted. Game Over!");
            
            // to do: trigger game over sequence
        }
    }

    // refill oxygen to max
    public void RefillOxygen()
    {
        currentOxygen = maxOxygen;
        UpdateOxygenUI();
        Debug.Log("Oxygen refilled.");
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
