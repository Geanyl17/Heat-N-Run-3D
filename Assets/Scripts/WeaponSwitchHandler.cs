using UnityEngine;

public class WeaponSwitchHandler : MonoBehaviour
{
    public WeaponSwitching weaponSwitching; // Drag your WeaponSwitching component in the Inspector

    void Start()
    {
        if (weaponSwitching == null)
        {
            weaponSwitching = GetComponent<WeaponSwitching>(); // Attempt to get it automatically
        }

        if (weaponSwitching == null)
        {
            Debug.LogError("WeaponSwitching component not found on this object or its children!"); // Log error if not found
        }
    }

    private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger entered with object: " + other.name);  // Log incoming collider

    if (other.CompareTag("WeaponPickup"))
    {
        WeaponPickup pickup = other.GetComponent<WeaponPickup>();

        if (pickup != null && weaponSwitching != null)
        {
            Debug.Log("WeaponPickup found with index: " + pickup.weaponIndex); // Log the weapon index
            Debug.Log("Equipping weapon with index: " + pickup.weaponIndex); // Log the index being set

            // Equip the weapon using the method in WeaponSwitching
            weaponSwitching.EquipWeapon(pickup.weaponIndex);  

            // Optionally destroy the pickup object
            Destroy(other.gameObject);
        }
        else
        {
            Debug.LogError("WeaponPickup or WeaponSwitching component not found!"); // Log error if not found
        }
    }
}

}
