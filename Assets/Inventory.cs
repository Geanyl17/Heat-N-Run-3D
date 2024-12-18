using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 2;

    private List<IInventoryItem> mItems = new List<IInventoryItem>();

    public event EventHandler<InventoryEventArgs> ItemAdded;

    // Public property to access the inventory items
    public List<IInventoryItem> Items
    {
        get { return mItems; }
    }

    public void AddItem(IInventoryItem item)
    {
        if (mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);
                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
    }

    // Method to set the current item in the inventory (optional)
    public void SetCurrentItem(int index)
    {
        if (index >= 0 && index < mItems.Count)
        {
            // Code to set the current item (if needed)
            ItemChanged?.Invoke(this, new ItemChangedEventArgs(mItems[index]));
        }
    }

    public event EventHandler<ItemChangedEventArgs> ItemChanged;
}

public class ItemChangedEventArgs : EventArgs
{
    public IInventoryItem Item { get; private set; }

    public ItemChangedEventArgs(IInventoryItem item)
    {
        Item = item;
    }
}
