using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

public class HitCollider : MonoBehaviour, IHitter
{
    public UnityEvent <HitCollider, HurtCollider> onHitDelivered;
    [SerializeField] List<string> hittableTags;
    [SerializeField] float damage = 0.25f;

    private void OnTriggerEnter(Collider other)
    {
        CheckCollider(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollider(collision.collider);
    }

    private void CheckCollider(Collider other)
    {
        if (hittableTags.Contains(other.tag))
        {
            HurtCollider hurtCollider= other.GetComponent<HurtCollider>();
            if(hurtCollider)
            {
                hurtCollider.NotifyHit(this);
                onHitDelivered.Invoke(this, hurtCollider);
            }
        }
    }

    float IHitter.GetDamage()
    {
        return damage;
    }

    Transform IHitter.GetTransform()
    {
        return transform;   
    }
}
