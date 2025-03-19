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

        float horizontal;
        float vertical;
        float moveAmount;

        public enum ExecutionOrder { 
            fixedUpdate, update, lateUpdate
        }

        private void Start()
        {
            cameraManager.wallCameraObject.SetActive(false);
            cameraManager.mainCameraObject.SetActive(true);
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
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            moveDirection = camHolder.forward * vertical;
            moveDirection += camHolder.right * horizontal;
            moveDirection.Normalize();

            float delta = Time.deltaTime;

            if (movementOrder == ExecutionOrder.update)
            {
                HandleMovement(moveDirection, delta);
            }

            controller.HandleAnimationStates();
        }

        void HandleMovement(Vector3 moveDirection, float delta)
        {
            Vector3 origin = controller.transform.position;
            origin.y += 1;

            Debug.DrawRay(origin, moveDirection * wallDetectDistance);
            if (Physics.SphereCast(origin, 0.25f, moveDirection, out RaycastHit hit, wallDetectDistance))
            {
                cameraManager.wallCameraObject.SetActive(true);
                cameraManager.mainCameraObject.SetActive(false);

                controller.isWall = true;
                controller.Wallmovement(moveDirection, hit.normal, delta);
            }
            else
            {
                controller.isWall = false;

                cameraManager.wallCameraObject.SetActive(false);
                cameraManager.mainCameraObject.SetActive(true);
                controller.Move(moveDirection, delta);
                controller.HandleMovementAnimations(moveAmount, delta);
            }
            
        }
    }

}
