using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{ 
    public class InputHandler : MonoBehaviour
    {
        public Transform camHolder;

        public ExecutionOrder movementOrder;
        public Controller controller;
        public CameraManager cameraManager;

        Vector3 moveDirection;
        public float wallDetectDistance = .5f;
        public float wallDetectDistanceOnWall = 1.2f;
        public float wallAngleTreshold = 35;

        float horizontal;
        float vertical;
        float moveAmount;
        bool freeLook;
        bool grabInput;
        float grabDeadTimer;
        LayerMask ignoreForWall;
        


        public enum ExecutionOrder { 
            fixedUpdate, update, lateUpdate
        }

        private void Start()
        {
            cameraManager.wallCameraObject.SetActive(false);
            cameraManager.mainCameraObject.SetActive(true);
            cameraManager.fpsCameraObject.SetActive(false);
            cameraManager.mainCamera.cullingMask = ~0;
            ignoreForWall = ~(1 << 11);
        }

        private void FixedUpdate()
        {
            if (movementOrder == ExecutionOrder.fixedUpdate)
            {
                HandleMovement(moveDirection, Time.fixedDeltaTime);
            }  
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            controller.isAiming = Input.GetMouseButton(1);
            freeLook = Input.GetKey(KeyCode.F);
            bool rawGrabInputHold = Input.GetMouseButton(0);
            bool rawGrabInputDown = Input.GetMouseButtonDown(0);
            bool doubleGrab = false;

            if (rawGrabInputHold)
            {
                grabInput = true;
                if (grabDeadTimer < 1)
                { 
                    doubleGrab = true;
                }
                grabDeadTimer = 0;

            }
            else
            {
                grabDeadTimer += delta;
                if (grabDeadTimer > 1)
                { 
                    grabInput = false;
                }
            }

            controller.isInteracting = controller.animator.GetBool("isInteracting");
            if (controller.isAiming)
            {
                grabInput = false;
                freeLook = false;
            }
            if (grabInput)
            { 
                freeLook = false;
            }

            if (freeLook)
            {
                if (controller.isFreelook == false)
                {
                    controller.isFreelook = true;
                    controller.isAiming = false;
                    controller.isProne = false;
                    cameraManager.fpsCameraObject.SetActive(true);
                    //controller.meshRenderer.enabled = false;
                    controller.rigidbody.velocity = Vector3.zero;
                    cameraManager.mainCamera.cullingMask = ~(1 << 12 | 1 << 14);
                }

            }
            else
            {
                if (controller.isFreelook)
                {
                    controller.isFreelook = false;
                    cameraManager.fpsCameraObject.SetActive(false);
                    //controller.meshRenderer.enabled = true;
                    cameraManager.mainCamera.cullingMask = ~0;
                }
            }
            if (controller.isInteracting)
            {
                controller.rigidbody.velocity = Vector3.zero;
            }

            controller.HandlerGrab(grabInput, doubleGrab, rawGrabInputDown);

            

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            moveDirection = Vector3.forward * vertical;
            moveDirection += Vector3.right * horizontal;
            moveDirection.Normalize();

            if (Input.GetKeyDown(KeyCode.C))
            {
                controller.isCrouch = !controller.isCrouch;
                if (!controller.isWall)
                    moveDirection = Vector3.zero;
            }


            if (controller.isFreelook)
            {
                controller.FPSRotate(horizontal, delta);
            }
            else
            {
                if (controller.isAiming)
                {
                    //controller.isWall = false;
                    controller.isCrouch = false;
                    controller.HandleRotation(moveDirection, delta);

                    if (Input.GetMouseButton(0))
                    {
                        controller.HandleShooting();
                    }

                    if (controller.inventoryManager.currentWeapon.canMoveWithWeapon)
                    {
                        controller.Move(moveDirection, delta);
                        controller.HandleMovementAnimations(moveAmount, delta);
                    }
                    else
                    {
                        controller.HandleMovementAnimations(0, delta);
                        controller.rigidbody.velocity = Vector3.zero;
                    }
                }
                else
                {
                    if (movementOrder == ExecutionOrder.update)
                    {
                        HandleMovement(moveDirection, delta);
                    }
                }
            }

            controller.HandleAnimationStates();
        }


        void HandleMovement(Vector3 moveDirection, float delta)
        {

            if (controller.isGrab)
            {
                controller.HandleGrabAnimation(moveAmount, delta);

                if (moveAmount == 1)
                {
                    controller.GrabMove(moveDirection, delta);
                    controller.HandleRotation(-moveDirection, delta);
                }

                controller.HandleEnemyPositionOnGrab();
                return;
            }


            Vector3 origin = controller.transform.position;
            origin.y += 1;


            Debug.DrawRay(origin, moveDirection * wallDetectDistance);

            bool willStickToWall = false;
            Vector3 wallNormal = Vector3.zero;

            float detectDis = wallDetectDistance;
            if (controller.isWall)
            {
                detectDis = wallDetectDistanceOnWall; 
            }

            Debug.DrawRay(origin, moveDirection * detectDis);

            if (Physics.SphereCast(origin, 0.25f, moveDirection, out RaycastHit hit, wallDetectDistance, ignoreForWall))
            {
                    willStickToWall = true;
                    wallNormal = hit.normal;
                //    Debug.Log(hit.transform.name);
            }
            
            if (willStickToWall)
            {
                controller.isProne = false; 
                controller.isWall = true;
                controller.Wallmovement(moveDirection, wallNormal, delta, ignoreForWall);
                cameraManager.wallCameraObject.SetActive(true);
                cameraManager.mainCameraObject.SetActive(false);
            }
            else
            {

                controller.isWall = false;
                cameraManager.wallCameraObject.SetActive(false);
                cameraManager.mainCameraObject.SetActive(true);

                if (controller.isCrouch)
                {
                    controller.CrouchMovement(moveDirection, delta, moveAmount);
                }
                else
                {
                    controller.Move(moveDirection, delta);
                    controller.HandleRotation(moveDirection, delta);
                    controller.HandleMovementAnimations(moveAmount, delta); 
                }
            }            
        }
    }

}
