using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiRotator : MonoBehaviour
{
    public GameObject TextName;
    public GameObject markRoattion;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TextName != null)
        { 
            TextName.transform.LookAt(Camera.main.transform.position);
            TextName.transform.Rotate(0, 180, 0);
        }

        if (markRoattion != null)
        {
            markRoattion.transform.LookAt(Camera.main.transform.position);
            markRoattion.transform.Rotate(0, 180, 0);
        }
    }
}
