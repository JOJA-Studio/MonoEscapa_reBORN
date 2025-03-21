using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace SA
{ 
    public class Controller : MonoBehaviour
    {
        new Rigidbody rigidbody;
        public float moveSpeed = .4f;
        public float proneSpeed = .4f;
        public float wallSpeed = .4f;
        public float rotateSpeed = .2f;
        public float wallCheckDis = .2f;
        [HideInInspector]
        public Transform mtransform;
        Animator animator;

        public bool isWall;
        public bool isCrouch;
        public bool isProne;


        private void Start()
        {
            mtransform = this.transform;
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
        }

        public void Wallmovement(Vector3 moveDirection, Vector3 normal, float delta, LayerMask layermask)
        {
            //float dot = Vector3.Dot(moveDirection, Vector3.forward);
            //Debug.Log(dot);
            //moveDirection *= (dot < -0.8f) ? -1 : 1;

            Vector3 projectvel = Vector3.ProjectOnPlane(moveDirection, normal);
            Debug.DrawRay(mtransform.position, projectvel,Color.blue);
            Vector3 relativeDir = mtransform.InverseTransformDirection(projectvel);


            Vector3 origin = mtransform.position;
            origin.y += 1;

            if ((Mathf.Abs(relativeDir.x) > 0.01f))
            {
                if (relativeDir.x > 0)
                    origin += mtransform.right * wallCheckDis;
                if(relativeDir.x < 0)
                    origin -= mtransform.right * wallCheckDis;


                Debug.DrawRay(origin, -normal, Color.red);
                if (Physics.Raycast(origin, -normal, out RaycastHit hit, 2, layermask))
                {

                }
                else
                {
                    projectvel = Vector3.zero;
                    relativeDir.x = 0;
                }
            }
            else
            {
                projectvel = Vector3.zero;
                relativeDir.x = 0;
            }

            rigidbody.velocity = projectvel * wallSpeed; 
            HandleRotation(-normal, delta);

            float m = 0;
            Debug.Log(relativeDir);
            m = relativeDir.x;
            if (m < 0.1f && m > 0.1f)
            {
                m = 0;
            }
            else
            {
                m = (m < 0) ? -1 : 1;
            }
            animator.SetFloat("movement", m, 0.1f, delta);
        }

        public void Move(Vector3 moveDirection, float delta)
        {

            rigidbody.velocity = moveDirection * moveSpeed;            
        }

        public void CrouchMovement(Vector3 moveDirection, float delta, float moveAmount)
        {
            float dot = Vector3.Dot(moveDirection, mtransform.forward);
            HandleMovementAnimations(moveAmount, delta);
            if (dot > 0)
            {
                Debug.DrawRay(mtransform.position, moveDirection);
                rigidbody.velocity = moveDirection * proneSpeed;


                if (moveAmount > 0)
                { 
                    animator.SetBool("isProne", true);
                    HandleRotation(moveDirection, delta);
                    animator.SetBool("canRotate", false);
                }
            }
            else
            {
                if (moveAmount > 0)
                { 
                    animator.SetBool("isProne", false);

                    if (animator.GetBool("canRotate"))
                    { 
                         HandleRotation(moveDirection, delta);                    
                    }
                }
            }
        }

        public void HandleRotation(Vector3 lookDir, float delta)
        {
            if (lookDir == Vector3.zero)
                lookDir = mtransform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            mtransform.rotation = Quaternion.Slerp(mtransform.rotation, lookRotation, delta / rotateSpeed);
        }

        public void HandleAnimationStates()
        {
            animator.SetBool("isCrouch", isCrouch);
            animator.SetBool("isWall", isWall);
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

           

            animator.SetFloat("movement", m, 0.1f, delta);
        }
    }
}
