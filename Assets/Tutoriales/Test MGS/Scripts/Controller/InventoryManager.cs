using System.Collections;
using System.Collections.Generic;
using SA;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Transform rightHand;

    public WeaponItem currentWeapon {

        get{
            if (currentWeaponHook == null)
                return null;

            return currentWeaponHook.baseItem;
        }
    }
    public WeaponHook currentWeaponHook;

    public List<Item> pickedUpItems = new List<Item>();
    public List<WeaponItem> allWeapons;
    Dictionary<WeaponItem, WeaponHook> weaponsDict = new Dictionary<WeaponItem, WeaponHook>();

    Controller playerController;
    PassiveItem currentPassiveItem;

    private void Start()
    {
        if(allWeapons.Count > 0)
            LoadWeapon(allWeapons[0]);

        playerController = GetComponent<Controller>();
    }

    public void PickUpItem(Item item)
    {
        if (item is WeaponItem)
        {
            pickedUpItems.Add(item);

            WeaponItem w = (WeaponItem)item;
            if (allWeapons.Contains(w))
            {

            }
            else
            { 
                allWeapons.Add(w);
                LoadWeapon(w);
            }
        }

        if (item is PassiveItem)
        {
            pickedUpItems.Add(item);
        }
    }

    public void SwitchWeapon()
    {
        if (allWeapons.Count <= 0)
        { 
            return;
        }

        int index = 0;
        if (allWeapons.Contains(currentWeapon))
        {
            index = allWeapons.IndexOf(currentWeapon);
        }

        index++;
        if (index > allWeapons.Count - 1)
        {
            index = 0;
        }

        LoadWeapon(allWeapons[index]);
    }

    public void LoadItem(Item targetItem)
    {
        if (targetItem is WeaponItem)
        {
            LoadWeapon((WeaponItem)targetItem);
        }

        if (playerController != null)
        { 
            if (targetItem is PassiveItem)
            {
                if (currentPassiveItem != null)
                {
                    currentPassiveItem.OnUnEquip(playerController);
                }

                PassiveItem passive = (PassiveItem)targetItem;
                passive.OnEquip(playerController);
                currentPassiveItem = passive;
            }
        }
    }

    public void LoadWeapon(WeaponItem weaponItem)
    {
        if (currentWeaponHook != null)
        { 
            currentWeaponHook.gameObject.SetActive(false);
        }

        if (weaponsDict.ContainsKey(weaponItem))
        {
            weaponsDict.TryGetValue(weaponItem, out currentWeaponHook);            
        }
        else
        {
            GameObject go = Instantiate(weaponItem.prefab);
            go.transform.parent = rightHand;
            go.transform.localPosition = Vector3.zero; 
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            currentWeaponHook = go.GetComponent<WeaponHook>();
            weaponsDict.Add(weaponItem, currentWeaponHook);
        }

        currentWeaponHook.gameObject.SetActive(true);
        currentWeaponHook.Init(weaponItem);
    }
}

//[System.Serializable]
//public class Weapon
//{
//    public GameObject model;
//    public bool canMoveWithWeapon;
//    public float fireRate = .15f;
//    public ParticleSystem muzzle;
//    public float weaponSpread = .2f;
//    public bool hasPicked;
//}