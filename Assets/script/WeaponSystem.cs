using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

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


        if (projectilePrefab != null && firePoint != null)
        {
            // 1. Instantiate the projectile at the fire point with the sub's rotation
            GameObject newPulse = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

            // 2. Get the script on the newly spawned object
            ProjectileBehavior pScript = newPulse.GetComponent<ProjectileBehavior>();

            // 3. Set the range (Lifetime)
            if (pScript != null)
            {
                pScript.SetRange(attackRange);
            }
        }
        else
        {
            Debug.LogWarning("WeaponSystem: Projectile Prefab or FirePoint is missing!");
        }

        // 4. Start Cooldown
        StartCooldown();
    }


    private void StartCooldown()
    {
        isCoolingDown = true;

        // Use your custom TimerSystem if it exists
        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.AddTimer("weaponCooldown", coolDownDuration, ResetCooldown);
        }
        else
        {
            // Fallback: Use standard Unity Invoke if TimerSystem is missing
            Invoke(nameof(ResetCooldown), coolDownDuration);
        }
    }

    private void ResetCooldown()
    {
        isCoolingDown = false;
        Debug.Log("Weapon ready to fire again!");
    }
}
