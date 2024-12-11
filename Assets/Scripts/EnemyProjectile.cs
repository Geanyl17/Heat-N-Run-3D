using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifetime = 30f; // Lifetime of the projectile in seconds
    public float fireDamage = 5f; // Damage amount for fire effect
    public float fireDuration = 3f; // Duration of the fire effect
    public float fireDamageInterval = 1f; // Interval between fire damage applications
    public GameObject fireParticlePrefab; // Fire particle effect prefab

    void Start()
    {
        // Destroy the projectile after a specified lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the PlayerStats component
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            // Apply the fire effect to the player
            StartCoroutine(ApplyFireEffect(playerStats));
        }

        // Destroy the projectile
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator ApplyFireEffect(PlayerStats playerStats)
    {
        float elapsed = 0f;

        // Spawn fire particles on the target
        GameObject fireEffect = null;
        if (fireParticlePrefab != null)
        {
            fireEffect = Instantiate(fireParticlePrefab, playerStats.transform);
            fireEffect.transform.localPosition = Vector3.zero; // Adjust position as needed
        }

        // Apply fire damage over time
        while (elapsed < fireDuration)
        {
            playerStats.TakeDamage(fireDamage);
            elapsed += fireDamageInterval;
            yield return new WaitForSeconds(fireDamageInterval);
        }

        // Destroy the fire effect once the fire duration ends
        if (fireEffect != null)
        {
            Destroy(fireEffect);
        }
    }
}
