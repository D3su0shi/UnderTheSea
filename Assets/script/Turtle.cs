using UnityEngine;

public class Turtle : MonoBehaviour
{
    [SerializeField] private float rescueZoneRadius; //circular radius the ship must enter to rescue turtle
    [SerializeField] private float rescueTimeDuration; //time taken to rescue turtle
    [SerializeField] private int pointsForRescue = 100; //points awarded for rescuing turtle

    [SerializeField] private bool isBeingRescued = false; //flag to check if rescue is in progress

    public subScript ship; //reference to the ship
   // public GameManager gameManager; //reference to game manager

   private string timerID; //unique ID for this instance's timer

    void Start()
    {
        timerID = "TurtleRescue_" + gameObject.GetInstanceID(); // create unique timer ID for this turtle instance

    }

    void Update()
    {
        if (ship == null) return; // if ship reference is missing, exit

        float distanceToShip = Vector2.Distance(transform.position, ship.transform.position); // calculate distance to ship

        bool canRescue = (distanceToShip <= rescueZoneRadius && ship.isStationary()); // check if ship is in rescue zone and stationary

        if (canRescue)
        {
            if(!isBeingRescued) // start rescue if not already in progress
            {
                StartRescue();
            }

            // to do: update UI
        }
        else
        {
            if (isBeingRescued) // cancel rescue if ship leaves rescue zone or starts moving
            {
                CancelRescue();
            }
        }
    }

    private void StartRescue()
    {
        isBeingRescued = true;
        TimerSystem.Instance.AddTimer(timerID, rescueTimeDuration, CompleteRescue); // start timer for rescue

        Debug.Log("Turtle rescue started.");
    }

    private void CancelRescue()
    {
        isBeingRescued = false;
        TimerSystem.Instance.CancelTimer(timerID); // ensure timer is cancelled
       
        Debug.Log("Moved out of zone, rescue cancelled.");
    }


    public void CompleteRescue()
    {
        isBeingRescued = false;

        // to do: add points to player score in GameManager

        TimerSystem.Instance.CancelTimer(timerID); // clean up timer
        Destroy(gameObject); // remove turtle from scene

        Debug.Log("Turtle rescue complete.");
    }
}
