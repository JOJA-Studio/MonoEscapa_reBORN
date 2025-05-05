using UnityEngine;
using UnityEngine.Events;

public class HurtCollider : MonoBehaviour
{
    public UnityEvent <IHitter, HurtCollider> onHitRecived;

    public void NotifyHit(IHitter iHitter)
    {
        onHitRecived.Invoke(iHitter, this);
    }

}
