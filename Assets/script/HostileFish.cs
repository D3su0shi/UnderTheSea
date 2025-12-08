using UnityEngine;

public abstract class HostileFish : MonoBehaviour
{
    [SerializeField] public int maxHits = 3;
    [SerializeField] protected float stunDuration = 1.0f;
    [SerializeField] protected float detectionRange = 8f;
    [SerializeField] protected float attackCooldown = 2.0f;

    [SerializeField] public int currentHits = 0;
    [SerializeField] protected bool isStunned = false;
    [SerializeField] protected bool isAttackReady = true; 

    // Internal Timer IDs for the TimerSystem
    protected string stunTimerID;
    protected string attackTimerID;

    // References
    protected subScript submarineReference;

    protected virtual void Start()
    {
        stunTimerID = $"Fish_Stun_{GetInstanceID()}";
        attackTimerID = $"Fish_Attack_{GetInstanceID()}";

        // Find Player
        submarineReference = FindFirstObjectByType<subScript>();
        if (submarineReference == null)
            Debug.LogError("HostileFish: No 'subScript' found in scene!");
    }

    protected virtual void OnDestroy()
    {
        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.CancelTimer(stunTimerID);
            TimerSystem.Instance.CancelTimer(attackTimerID);
        }
    }

    protected void Update()
    {

        if (isStunned) return; 
        UpdateAI(Time.deltaTime);
    }

    public void TakeHit()
    {
        currentHits++;

        Debug.Log($"{gameObject.name} hit! ({currentHits}/{maxHits})");

        // Check Death first
        if (currentHits >= maxHits)
        {
            Die();
            return;
        }

        // Trigger Stun using TimerSystem
        StartStun();
    }

    private void StartStun()
    {
        if (TimerSystem.Instance == null) return;

        isStunned = true;

        // 1. Cancel existing stun timer (refreshes the duration if hit again)
        TimerSystem.Instance.CancelTimer(stunTimerID);

        // 2. Add new timer
        TimerSystem.Instance.AddTimer(stunTimerID, stunDuration, () =>
        {
            // Callback when time is up:
            isStunned = false;
            // Optional: Add visual code here to return color to normal
        });
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected bool IsSubmarineInRange()
    {
        if (submarineReference == null) return false;
        return Vector2.Distance(transform.position, submarineReference.transform.position) <= detectionRange;
    }

    protected bool CanAttack()
    {
        return isAttackReady && !isStunned;
    }

    protected virtual void Attack()
    {
        if (submarineReference != null && TimerSystem.Instance != null)
        {
            // 1. Deal Damage
            submarineReference.TakeDamage(10f);
            Debug.Log("Fish Attacked Player!");

            // 2. Start Cooldown using TimerSystem
            isAttackReady = false;

            // Note: We don't need to cancel previous attack timers because 
            // we wouldn't be here if isAttackReady was false.
            TimerSystem.Instance.AddTimer(attackTimerID, attackCooldown, () =>
            {
                isAttackReady = true;
            });
        }
    }

    protected abstract void UpdateAI(float deltaTime);
}