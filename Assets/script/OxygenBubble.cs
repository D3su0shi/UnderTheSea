using UnityEngine;

public class OxygenBubble : MonoBehaviour
{
    [SerializeField] private float dockingZoneRadius = 3f; // Visual radius of the zone
    [SerializeField] private float refillTimeDuration = 3f; // Time to wait before refilling

    // Internal state
    private bool isRefilling = false;
    private string timerID;

    private subScript ship;

    void Start()
    {
        // Generate a unique ID for this specific bubble so timers don't clash
        timerID = "OxygenBubbleRefill_" + gameObject.GetInstanceID();

        // AUTOMATICALLY find the ship. No manual dragging needed!
        ship = FindFirstObjectByType<subScript>();

        if (ship == null)
        {
            Debug.LogError("OxygenBubble: Could not find the 'subScript' in the scene! Make sure the Submarine is active.");
        }
    }

    void Update()
    {
        if (ship == null) return;

        // Calculate distance
        float distanceToShip = Vector2.Distance(transform.position, ship.transform.position);

        // Check conditions: Inside radius AND Ship is not moving
        bool canRefill = (distanceToShip <= dockingZoneRadius && ship.isStationary());

        if (canRefill)
        {
            if (!isRefilling)
            {
                StartRefill();
            }
        }
        else
        {
            // If the ship moves or leaves the circle, cancel immediately
            if (isRefilling)
            {
                CancelRefill();
            }
        }
    }

    private void StartRefill()
    {
        isRefilling = true;

        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.AddTimer(timerID, refillTimeDuration, CompleteRefill);
            Debug.Log("Entered Oxygen Bubble. Refilling in " + refillTimeDuration + " seconds...");
        }
    }

    private void CancelRefill()
    {
        isRefilling = false;

        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.CancelTimer(timerID);
        }

        Debug.Log("Left Oxygen Bubble / Moved. Refill Cancelled.");
    }

    private void CompleteRefill()
    {
        isRefilling = false;
        if (ship != null)
        {
            ship.RefillOxygen();
            Debug.Log("Oxygen Refilled!");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f); // Semi-transparent green
        Gizmos.DrawWireSphere(transform.position, dockingZoneRadius);
    }
}