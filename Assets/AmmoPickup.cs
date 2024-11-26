using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public int ammoAmount = 30; // Amount of ammo to replenish

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided is the player
        if (other.CompareTag("Player"))
        {
            // Get the gun script attached to the player
            Gun gun = other.GetComponentInChildren<Gun>(); // Assuming the gun is a child of the player

            if (gun != null)
            {
                // Replenish the gun's ammo
                gun.ReplenishAmmo(ammoAmount);
                Debug.Log("Ammo replenished!");

                // Destroy or disable the ammo pickup object after use
                Destroy(gameObject); // You can replace this with SetActive(false) to keep the object in the scene
            }
        }
    }
}
