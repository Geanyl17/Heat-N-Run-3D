using UnityEngine;
using TMPro; // Required for TextMeshPro

public class InteractableObjectScript : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private bool isReloading = false;
    private float reloadTime = 2f; // Time to hold "E" for reload
    private float currentReloadTime = 0f;
    [SerializeField] private GameObject interactionText; // UI text to show interaction


    void Start()
    {
        if (interactionText != null)
        {
            interactionText.SetActive(false); // Hide the interaction prompt initially
        }
    }

    void Update()
    {
        // When the player is nearby and holding "E"
        if (isPlayerNearby && !isReloading && Input.GetKey(KeyCode.E))
        {
            currentReloadTime += Time.deltaTime;

            // Update UI to show the hold time
            if (interactionText != null)
            {
                float holdProgress = currentReloadTime / reloadTime;
                interactionText.GetComponent<TextMeshProUGUI>().text = $"Hold E to reload: {Mathf.CeilToInt(holdProgress * 100)}%";
            }

            // Reload the weapon after 2 seconds of holding "E"
            if (currentReloadTime >= reloadTime)
            {
                ReloadWeapon();
            }
        }
        else if (isPlayerNearby && !Input.GetKey(KeyCode.E))
        {
            // Reset the reload progress when the key is released
            currentReloadTime = 0f;
            if (interactionText != null)
            {
                interactionText.GetComponent<TextMeshProUGUI>().text = "Press E to reload";
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // Enable the interaction UI prompt
            if (interactionText != null)
            {
                interactionText.SetActive(true);
                interactionText.GetComponent<TextMeshProUGUI>().text = "Press E to reload";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            // Disable the interaction UI prompt when leaving the trigger
            if (interactionText != null)
            {
                interactionText.SetActive(false);
            }
        }
    }

    private void ReloadWeapon()
    {
        Gun gun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();

        if (gun != null)
        {
            // Replenish ammo using the method from the Gun script
            gun.ReplenishAmmo(gun.maxAmmo); // Using maxAmmo or a defined ammo value
            Debug.Log("Weapon reloaded!");

            // Reset the UI and reload state
            if (interactionText != null)
            {
                interactionText.SetActive(false); // Hide the interaction prompt
            }

            isReloading = true;
            currentReloadTime = 0f;

            // Optionally, disable reloading after a short delay
            Invoke("ResetReloading", 1f); // 1 second delay before allowing interaction again
        }
    }


    private void ResetReloading()
    {
        isReloading = false;
    }
}
