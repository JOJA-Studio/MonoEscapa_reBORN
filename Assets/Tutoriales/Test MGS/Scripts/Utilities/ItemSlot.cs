using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using SA.Utilities;


public class ItemSlot : MonoBehaviour
{
    public Image img;
    public Item targetItem;

    public void LoadItem(Item targetItem)
    { 
        this.targetItem = targetItem;

        if (targetItem.inventoryIcon == null)
        {
            IconMaker.RequestIcon(targetItem, LoadIcon);
        }
    }

    void LoadIcon()
    { 
        img.sprite = targetItem.inventoryIcon;
    }
}
