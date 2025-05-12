using System.Collections;
using UnityEngine;

public static class ResourcesManager
{
    static ResourcesManagerAsset _resourcesManager;
    public static ResourcesManagerAsset singleton {
        get {
            if (_resourcesManager == null)
            { 
                _resourcesManager = Resources.Load("ResourcesManager") as ResourcesManagerAsset;
            }
        
            return _resourcesManager;   
        }

    }
}