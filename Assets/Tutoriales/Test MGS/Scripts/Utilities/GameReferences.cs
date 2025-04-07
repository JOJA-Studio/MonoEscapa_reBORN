using System.Collections;
using UnityEngine;
using static Interfaces;
using UnityEngine.UIElements;

public static class GameReferences 
{
    static ObjectPooler _objectPooler;
    public static ObjectPooler objectPooler { 
        get{
            if (_objectPooler == null)
            {
                _objectPooler = Resources.Load("ObjectPooler") as ObjectPooler;
                _objectPooler.Init();
            }

            return _objectPooler;
        }
    }

    public static void RaycastShoot(Transform mTransform, float spread)
    {
        RaycastHit hit;
        Vector3 origin = Random.insideUnitCircle * spread;
        origin = mTransform.TransformPoint(origin);
        origin.y += 1.3f;
        origin += mTransform.forward;
        //origin += randomPosition;
        Debug.DrawRay(origin, mTransform.forward * 100, Color.white);

        Vector3 endPosition = origin + mTransform.forward * 100;

        if (Physics.Raycast(origin, mTransform.forward, out hit, 100))
        {
            IShootable shootable = hit.transform.GetComponentInParent<IShootable>();
            if (shootable != null)
            {
                GameObject fx = GameReferences.objectPooler.GetObject(shootable.GetHitFx());
                fx.transform.position = hit.point;
                fx.transform.rotation = Quaternion.LookRotation(hit.normal);
                fx.SetActive(true);
                Debug.Log("HIT OPONENT");
            }
            else
            {
                GameObject fx = GameReferences.objectPooler.GetObject("default");
                fx.transform.position = hit.point;
                fx.transform.rotation = Quaternion.LookRotation(hit.normal);
                fx.SetActive(true);
            }
            endPosition = hit.point;
        }
        GameObject go = objectPooler.GetObject("bulletLine");
        LineRenderer line  = go.GetComponent<LineRenderer>();
        line.SetPosition(0, origin);
        line.SetPosition(1, endPosition);
        go.SetActive(true);
    }
}
