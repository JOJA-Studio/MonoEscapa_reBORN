using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class saveCubePlayer : MonoBehaviour
{
    GuidComponent guidComponent;

    void Awake()
    {
        guidComponent = GetComponent<GuidComponent>();
    }

    void Start()
    {
        loadSaveData();
    }

    void Update()
    {
        Debug.Log(guidComponent.GetGuid());
        saveData();
    }

    #region SaveGame

    private void saveData()
    { 
        SaveGameSystem.SetFloat(guidComponent.GetGuid() + SaveGameStrings.posicionX, transform.position.x);
        SaveGameSystem.SetFloat(guidComponent.GetGuid() + SaveGameStrings.posicionY, transform.position.y);
        SaveGameSystem.SetFloat (guidComponent.GetGuid() + SaveGameStrings.posicionZ, transform.position.z);
        
    }
    #endregion

    #region LoadGame
    private void loadSaveData()
    { 
    
        Vector3 loadedPosition = new Vector3();
        loadedPosition.x = SaveGameSystem.GetFloat(guidComponent.GetGuid() + SaveGameStrings.posicionX, transform.position.x);
        loadedPosition.y = SaveGameSystem.GetFloat(guidComponent.GetGuid() + SaveGameStrings.posicionY, transform.position.y);
        loadedPosition.z = SaveGameSystem.GetFloat(guidComponent.GetGuid() + SaveGameStrings.posicionZ, transform.position.z);

        transform.position = loadedPosition;
    }
    #endregion
}
