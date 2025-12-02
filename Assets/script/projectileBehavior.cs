using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        MoveProjectile();
    }

    void MoveProjectile()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    public void SetRange(float range)
    {
        float calculatedLifeTime = range / speed;

        // Destroy the projectile after that calculated time
        Destroy(gameObject, calculatedLifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("sub"))
        {
        

            Destroy(gameObject); // Destroy pulse on impact
        }
    }
}