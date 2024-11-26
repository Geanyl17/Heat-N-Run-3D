using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour
{
    public TextMeshProUGUI ammoText; // Reference to the UI Text component
    public Gun playerGun; // The player's current gun

    void Start()
    {

        if (ammoText == null)
        {
            ammoText = FindObjectOfType<TextMeshProUGUI>(); // Find Text UI element in scene if not assigned
        }

        if (playerGun != null && ammoText != null)
        {
            UpdateAmmoDisplay(playerGun.currentAmmo, playerGun.maxAmmo); // Initialize ammo display
        }
    }

    void Update()
    {
        // Continuously check for updates if needed, like during switching weapons
        if (playerGun != null && ammoText != null)
        {
            UpdateAmmoDisplay(playerGun.currentAmmo, playerGun.maxAmmo);
        }
        playerGun = FindObjectOfType<Gun>(); // Find the gun in the scene (this could be adjusted if there are multiple weapons)

    }

    // Update ammo display
    public void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    // Set the current weapon and its ammo
    public void SetCurrentWeapon(Gun newGun)
    {
        playerGun = newGun;
        UpdateAmmoDisplay(playerGun.currentAmmo, playerGun.maxAmmo); // Update ammo UI for new weapon
    }
}
