using UnityEngine;

public class WeaponSystem : MonoBehaviour
{

    [SerializeField] private float attackRange = 10f; //how far the weapon can reach
    [SerializeField] private float coolDownDuration = 2f; //time between attacks
    private bool isCoolingDown = false; //flag to check if weapon is cooling down
  
   // public GameUI gameUI; 
    public subScript ship; // Reference to the ship's script
    private Collider2D[] hitBuffer = new Collider2D[5]; //buffer to store hit results, we can hit up to 5 targets at once
    [SerializeField] private LayerMask enemyLayer; //layer mask to filter for enemy targets


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCoolingDown)
        {
            // calculate elapsed time since cooldown started
            // calculate remaining time
            // update UI

            float progress = TimerSystem.Instance.GetProgress("weaponCooldown", coolDownDuration);
            Debug.Log($"Recharging... {progress * 100}%");
        }
        else
        {
            // gameUI.showAttackReady(); //indicate that attack is ready
        }
    }

    public void Attack()
    {
        if (isCoolingDown) // if weapon is cooling down, do not attack
            return;
        
        
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayer); //set filter to only detect enemies

        int hitCount = Physics2D.OverlapCircle(ship.transform.position,attackRange, filter, hitBuffer); //check for targets in range

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = hitBuffer[i];

            //uncomment when HostileFish script is ready
            
            /*if (hit != null && hit.TryGetComponent<HostileFish>(out HostileFish fish)) //check if hit object is a hostile fish
            {
                fish.takeHit(); //apply damage to the hostile fish
            }
            */
        }

        isCoolingDown = true; //start cooldown
       
       TimerSystem.Instance.AddTimer("weaponCooldown", coolDownDuration, ResetCooldown);
        
    }
    private void ResetCooldown()
    {
        isCoolingDown = false;
        Debug.Log("Weapon ready to fire again!");
    }
}
