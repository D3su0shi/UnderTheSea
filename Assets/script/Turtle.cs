using UnityEngine;

public class Turtle : MonoBehaviour
{
    [SerializeField] private float rescueZoneRadius = 3f; // Distance ship must be within
    [SerializeField] private float rescueTimeDuration = 5f; // Time required to rescue
    [SerializeField] private bool isBeingRescued = false; // Debug flag to see status in Inspector

    public subScript ship; // DRAG YOUR SUBMARINE HERE
    private string timerID; // Unique ID for this turtle's timer

    void Start()
    {
        // specific ID so if you have multiple turtles, their timers don't conflict
        timerID = "TurtleRescue_" + gameObject.GetInstanceID();
    }

    void Update()
    {
        if (ship == null) return;

        // 1. Calculate Distance
        float distanceToShip = Vector2.Distance(transform.position, ship.transform.position);

        // 2. Check Conditions: Inside radius AND Ship is stationary
        bool canRescue = (distanceToShip <= rescueZoneRadius && ship.isStationary());

        if (canRescue)
        {
            // If we are in range/stopped, and not yet rescuing, START the timer
            if (!isBeingRescued)
            {
                StartRescue();
            }
        }
        else
        {
            // If we move out of range or start moving, STOP the timer
            if (isBeingRescued)
            {
                CancelRescue();
            }
        }
    }

    private void StartRescue()
    {
        isBeingRescued = true;

        // Calls your existing TimerSystem
        TimerSystem.Instance.AddTimer(timerID, rescueTimeDuration, CompleteRescue);

        Debug.Log("Turtle rescue started...");
    }

    private void CancelRescue()
    {
        isBeingRescued = false;

        // Cancels the specific timer for this turtle
        TimerSystem.Instance.CancelTimer(timerID);

        Debug.Log("Rescue cancelled (Ship moved or left zone).");
    }

    public void CompleteRescue()
    {
        isBeingRescued = false;

        // Cleanup the timer just in case
        TimerSystem.Instance.CancelTimer(timerID);

        Debug.Log("Turtle rescue complete!");

        // TODO: Add points here if you have a GameManager
        // GameManager.Instance.AddScore(100);

        // Remove the turtle from the game
        Destroy(gameObject);
    }

    // OPTIONAL: Draws a yellow circle in the Scene view so you can see the radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rescueZoneRadius);
    }
}