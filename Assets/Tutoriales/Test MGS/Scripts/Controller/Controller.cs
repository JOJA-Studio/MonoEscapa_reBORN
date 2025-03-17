using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{ 
    public class Controller : MonoBehaviour
    {
        new Rigidbody rigidbody;
        public float moveSpeed = .4f;
        public float rotateSpeed = .2f;
        Transform mtransform;
        Animator animator;

        private void Start()
        {
            mtransform = this.transform;
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
        }

        public void Move(Vector3 moveDirection, float delta)
        {
            rigidbody.velocity = moveDirection * moveSpeed;

            Vector3 lookDir = moveDirection;
            if(lookDir == Vector3.zero)
                lookDir = mtransform.forward;
            Quaternion lookRotation = Quaternion.LookRotation(moveDirection);   
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
