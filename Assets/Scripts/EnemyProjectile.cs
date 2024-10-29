using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 30f; // Lifetime of the projectile in seconds

    void Start()
    {
        // Destroy the projectile after a specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Optional: Add logic for what happens when the projectile hits something
        // Destroy the projectile upon collision
        Destroy(gameObject);
    }
}
