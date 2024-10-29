using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = -1; // Start with no weapon equipped
    private bool weaponEquipped = false; // Track if a weapon has been picked up

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        // Allow scrolling only if a weapon is equipped
        if (weaponEquipped)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedWeapon >= transform.childCount - 1)
                    selectedWeapon = 0;
                else
                    selectedWeapon++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedWeapon <= 0)
                    selectedWeapon = transform.childCount - 1;
                else
                    selectedWeapon--;
            }
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(i == selectedWeapon); // Activate the weapon only if its index matches selectedWeapon
            i++;
        }
    }

    // Method to set the weaponEquipped status
    public void EquipWeapon(int index)
    {
        selectedWeapon = index; // Set the selected weapon index
        weaponEquipped = true; // Mark weapon as equipped
        SelectWeapon(); // Activate the selected weapon
    }
}
