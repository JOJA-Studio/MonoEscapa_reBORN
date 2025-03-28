using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angulo;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true) 
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FieldOfViewCheck()
    {
        Collider[] rangeCheck = Physics.OverlapSphere(transform.position, radius,targetMask);
        if (rangeCheck.Length != 0 ) 
        {
            Transform target = rangeCheck[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < angulo / 2)
            { 
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) 
                {
                    canSeePlayer = true;

                }
                else { canSeePlayer = false; }
            }
            else { canSeePlayer = false; }
        }
        else if ( canSeePlayer )
        {
            canSeePlayer = false;
        }
    }
}
