using System.Collections;
using UnityEngine;

public class FireStatus : MonoBehaviour
{
    public float damageAmount = 5f;
    public float duration = 3f;
    public float damageInterval = 1f; // Interval between damage applications

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            StartCoroutine(ApplyDamageOverTime(playerStats));
        }
    }

    private IEnumerator ApplyDamageOverTime(PlayerStats playerStats)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            playerStats.TakeDamage(damageAmount);
            elapsed += damageInterval;
            yield return new WaitForSeconds(damageInterval);
        }

    }
}
