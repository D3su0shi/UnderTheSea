using UnityEngine;

public class ChaserJellyfish : HostileFish
{
    public GraphNode currentGraphNode; // Starting node
    public float patrolSpeed = 2f;
    public float chaseSpeed = 6f;
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
        // Move towards player
        transform.position = Vector2.MoveTowards(transform.position, submarineReference.transform.position, chaseSpeed * dt);

        // Face the player
        RotateTowards(submarineReference.transform.position);

        // Try to attack if close enough
        float dist = Vector2.Distance(transform.position, submarineReference.transform.position);
        if (dist < 1.5f && CanAttack()) // 1.5f is contact range
        {
            Attack();
        }
    }

    private void PerformPatrol(float dt)
    {
        if (targetNode == null) return;

        // Move towards graph node
        transform.position = Vector2.MoveTowards(transform.position, targetNode.transform.position, patrolSpeed * dt);

        // Face the node
        RotateTowards(targetNode.transform.position);

        // Check arrival
        if (Vector2.Distance(transform.position, targetNode.transform.position) < arrivalThreshold)
        {
            // Pick next random neighbor from the Adjacency List
            targetNode = targetNode.GetRandomNeighbor();
        }
    }

    private void RotateTowards(Vector3 target)
    {
        Vector2 dir = target - transform.position;
        if (dir != Vector2.zero)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}