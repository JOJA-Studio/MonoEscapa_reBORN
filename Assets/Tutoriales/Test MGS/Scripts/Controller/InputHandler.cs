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

        float horizontal;
        float vertical;
        float moveAmount;

    Vector3 moveDirection;
        public float wallDetectDistance = .5f;

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

            controller.HandleMovementAnimations(moveAmount, delta);
        }

        void HandleMovement(Vector3 moveDirection, float delta)
        {
            Vector3 origin = controller.transform.position;
            origin.y += 1;
            Debug.DrawRay(origin, moveDirection * wallDetectDistance);
            if (Physics.Raycast(origin, moveDirection, out RaycastHit hit, wallDetectDistance))
            {
                cameraManager.wallCameraObject.SetActive(true);
                cameraManager.mainCameraObject.SetActive(false);

                controller.Wallmovement(moveDirection, hit.normal, delta);
                //Debug.Log("Chocando con pared");
                //if()
            }
            else
            {
                cameraManager.wallCameraObject.SetActive(false);
                cameraManager.mainCameraObject.SetActive(true);
                controller.Move(moveDirection, delta);            
            }
            
        }
    }

}
