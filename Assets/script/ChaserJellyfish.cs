using UnityEngine;

public class ChaserJellyfish : HostileFish
{
    public GraphNode currentGraphNode; // Starting node
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float arrivalThreshold = 0.5f;

    // Internal navigation state
    private GraphNode targetNode;
    private bool isChasing = false;

    protected override void Start()
    {
        base.Start(); // Run the HostileFish setup (finding the player)

        // Initialize Graph Logic
        if (currentGraphNode != null)
        {
            targetNode = currentGraphNode;
        }
    }

    protected override void UpdateAI(float deltaTime)
    {
        if (submarineReference == null) return;

        // 1. Check State Transition
        bool playerInSight = IsSubmarineInRange();

        if (playerInSight)
        {
            isChasing = true;
        }
        else if (isChasing && !playerInSight)
        {
            // Keep chasing for a bit, or return to patrol immediately?
            float dist = Vector2.Distance(transform.position, submarineReference.transform.position);
            if (dist > detectionRange * 1.5f)
            {
                isChasing = false;
            }
        }

        // 2. Execute Movement
        if (isChasing)
        {
            PerformChase(deltaTime);
        }
        else
        {
            PerformPatrol(deltaTime);
        }
    }

    private void PerformChase(float dt)
    {
        // 1. Calculate the new 2D position
        Vector2 newPos = Vector2.MoveTowards(transform.position, submarineReference.transform.position, chaseSpeed * dt);

        // 2. Assign back to transform.position, preserving the current Z
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);

        // Try to attack if close enough
        float dist = Vector2.Distance(transform.position, submarineReference.transform.position);
        if (dist < 1.5f && CanAttack())
        {
            Attack();
        }
    }

    private void PerformPatrol(float dt)
    {
        if (targetNode == null) return;

        // 1. Calculate the new 2D position
        Vector2 newPos = Vector2.MoveTowards(transform.position, targetNode.transform.position, patrolSpeed * dt);

        // 2. Assign back to transform.position, preserving the current Z
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);


        // Check arrival (using Vector2.Distance ignores Z difference, which is good)
        if (Vector2.Distance(transform.position, targetNode.transform.position) < arrivalThreshold)
        {
            targetNode = targetNode.GetRandomNeighbor();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}