using UnityEngine;

public class OxygenBubble : MonoBehaviour
{
    [SerializeField]  private float dockingZoneRadius; //circular radius the ship must enter to collect oxygen
    [SerializeField] private float refillTimeDuration; //time taken to refill oxygen
    [SerializeField] private bool isRefilling = false; //flag to check if refilling is in progress
    //public GameUI gameUI; //reference to game UI
    public subScript ship; //reference to the ship
   
   private string timerID; //unique ID for this instance's timer
    void Start()
    {
        timerID = "OxygenBubbleRefill_" + gameObject.GetInstanceID();
    }

   
    void Update()
    {
       if (ship == null) return; // if ship reference is missing, exit

       float distanceToShip = Vector2.Distance(transform.position, ship.transform.position); // calculate distance to ship

       bool canRefill = (distanceToShip <= dockingZoneRadius && ship.isStationary()); // check if ship is in docking zone and stationary

       if (canRefill)
        {
            if (!isRefilling) // start refilling if not already in progress
            {
                StartRefill();
            }

            float refillProgress = TimerSystem.Instance.GetProgress(timerID, refillTimeDuration); // get refill progress from timer system
            // to do: update UI

        }
        else
        {
            if (isRefilling) // cancel refilling if ship leaves docking zone or starts moving
            {
                CancelRefill();
            }
        }
    }

    private void StartRefill()
    {
        isRefilling = true;
        TimerSystem.Instance.AddTimer(timerID, refillTimeDuration, CompleteRefill); // start timer for refill

        Debug.Log("Oxygen refill started.");
    }

    private void CancelRefill()
    {
        isRefilling = false;
        TimerSystem.Instance.CancelTimer(timerID); // ensure timer is cancelled
       
        Debug.Log("Moved out of zone, refill cancelled.");
    }

    private void CompleteRefill()
    {
        isRefilling = false;
        ship.RefillOxygen(); // refill ship's oxygen

        Debug.Log("Oxygen refill complete.");
    }

}
