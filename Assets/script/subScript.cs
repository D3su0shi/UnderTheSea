using UnityEngine;

public class subScript : MonoBehaviour
{
    private float moveSpeed = 7f;        // how fast the sub moves
    private float rotationSpeed = 90f;  // how fast the sub rotates

    // NOTE: linearVelocity is obsolete; use velocity
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;             // Submarine shouldn't fall
        rb.linearDamping = 0;            // No physics drag needed

        // **IMPORTANT:** Add this to prevent physics from rotating the sub on collision
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
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
}
