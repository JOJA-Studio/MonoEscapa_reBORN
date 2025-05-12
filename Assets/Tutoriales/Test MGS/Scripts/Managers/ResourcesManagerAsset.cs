using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ResourcesManagerAsset : ScriptableObject
{
    public GameObject playerPrefab;
    public Item[] allItem;

    public Item[] GetAllItems()
    { 
        return allItem;
    }
}