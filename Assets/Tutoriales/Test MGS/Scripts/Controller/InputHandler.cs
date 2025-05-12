using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SA
{
    public class InputHandler : MonoBehaviour
    {
        public ExecutionOrder movementOrder;
        public Controller controller;

        public CameraManager cameraManager;
        Vector3 moveDirection;
        public float wallDetectDis = .5f;
        public float wallDetectDisOnWall = 1.2f;
        public float wallAngleThreshold = 35;

        Vector2 moveInputDirection;
        Vector2 lookInputDirection;
        float moveAmount;
        bool freeLook;
        bool grabInput;
        bool rawGrabInputDown;
        bool switchWeapon;
        bool crouchInput;
        float grabDeadTimer;
        bool isFPSinit;
        LayerMask ignoreForWall;

        PlayerControls inputActions;
        public Transform wallCameraTarget;

        public Transform mTransform
        {
            get
            {
                return controller.mTransform;
            }
        }

        public enum ExecutionOrder
        {
            fixedUpdate, update, lateUpdate
        }

        private void Start()
        {
            inputActions = new PlayerControls();
            inputActions.Player.Movement.performed += i => moveInputDirection = i.ReadValue<Vector2>();
            inputActions.Player.FreeLookDirection.performed += i => lookInputDirection = i.ReadValue<Vector2>();
            inputActions.Player.Crouch.started += i => crouchInput = true;
            inputActions.Player.Grab.started += i => rawGrabInputDown = true;
            inputActions.Player.Freelook.started += i => cameraManager.tiltAngle = 0;

            inputActions.Enable();
            cameraManager.Init();

            cameraManager.wallCameraObject.SetActive(false);
            cameraManager.mainCameraObject.SetActive(true);
            cameraManager.fpsCameraObject.SetActive(false);
            cameraManager.mainCamera.cullingMask = ~0;

            ignoreForWall = ~(1 << 11 | 1 << 14 | 1 << 15);
            GameReferences.ignoreForShooting = ~(1 << 14 | 1 << 15);
            GameReferences.controllersLayer = (1 << 11);


            UIManager.singleton.Init(controller.inventoryManager);

            List<IICon> l = new List<IICon>();
            l.AddRange(ResourcesManager.singleton.GetAllItems());
            IconMaker.RequestIconForList(l, UpdateUIManagerWithItems);

            DontDestroyOnLoad(this.gameObject);
        }

        void UpdateUIManagerWithItems()
        {
            List<Item> l = new List<Item>();
            l.AddRange(ResourcesManager.singleton.GetAllItems());
            UIManager.singleton.CreateSlotsForItemList(l);
        }

        private void OnEnable()
        {
            if (inputActions != null)
                inputActions.Enable();

            moveInputDirection = Vector2.zero;
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        private void FixedUpdate()
        {
            if (movementOrder == ExecutionOrder.fixedUpdate)
            {
                HandleMovement(moveDirection, Time.fixedDeltaTime);
            }
        }

        bool IsPressed(UnityEngine.InputSystem.InputActionPhase phase)
        {
            return phase == UnityEngine.InputSystem.InputActionPhase.Started;
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            bool isInventory = UIManager.singleton.Tick(moveInputDirection.y, delta, IsPressed(inputActions.Player.LeftBumper.phase),
                false);//IsPressed(inputActions.Player.RightBumper.phase));

            if (isInventory)
                return;

            controller.isAiming = IsPressed(inputActions.Player.Aim.phase);
            freeLook = IsPressed(inputActions.Player.Freelook.phase);
            bool rawGrabInputHold = IsPressed(inputActions.Player.Grab.phase);
            bool doubleGrab = false;

            if (rawGrabInputHold)
            {
                grabInput = true;
                if (grabDeadTimer > 0)
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

            if (switchWeapon)
            {
                controller.inventoryManager.SwitchWeapon();
            }

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
                    controller.rigidbody.velocity = Vector3.zero;
                    cameraManager.mainCamera.cullingMask = ~(1 << 12);

                }
            }
            else
            {
                if (controller.isFreelook)
                {
                    controller.isFreelook = false;
                    cameraManager.fpsCameraObject.SetActive(false);
                    cameraManager.mainCamera.cullingMask = ~0;
                }
            }

            if (controller.isInteracting)
            {
                controller.rigidbody.velocity = Vector3.zero;
                return;
            }

            controller.HandleGrab(grabInput, doubleGrab, rawGrabInputDown);

            if (controller.isFPS)
            {
                if (!isFPSinit)
                {
                    cameraManager.fpsCameraObject.SetActive(true);
                    cameraManager.mainCamera.cullingMask = ~(1 << 10);

                    isFPSinit = true;
                }

                moveDirection = controller.mTransform.forward * moveInputDirection.y;
                moveDirection += controller.mTransform.right * moveInputDirection.x;
                moveDirection.Normalize();
                controller.FPSRotate(lookInputDirection.x, delta);
                controller.MoveProne(moveDirection, delta);

                return;
            }

            if (isFPSinit)
            {
                cameraManager.fpsCameraObject.SetActive(false);
                cameraManager.mainCamera.cullingMask = ~0;
                isFPSinit = false;
            }

            moveAmount = Mathf.Clamp01(Mathf.Abs(moveInputDirection.x) + Mathf.Abs(moveInputDirection.y));
            moveDirection = Vector3.forward * moveInputDirection.y;
            moveDirection += Vector3.right * moveInputDirection.x;
            moveDirection.Normalize();

            if (crouchInput)
            {
                controller.isCrouch = !controller.isCrouch;

                if (controller.isCrouch)
                {
                    controller.UpdatePoseStats(controller.crouching);
                }
                else
                {
                    controller.UpdatePoseStats(controller.standing);
                }

                if (!controller.isWall)
                    moveDirection = Vector3.zero;
            }



            if (controller.isFreelook)
            {
                controller.FPSRotate(lookInputDirection.x, delta);
                cameraManager.HandleFPSTilt(lookInputDirection.y, delta);
            }
            else
            {
                if (controller.inventoryManager.currentWeaponHook == null)
                {
                    controller.isAiming = false;
                }

                if (controller.isAiming)
                {
                    //controller.isWall = false;
                    controller.isCrouch = false;
                    controller.HandleRotation(moveDirection, delta);

                    if (IsPressed(inputActions.Player.Shoot.phase))
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

        private void LateUpdate()
        {
            ResetInput();
        }

        void ResetInput()
        {
            rawGrabInputDown = false;
            crouchInput = false;
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
            origin.y += controller.getWallDetectOrigin;


            bool willStickToWall = false;
            Vector3 wallNormal = Vector3.zero;

            float detectDis = wallDetectDis;
            if (controller.isWall)
            {
                detectDis = wallDetectDisOnWall;
            }

            Debug.DrawRay(origin, moveDirection * detectDis);

            if (Physics.SphereCast(origin, 0.25f, moveDirection, out RaycastHit hit, detectDis, ignoreForWall))
            {
                willStickToWall = true;
                wallNormal = hit.normal;
            }

            if (willStickToWall)
            {
                wallCameraTarget.transform.position = controller.transform.position;
                wallCameraTarget.transform.rotation = Quaternion.LookRotation(wallNormal);

                controller.isProne = false;
                controller.isWall = true;
                controller.WallMovement(moveDirection, wallNormal, delta, ignoreForWall);
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
