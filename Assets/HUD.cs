using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Inventory inventory;
    public Color selectedBorderColor = Color.green;
    public Color defaultBorderColor = Color.white;

    private GameObject currentWeapon; // Track the current weapon object

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

        // Instantiate and attach the weapon when added
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
                border.color = selectedBorderColor;
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
        // Destroy the current weapon if it exists
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instantiate the new weapon and attach it to the camera's child object
        currentWeapon = Instantiate(weaponPrefab);

        // Find the child object in the camera (e.g., an empty GameObject called "WeaponHolder")
        Transform weaponHolder = Camera.main.transform.Find("WeaponHolder");

        if (weaponHolder != null)
        {
            // Attach the weapon to the WeaponHolder (child of the camera)
            currentWeapon.transform.SetParent(weaponHolder);

            // Position the weapon in front of the camera with an offset
            Vector3 weaponOffset = new Vector3(0.5f, -0.2f, 1f); // Example offset
            currentWeapon.transform.localPosition = weaponOffset;

            // Reset the weapon's rotation to ensure consistent orientation
            currentWeapon.transform.localRotation = Quaternion.Euler(0, 0, 0); // Or match the camera's rotation
        }
        else
        {
            Debug.LogError("WeaponHolder child object not found!");
        }
    }
}



    private void SwitchWeapon(IInventoryItem item)
    {
        // Destroy the current weapon if it's instantiated
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
        }

        // Instantiate the new weapon
        AttachWeaponToPlayer(item);
    }
}
