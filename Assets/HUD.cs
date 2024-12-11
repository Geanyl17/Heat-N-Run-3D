using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory inventory;
    public Color selectedBorderColor = new Color(0f, 0f, 0f, 86f); // Transparent white
    public Color defaultBorderColor = new Color(1f, 1f, 1f, 125f); // Transparent white


    private GameObject currentWeapon; // Track the current weapon object
    private Dictionary<string, GameObject> weaponInstances = new Dictionary<string, GameObject>(); // Track instantiated weapons by name
    public AmmoUIManager ammoUIManager; // Reference to AmmoUIManager to update ammo UI

    void Start()
    {
        inventory.ItemAdded += InventoryScript_ItemAdded;
        inventory.ItemChanged += InventoryScript_ItemChanged;
    }

    private void Update()
    {
        // Switch items using keys 1-4
        if (Input.GetKeyDown(KeyCode.Alpha1)) inventory.SetCurrentItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) inventory.SetCurrentItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) inventory.SetCurrentItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) inventory.SetCurrentItem(3);
    }

    private void InventoryScript_ItemAdded(object sender, InventoryEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        // Add item image to the inventory slot
        foreach (Transform slot in inventoryPanel)
        {
            Image image = slot.Find("border/itemimage")?.GetComponent<Image>();
            if (image != null && !image.enabled)
            {
                image.enabled = true;
                image.sprite = e.Item.Image;
                break;
            }
        }

        // Instantiate and attach the weapon to player when it's added to the inventory
        AttachWeaponToPlayer(e.Item);
    }

    private void InventoryScript_ItemChanged(object sender, ItemChangedEventArgs e)
    {
        Transform inventoryPanel = transform.Find("Inventory");

        // Reset all border colors
        foreach (Transform slot in inventoryPanel)
        {
            Image border = slot.Find("border")?.GetComponent<Image>();
            if (border != null)
            {
                border.color = defaultBorderColor;
            }
        }

        // Highlight the selected item's border
        foreach (Transform slot in inventoryPanel)
        {
            int slotIndex = slot.GetSiblingIndex();
            Image border = slot.Find("border")?.GetComponent<Image>();

            if (border != null && slotIndex == inventory.Items.IndexOf(e.Item))
            {
                border.color = selectedBorderColor;  // Use the selected color set in the Inspector
                break;
            }
        }

        // Log the selected item
        Debug.Log($"Selected Item: {e.Item.Name}");

        // Switch the weapon based on the selected item
        SwitchWeapon(e.Item);
    }

    private void AttachWeaponToPlayer(IInventoryItem item)
    {
        GameObject weaponPrefab = item.GetWeaponPrefab(); // Get the weapon prefab

        if (weaponPrefab != null)
        {
            // Check if the weapon is already instantiated (using name as key)
            if (!weaponInstances.ContainsKey(weaponPrefab.name))
            {
                // Instantiate the weapon and store it in the dictionary
                GameObject weaponInstance = Instantiate(weaponPrefab);

                // Parent the weapon to the 'weaponHolder' child of the main camera
                Transform weaponHolder = Camera.main.transform.Find("WeaponHolder");

                if (weaponHolder != null)
                {
                    weaponInstance.transform.SetParent(weaponHolder);
                }

                // Position and rotate the weapon correctly
                weaponInstance.transform.localPosition = Vector3.zero; // Adjust position as needed
                weaponInstance.transform.localRotation = Quaternion.identity; // Adjust rotation as needed

                // Store the instantiated weapon in the dictionary
                weaponInstances[weaponPrefab.name] = weaponInstance;
            }

            // Deactivate all weapons
            foreach (var weapon in weaponInstances.Values)
            {
                weapon.SetActive(false);
            }

            // Activate the selected weapon
            GameObject selectedWeapon = weaponInstances[weaponPrefab.name];
            selectedWeapon.SetActive(true);
            currentWeapon = selectedWeapon;

            // Update Ammo UI for the new weapon
            if (ammoUIManager != null)
            {
                ammoUIManager.SetCurrentWeapon(selectedWeapon.GetComponent<Gun>());
            }
        }
    }

    private void SwitchWeapon(IInventoryItem item)
    {
        // Switch the weapon based on the selected item
        AttachWeaponToPlayer(item);
    }
}
