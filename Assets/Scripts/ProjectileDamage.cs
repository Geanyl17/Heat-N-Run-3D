using UnityEngine;

public class ProjectileDamage : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.TakeDamage(damageAmount);

            Destroy(gameObject);
        }
    }
}
