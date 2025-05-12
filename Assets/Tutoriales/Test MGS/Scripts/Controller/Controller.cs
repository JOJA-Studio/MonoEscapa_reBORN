using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interfaces;

namespace SA
{
    public class Controller : MonoBehaviour, IShootable, IPointOfInterest
    {
        public new Rigidbody rigidbody;
        public float moveSpeed = .4f;
        public float grabSpeed = .4f;
        public float proneSpeed = .4f;
        public float wallSpeed = .4f;
        public float rotateSpeed = .2f;
        public float fpsRotateSpeed = .2f;
        public float wallCheckDis = .2f;
        public float aimSpeed = 1;
        public Transform mTransform
        {
            get
            {
                if (_mTransform == null)
                {
                    _mTransform = this.transform;
                }

                return _mTransform;
            }
        }
        Transform _mTransform;

        [HideInInspector]
        public Animator animator;
        [HideInInspector]
        public InventoryManager inventoryManager;

        public ControllerState controllerState;
        public bool isAiming;
        public bool isWall;
        public bool isFreelook;
        public bool isInteracting;
        public bool isGrab;
        public bool isFPS;
        AIController currentGrabbed;

        public bool isCrouch
        {
            get
            {
                return _isCrouch;
            }
            set
            {
                animator.SetBool("isProne", false);
                _isCrouch = value;
            }
        }
        bool _isCrouch;
        public bool isProne;
        public float wallCamXPos = 1;
        public Transform wallCamParent;
        public Vector3 startWallCamPosition;
        CapsuleCollider controllerCollider;

        [HideInInspector]
        public GameObject storedObject;
        [HideInInspector]
        public Animator boxAnimator;

        public PoseStats standing;
        public PoseStats crouching;
        public float getWallDetectOrigin
        {
            get
            {
                if (isCrouch)
                {
                    return crouching.wallDetectHeight;
                }
                else
                {
                    return standing.wallDetectHeight;
                }
            }
        }

        public enum ControllerState
        {
            normal, cardboardBox, prone
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            inventoryManager = GetComponent<InventoryManager>();
            controllerCollider = GetComponent<CapsuleCollider>();
            startWallCamPosition = wallCamParent.localPosition;

            UpdatePoseStats(standing);
        }

        public void WallMovement(Vector3 moveDirection, Vector3 normal, float delta, LayerMask layerMask)
        {
            float dot = Vector3.Dot(moveDirection, Vector3.forward);
            Vector3 wallCamTargetPosition = startWallCamPosition;

            if (dot < 0)
            {
                moveDirection.x *= -1;
            }

            HandleRotation(normal, delta);

            Vector3 projectVel = Vector3.ProjectOnPlane(moveDirection, normal);
            Debug.DrawRay(mTransform.position, projectVel, Color.blue);

            Vector3 relativeDir = mTransform.InverseTransformDirection(projectVel);

            Vector3 origin = mTransform.position;
            origin.y += 1;
            if ((Mathf.Abs(relativeDir.x) > 0.01f))
            {
                if (relativeDir.x > 0)
                    origin += mTransform.right * wallCheckDis;
                if (relativeDir.x < 0)
                    origin -= mTransform.right * wallCheckDis;

                Debug.DrawRay(origin, -normal, Color.red);
                if (Physics.Raycast(origin, -normal, out RaycastHit hit, 2, layerMask))
                {

                }
                else
                {
                    projectVel = Vector3.zero;
                    wallCamTargetPosition.x = wallCamXPos * ((relativeDir.x < 0) ? -1 : 1);
                    relativeDir.x = 0;
                }
            }
            else
            {
                projectVel = Vector3.zero;
                relativeDir.x = 0;
            }

            rigidbody.velocity = projectVel * wallSpeed;

            float m = 0;

            m = relativeDir.x;

            if (m < 0.1f && m > -0.1f)
            {
                m = 0;
            }
            else
            {
                m = (m < 0) ? -1 : 1;
            }

            animator.SetFloat("movement", m, 0.1f, delta);

            wallCamParent.localPosition = Vector3.Lerp(wallCamParent.localPosition, wallCamTargetPosition, delta / 0.2f);

        }

        public void GrabMove(Vector3 moveDirection, float delta)
        {
            rigidbody.velocity = moveDirection * grabSpeed;
        }


        public void Move(Vector3 moveDirection, float delta)
        {
            if (animator.GetBool("canRotate"))
            {
                moveDirection = Vector3.zero;
            }

            float speed = moveSpeed;
            if (isAiming)
                speed = aimSpeed;

            rigidbody.velocity = moveDirection * speed;
        }

        public void MoveProne(Vector3 moveDirection, float delta)
        {
            rigidbody.velocity = moveDirection * proneSpeed;
        }

        public void CrouchMovement(Vector3 moveDirection, float delta, float moveAmount)
        {
            float dot = Vector3.Dot(moveDirection, mTransform.forward);
            HandleMovementAnimations(moveAmount, delta);
            if (dot > 0)
            {
                Debug.DrawRay(mTransform.position, moveDirection);

                MoveProne(moveDirection, delta);

                if (moveAmount > 0)
                {
                    isProne = true;
                    HandleRotation(moveDirection, delta);
                    animator.SetBool("canRotate", false);
                }
            }
            else
            {
                if (moveAmount > 0)
                {
                    isProne = false;

                    if (animator.GetBool("canRotate"))
                    {
                        rigidbody.velocity = Vector3.zero;
                        HandleRotation(moveDirection, delta);
                    }
                }
            }
        }

