using UnityEngine;

public abstract class HostileFish : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHits = 3;
    [SerializeField] protected float stunDuration = 1.0f;
    [SerializeField] protected float detectionRange = 8f;
    [SerializeField] protected float attackCooldown = 2.0f;

    [Header("State (Read Only)")]
    [SerializeField] protected int currentHits = 0;
    [SerializeField] protected bool isStunned = false;
    [SerializeField] protected bool isAttackReady = true;

    // Internal Timer IDs for the TimerSystem
    protected string stunTimerID;
    protected string attackTimerID;

    // References
    protected subScript submarineReference;
    protected Rigidbody2D rb; // <--- ADDED PHYSICS REFERENCE

    protected virtual void Start()
    {
        stunTimerID = $"Fish_Stun_{GetInstanceID()}";
        attackTimerID = $"Fish_Attack_{GetInstanceID()}";

        // Find Player
        submarineReference = FindFirstObjectByType<subScript>();
        if (submarineReference == null)
            Debug.LogError("HostileFish: No 'subScript' found in scene!");

        // --- PHYSICS SETUP ---
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true; // Prevent spinning like a ball
        }
        else
        {
            Debug.LogError("HostileFish: Missing Rigidbody2D component!");
        }
    }

    protected virtual void OnDestroy()
    {
        if (TimerSystem.Instance != null)
        {
            TimerSystem.Instance.CancelTimer(stunTimerID);
            TimerSystem.Instance.CancelTimer(attackTimerID);
        }
    }

    // CHANGED TO FIXEDUPDATE FOR PHYSICS STABILITY
    protected void FixedUpdate()
    {
        // 1. Kill "Push" Momentum
        // This prevents the fish from sliding/bouncing if the player hits it
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (isStunned) return;

        // 2. Run AI Logic
        UpdateAI(Time.fixedDeltaTime);
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
        });
    }

    protected virtual void Die()
    {
        // --- UPDATED: STATS INTEGRATION ---
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.AddStat("EnemiesDefeated", 1);
        }

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

            TimerSystem.Instance.AddTimer(attackTimerID, attackCooldown, () =>
            {
                isAttackReady = true;
            });
        }
    }

    protected abstract void UpdateAI(float deltaTime);
}