using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Unity.VisualScripting;

public class HitCollider : MonoBehaviour
{
    public UnityEvent<HitCollider, HurtCollider> onHitDelivered;
    [SerializeField] List<string> hittableTags;

    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    private void CheckCollider(Collider other)
    {
        Debug.Log(other);
        Debug.Log(other.tag);
        if (hittableTags.Contains(other.tag))
        {
            Debug.Log("hited");
            HurtCollider hurtCollider = other.GetComponent<HurtCollider>();
            if (hurtCollider)
            {
                Debug.Log("Has hurtCollider");
                hurtCollider.NotifyHit(this);
                onHitDelivered.Invoke(this, hurtCollider);
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollider(collision.collider);
    }
}
