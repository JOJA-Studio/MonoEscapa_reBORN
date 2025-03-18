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
        public float rotateSpeed = .2f;
        [HideInInspector]
        public Transform mtransform;
        Animator animator;

        private void Start()
        {
            mtransform = this.transform;
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
        }

        public void Wallmovement(Vector3 moveDirection, Vector3 normal, float delta)
        {
            Vector3 projectvel = Vector3.ProjectOnPlane(moveDirection, normal);

            rigidbody.velocity = projectvel * moveSpeed; 
            HandleMovementAnimations(0, delta);

            HandleRotation(-normal, delta);
        }

        public void Move(Vector3 moveDirection, float delta)
        {
            rigidbody.velocity = moveDirection * moveSpeed;
            HandleRotation(moveDirection, delta);
        }

        void HandleRotation(Vector3 lookDir, float delta)
        {
            if (lookDir == Vector3.zero)
                lookDir = mtransform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            mtransform.rotation = Quaternion.Slerp(mtransform.rotation, lookRotation, delta / rotateSpeed);
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
