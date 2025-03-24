using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInventory : MonoBehaviour
{
    public interface InventoryItem 
    {
        string Name { get; }
        Sprite Image { get; }
        void OnPickup();
    }
    public class InventoryEvents : EventArgs
    { 
        public InventoryItem Item;
        public InventoryEvents(InventoryItem item) 
        {
            Item = item;
        }
    }
}
