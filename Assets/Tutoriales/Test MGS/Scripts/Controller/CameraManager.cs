using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject mainCameraObject;
    public GameObject wallCameraObject;
    public GameObject fpsCameraObject;
    public Transform camTransform;
    public Camera mainCamera;

    public float tiltAngle;
    public float tiltRotation = 5;

    public static CameraManager singleton;

    public void Init()
    { 
        wallCameraObject.transform.parent = transform;
        mainCameraObject.transform.parent = transform;
        this.transform.parent = null;

        DontDestroyOnLoad(this.gameObject);
    }

    public void HandleFPSTilt(float vertical, float delta)
    {
        tiltAngle -= vertical * tiltRotation;

        tiltAngle = Mathf.Clamp(tiltAngle, -35, 35); 
        fpsCameraObject.transform.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
        camTransform.rotation *= fpsCameraObject.transform.rotation; 
    }
}
