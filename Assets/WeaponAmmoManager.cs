using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIManager : MonoBehaviour
{
    public Slider ammoSlider; // Reference to the UI Slider component
    public TextMeshProUGUI ammoText; // Optional: Reference to show numerical ammo
    public Gun playerGun; // The player's current gun

    void Start()
    {
        // Find and assign the slider if not already set
        if (ammoSlider == null)
        {
            ammoSlider = FindObjectOfType<Slider>(); // Finds a Slider in the scene if not assigned
        }

        if (ammoText == null)
        {
            ammoText = FindObjectOfType<TextMeshProUGUI>(); // Find Text UI element in scene if not assigned
        }

        if (playerGun != null && ammoSlider != null)
        {
            InitializeAmmoUI(playerGun.currentAmmo, playerGun.maxAmmo);
        }
    }

    void Update()
    {
        // Continuously check for updates if needed, like during switching weapons
        if (playerGun != null && ammoSlider != null)
        {
            UpdateAmmoDisplay(playerGun.currentAmmo, playerGun.maxAmmo);
        }

        // Dynamically find the player's gun (if switching weapons is possible)
        playerGun = FindObjectOfType<Gun>();
    }

    // Initialize the ammo UI
    private void InitializeAmmoUI(int currentAmmo, int maxAmmo)
    {
        // Set slider's max value to max ammo
        ammoSlider.maxValue = maxAmmo;
        ammoSlider.value = currentAmmo;

        // Update numerical text if available
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo} / {maxAmmo}";
        }
    }

    // Update ammo display
    public void UpdateAmmoDisplay(int currentAmmo, int maxAmmo)
    {
        if (ammoSlider != null)
        {
            // Update slider value
            ammoSlider.value = currentAmmo;

            // Optionally update numerical text
            if (ammoText != null)
            {
                ammoText.text = $"{currentAmmo} / {maxAmmo}";
            }
        }
    }

    // Set the current weapon and its ammo
    public void SetCurrentWeapon(Gun newGun)
    {
        playerGun = newGun;

        if (playerGun != null && ammoSlider != null)
        {
            // Update slider max value for new weapon
            ammoSlider.maxValue = playerGun.maxAmmo;
            UpdateAmmoDisplay(playerGun.currentAmmo, playerGun.maxAmmo);
        }
    }
}
