using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform teleportDestination;

    private void OnTriggerEnter(Collider other)
    {
        // Log entry to confirm detection
        Debug.Log("Object entered trigger: " + other.name);

        // Set the position directly using the transform
        other.transform.position = teleportDestination.position;

        // If the object has a CharacterController, we need to disable it temporarily
        CharacterController controller = other.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
            controller.enabled = true; // Re-enable to ensure correct behavior after teleport
        }

        // Ensure that the root object of the player gets teleported, if it's a child object
        if (other.transform.root != other.transform)
        {
            other.transform.root.position = teleportDestination.position;
        }
    }
}
