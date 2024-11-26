using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class watergun : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "WaterGun";
        }
    }

    public Sprite _Image = null;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }

    // This is the prefab that will be instantiated when the weapon is picked up
    public GameObject weaponPrefab;

    public void OnPickup()
    {
        // Hide the weapon model when picked up (or deactivate the pickup object)
        gameObject.SetActive(false);
    }

    // Add this method to get the weapon prefab
    public GameObject GetWeaponPrefab()
    {
        return weaponPrefab;
    }
}
