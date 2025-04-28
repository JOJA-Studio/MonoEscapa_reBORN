using SA;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static Interfaces;

public class AIController : MonoBehaviour, IShootable
{

    NavMeshAgent agent;
    new Rigidbody rigidbody;
    public Animator animator;

    public int index;
    public Waypoint[] waypoints;
    Waypoint currentWaypoint;
    Transform mTransform;

    public bool isDead;
    public bool isGrab;
    public bool isAgressive;
    public bool isCaution;
    public float cautionTimerNormal = 0.7f;
    float cautionTimer;

    float waitTimer;

    public float normalSpeed = 2;
    public float aggresiveSpeed = 4;
    public float rotateSpeed = .5f;
    public float fovRadius = 20;
    public float fovAngle = 45;
    
    public float attackDistance = 5;
    Vector3 lastKnownPosition;
    Vector3 lastKnownDirection;

    Controller currentTarget;
    LayerMask controllerLayer;

    public int magazineBullets = 40;
    int bulletsToFire;
    int timesShot;

    public int timesStruggle;

    InventoryManager inventoryManager;

    private void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        rigidbody = GetComponentInChildren<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        inventoryManager = GetComponentInChildren<InventoryManager>();
        currentWaypoint = waypoints[index];
        mTransform = this.transform;
        animator.applyRootMotion = false;
        controllerLayer = (1 << 11 | 1 << 14);
    }
    private void Update()
    {
        float delta = Time.deltaTime;

        if (isGrab)
        { 
           // agent.isStopped = true;
            return;
        }

        if (animator.GetBool("isInteracting"))
        {
            agent.isStopped = true;
            if (animator.GetBool("canRotate"))
            {
                HandleLookAtTarge(delta);
            }

            return;
        }

        if (!isAgressive)
        {
            //Debug.Log(delta);
            agent.speed = normalSpeed;
            HandleDetection();
            HandleNormalLogic(delta);
        }
        else
        {
            if (isCaution)
            {
                if (cautionTimer < 0)
                { 
                    isCaution = false;
                    //animator.SetBool("isCaution", false);
                    agent.isStopped = false;
                }
                else
                {
                    if (animator.GetBool("canRotate"))
                    {
                        HandleLookAtTarge(delta);
                    }

                    HandleLookAtTarge(delta);
                    agent.isStopped = true;
                    cautionTimer -= delta;
                }
            }
            else
            {
                agent.speed = aggresiveSpeed;
                HandleAggresiveLogic(delta);
            }

        }
    }

    void HandleNormalLogic(float delta)
    {
        currentWaypoint = waypoints[index];
        //Debug.Log(currentWaypoint);

        float dis = Vector3.Distance(mTransform.position, currentWaypoint.tragetPosition.position);
        //Debug.Log(dis);
        if (dis > agent.stoppingDistance)
        {
            animator.SetFloat("movement", 1, .1f, delta);

            agent.updateRotation = true;
            if (agent.hasPath == false)
                agent.SetDestination(currentWaypoint.tragetPosition.position);
        }
        else
        {
           // Debug.Log("Entra a else");

            animator.SetFloat("movement", 0, .1f, delta);
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

    public float fireRate = .1f;
    float currentFire;
    bool initRange;

    void HandleAggresiveLogic(float delta)
    {
        if (currentTarget != null)
        {
            if (!RaycastToTarget(currentTarget))
            {
                lastKnownDirection = (currentTarget.mtransform.position - lastKnownPosition).normalized;
                currentTarget = null;
            }
        }

        bool inRange = false;

        float dis = Vector3.Distance(lastKnownPosition, mTransform.position);
        agent.SetDestination(lastKnownPosition);
        
        #region Handle Raycast to target
        if (currentTarget != null)
        {
            if (dis < attackDistance)
            {
                inRange = true;

                if (!initRange)
                {
                    AssignRandomBulletsToFire();
                    PlayCautionState(cautionTimerNormal, delta, false);
                    currentFire = fireRate;
                    initRange = true;
                }
                agent.isStopped = true;


                HandleLookAtTarge(delta);

                if (currentFire < 0)
                {
                    currentFire = fireRate;
                    HandleShooting();
                    if (bulletsToFire <= 0)
                    {
                        AssignRandomBulletsToFire();
                        PlayCautionState(cautionTimerNormal, delta, false);
                    }
                }
                else
                {
                    currentFire -= delta;
                }
            }
            else
            {
                initRange = false;
                agent.updateRotation = true;
                agent.isStopped = false;
                HandleDetection();

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    HandleRotation(lastKnownPosition, delta);
                }
            }
        }
        else
        {
            initRange = false;
            agent.updateRotation = true;
            agent.isStopped = false;
            HandleDetection();
        }
        #endregion

        #region Handle animations
        if (currentTarget != null)
        { 
            if (!inRange)
            {
                animator.SetFloat("movement", 1, .1f, delta);
            }
            else
            {
                animator.SetFloat("movement", 0);
            }
        }
        else
        {
            if (agent.desiredVelocity.magnitude > 0)
            {
                animator.SetFloat("movement", 1, .1f, delta);
            }
            else
            {
                animator.SetFloat("movement", 0, .1f, delta);
            }
        }
        #endregion
    }

    public ParticleSystem muzzleFire;
    public float weaponSpread = .3f;

    void AssignRandomBulletsToFire()
    {
        bulletsToFire = Random.Range(5, 20);
        int bl = magazineBullets - timesShot;

        if (bulletsToFire > bl)
        {
            bulletsToFire = bl;
        }
    }

    void HandleLookAtTarge(float delta)
    {
        Vector3 dir = currentTarget.transform.position - mTransform.position;
       

        HandleRotation(dir, delta);
    }

    void HandleRotation(Vector3 dir, float delta)
    {
        dir.y = 0;
        if (dir == Vector3.zero)
            dir = mTransform.forward;
        
        Quaternion targetRot = Quaternion.LookRotation(dir);
        mTransform.rotation = Quaternion.Slerp(mTransform.rotation, targetRot, delta / rotateSpeed);
        agent.updateRotation = false;
    }

    void HandleShooting()
    {
        timesShot++;
        bulletsToFire--;
        //muzzleFire.Play();
        GameReferences.RaycastShoot(mTransform, inventoryManager.currentWeaponHook);
        inventoryManager.currentWeaponHook.Shoot();

        if (timesShot > magazineBullets)
        { 
            timesShot = 0;
            animator.CrossFade("Reload", 0.2f);
            animator.CrossFade("Reload_Body", 0.2f);
            //animator.SetBool("isInteracting", true);
        }


    }

    void PlayCautionState(float timer, float delta, bool crossfadeToState = true)
    {
        isCaution = true;
        cautionTimer = timer;
        if(crossfadeToState)
            animator.CrossFade("caution", 0.2f);
        animator.SetFloat("movement", 0, 0.1f, delta);
        animator.SetBool("isCaution", true);

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
                    if (!isAgressive || currentTarget == null)
                    {
                        cautionTimer = cautionTimerNormal;

                        isCaution = true;
                        isAgressive = true;
                        //animator.SetBool("isCaution", true);
                       // animator.CrossFade("caution", 0.2f);
                    }

                    currentTarget = targetController;
                    animator.SetBool("isAggressive", true);
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

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }

    public string hitFx = "blood";

    public string GetHitFx()
    {
        return hitFx;
    }

    public void StartGrab(Vector3 tp, Quaternion targetRotation)
    {
        agent.enabled = false;
        mTransform.position = tp;
        isGrab = true;
        animator.Play("e_Grab_start");
        mTransform.rotation = targetRotation;
    }

    public void KillByGrab()
    { 
        animator.Play("grab_death");
        this.enabled = false;
        isDead = true;
    }

    public void StopGrab(Controller target)
    {
        currentTarget = target;
        lastKnownPosition = currentTarget.mtransform.position;
        agent.enabled = true;
        agent.updateRotation = false;
        isGrab = false;
        animator.Play("e_grab_cancel");
        PlayCautionState(cautionTimerNormal, Time.deltaTime, false);
    }
}

[System.Serializable]
public class Waypoint
{
    public Transform tragetPosition;
    public Vector3 lookEulers;
    public float waitTime;
}
