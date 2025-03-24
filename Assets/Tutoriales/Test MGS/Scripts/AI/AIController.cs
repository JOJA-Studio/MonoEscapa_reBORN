using SA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

    NavMeshAgent agent;
    new Rigidbody rigidbody;
    Animator animator;

    public int index;
    public Waypoint[] waypoints;
    Waypoint currentWaypoint;
    Transform mTransform;

    public bool isAgressive;

    float waitTimer;

    public float rotateSpeed = .5f;
    public float fovRadius = 20;
    public float fovAngle = 45;
    
    public float attackDistance = 5;
    Vector3 lastKnownPosition;

    Controller currentTarget;
    LayerMask controllerLayer;

    private void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        currentWaypoint = waypoints[index];
        mTransform = this.transform;

        controllerLayer = (1 << 11);
    }
    private void Update()
    {
        float delta = Time.deltaTime;

        if (!isAgressive)
        {
            //Debug.Log(delta);
            HandleDetection();
            HandleNormalLogic(delta);
        }
        else
        {
            
        }
    }

    void HandleNormalLogic(float delta)
    {
        currentWaypoint = waypoints[index];
        //Debug.Log(currentWaypoint);

        float dis = Vector3.Distance(mTransform.position, currentWaypoint.tragetPosition.position);
        Debug.Log(dis);
        if (dis > agent.stoppingDistance)
        {
            //animator.SetFloat("movement", 1, .2f, delta);

            agent.updateRotation = true;
            if (agent.hasPath == false)
                agent.SetDestination(currentWaypoint.tragetPosition.position);
        }
        else
        {
            Debug.Log("Entra a else");
            //animator.SetFloat("movement", 0, .2f, delta);

            agent.updateRotation = false;
            Quaternion targetRot = Quaternion.Euler(currentWaypoint.lookEulers);
            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRot, delta / rotateSpeed);

            if (waitTimer < currentWaypoint.waitTime)
            {
                waitTimer += delta;
            }
            else
            { 
                waitTimer = 0;
                index++;
                if (index > waypoints.Length - 1)
                { 
                    index = 0;
                }
            }
        }
        
    }

    void HandleAggresiveLogic(float delta)
    {
        if (RaycastToTarget(currentTarget))
        {
            currentTarget = null;
        }

        float dis = Vector3.Distance(lastKnownPosition, mTransform.position);
        
        agent.SetDestination(lastKnownPosition);
        if (dis < attackDistance)
        {
            agent.isStopped = true;

            if (currentTarget != null)
            {
                Debug.Log("Attack");
            }
        }
        else
        {
            agent.isStopped = false;
        }
        

    }

    bool RaycastToTarget(Controller c)
    {
        Vector3 dir = c.mtransform.position - mTransform.position;
        dir.Normalize();
        float angle = Vector3.Angle(mTransform.forward, dir);

        if (angle < fovAngle)
        {
            Vector3 o = mTransform.position;
            o.y += 1;

            Debug.DrawRay(o, dir * 50, Color.red);
            if (Physics.Raycast(o, dir, out RaycastHit hit, 100))
            {
                Controller targetController = hit.transform.GetComponentInParent<Controller>();
                if (targetController != null)
                {
                    currentTarget = targetController;
                    isAgressive = true;
                    lastKnownPosition = currentTarget.transform.position;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void HandleDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(mTransform.position, fovRadius, controllerLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Controller c = colliders[i].transform.GetComponentInParent<Controller>();
            if (c != null)
            {
                if (RaycastToTarget(c))
                {
                    break;
                }
                
            }
        }
    }
}

[System.Serializable]
public class Waypoint
{
    public Transform tragetPosition;
    public Vector3 lookEulers;
    public float waitTime;
}
