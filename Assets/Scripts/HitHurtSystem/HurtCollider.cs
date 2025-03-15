using UnityEngine;
using UnityEngine.Events;


public class HurtCollider : MonoBehaviour
{

    public UnityEvent <HitCollider, HurtCollider> onHitRecived;


    public void NotifyHit(HitCollider hitCollider)
    {
        onHitRecived.Invoke(hitCollider, this);
    }
}