        public void HandleRotation(Vector3 lookDir, float delta)
        {
            if (lookDir == Vector3.zero)
                lookDir = mTransform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            mTransform.rotation = Quaternion.Slerp(mTransform.rotation, lookRotation, delta / rotateSpeed);
        }

        public void FPSRotate(float horizontal, float delta)
        {
            Vector3 targetEuler = mTransform.eulerAngles;
            targetEuler.y += horizontal * delta / fpsRotateSpeed;
            mTransform.eulerAngles = targetEuler;
        }

        public void HandleAnimationStates()
        {
            animator.SetBool("isCrouch", isCrouch);
            animator.SetBool("isWall", isWall);
            animator.SetBool("isAiming", isAiming);
            animator.SetBool("isProne", isProne);

            if (inventoryManager.currentWeaponHook != null)
            {
                inventoryManager.currentWeaponHook.gameObject.SetActive(isAiming);
            }
            //inventoryManager.currentWeapon.model.SetActive(isAiming);
        }

        public void HandleMovementAnimations(float moveAmount, float delta)
        {
            float m = moveAmount;
            if (m > 0.1f && m < 0.51f)
                m = 0.5f;
            if (m > 0.51f)
                m = 1;
            if (m < 0.1f)
                m = 0;

            switch (controllerState)
            {
                case ControllerState.normal:
                    animator.SetFloat("movement", m, 0.1f, delta);

                    break;
                case ControllerState.cardboardBox:
                    boxAnimator.SetFloat("movement", m, 0.1f, delta);
                    break;
                case ControllerState.prone:
                    break;
                default:
                    break;
            }
        }

        float lastShot;

        public void HandleShooting()
        {
            if (Time.realtimeSinceStartup - lastShot > inventoryManager.currentWeapon.fireRate)
            {
                lastShot = Time.realtimeSinceStartup;
                inventoryManager.currentWeaponHook.Shoot();

                GameReferences.RaycastShoot(mTransform, inventoryManager.currentWeaponHook);
            }
        }

        public float grabOffset;
        public float grabDistance = 1;

        public void HandleGrab(bool isHolding, bool doublGrab, bool isTrigger)
        {
            if (currentGrabbed != null)
            {
                if (doublGrab && !isInteracting)
                {
                    animator.Play("p_grab_struggle");
                    currentGrabbed.animator.Play("e_grab_struggle");
                    currentGrabbed.timesStruggle++;

                    if (currentGrabbed.timesStruggle > 2)
                    {
                        isGrab = false;
                        animator.Play("p_grab_finish");
                        currentGrabbed.KillByGrab();
                        currentGrabbed = null;
                        isHolding = false;
                        return;
                    }
                }
            }

            if (isHolding)
            {
                if (currentGrabbed == null && isTrigger)
                {
                    Vector3 origin = mTransform.position;
                    origin.y += 1.5f;
                    RaycastHit hit;
                    Debug.DrawRay(origin, mTransform.forward * grabDistance, Color.blue, 1, false);
                    rigidbody.velocity = Vector3.zero;

                    if (Physics.SphereCast(origin, 0.25f, mTransform.forward, out hit, grabDistance))
                    {
                        AIController aIController = hit.transform.GetComponentInParent<AIController>();

                        if (aIController != null)
                        {
                            if (aIController.isDead == false)
                            {
                                Vector3 tp = mTransform.forward * grabOffset;
                                tp += mTransform.position;
                                aIController.StartGrab(tp, mTransform.rotation);
                                animator.Play("p_grab_start");
                                animator.SetFloat("movement", 0);
                                isGrab = true;
                                currentGrabbed = aIController;
                            }
                        }
                        else
                        {
                            animator.Play("p_grab_empty");
                        }
                    }
                    else
                    {
                        animator.Play("p_grab_empty");
                    }
                }
                else
                {

                }
            }
            else
            {
                if (currentGrabbed != null)
                {
                    isGrab = false;
                    animator.Play("p_grab_cancel");
                    //currentGrabbed.StopGrab(this);
                    currentGrabbed = null;
                }
            }
        }

        public void HandleEnemyPositionOnGrab()
        {
            Vector3 tp = mTransform.forward * grabOffset;
            tp += mTransform.position;
            currentGrabbed.transform.position = tp;
            currentGrabbed.transform.rotation = mTransform.rotation;
        }

        public void HandleGrabAnimation(float moveAmount, float delta)
        {
            float m = moveAmount;
            //if (moveAmount > 0)
            //{
            //	m = 1;
            //}

            animator.SetFloat("movement", m, 0.1f, delta);
            currentGrabbed.animator.SetFloat("movement", m, 0.1f, delta);
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

        public void UpdatePoseStats(PoseStats p)
        {
            controllerCollider.height = p.colliderHeight;
            Vector3 centerPosition = controllerCollider.center;
            centerPosition.y = p.colliderPosY;
            controllerCollider.center = centerPosition;
        }

        public bool OnDetect(AIController aIController)
        {
            //if (controllerState == ControllerState.cardboardBox)
            //{
            //    if (rigidbody.velocity.sqrMagnitude > 0.1f)
            //        aIController.OnDetectPlayer(this);
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    aIController.OnDetectPlayer(this);
            //}
            return true;
        }

        public Transform GetTransform()
        {
            return mTransform;
        }
    }

    [System.Serializable]
    public class PoseStats
    {
        public float colliderHeight = 2;
        public float colliderPosY = 1;
        public float wallDetectHeight;
    }
}
