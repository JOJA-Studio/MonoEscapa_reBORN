using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    private Rigidbody rigidocuerpo;
    private bool targetHit;
    // Start is called before the first frame update
    void Start()
    {
        rigidocuerpo = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (targetHit) { return; }
        else 
        { 
            targetHit = true;
            rigidocuerpo.isKinematic = true;
            transform.SetParent(collision.transform);
        }
        
    }
}
