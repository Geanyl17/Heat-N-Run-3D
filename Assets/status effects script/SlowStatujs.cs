using System.Collections;
using UnityEngine;

public class SlowStatjus : MonoBehaviour
{
    public float slowSpeed = 2f; // Speed to set when player is slowed
    public float slowDuration = 5f; // Duration of slow effect after exit

    private PlayerMovement playerMovement; // Reference to PlayerMovement script
    private float originalWalkSpeed;
    private float originalRunSpeed;

    private void OnTriggerEnter(Collider other)
    {
        playerMovement = other.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            // Store the original speeds
            originalWalkSpeed = playerMovement.walkSpeed;
            originalRunSpeed = playerMovement.runSpeed;

            // Apply the slow effect
            playerMovement.walkSpeed = slowSpeed;
            playerMovement.runSpeed = slowSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerMovement != null)
        {
            StartCoroutine(RestoreSpeedAfterDelay());
        }
    }

    private IEnumerator RestoreSpeedAfterDelay()
    {
        yield return new WaitForSeconds(slowDuration);

        // Restore the original speeds
        playerMovement.walkSpeed = originalWalkSpeed;
        playerMovement.runSpeed = originalRunSpeed;
    }
}
