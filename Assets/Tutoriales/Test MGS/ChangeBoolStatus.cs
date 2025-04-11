using System.Collections;
using System.Collections.Generic;
using SA;
using UnityEngine;

public class ChangeBoolStatus : StateMachineBehaviour
{
    public string boolName;
    public bool status;
    public bool resetOnExit;
    public float delay = .1f;
    Controller controller;
    AIController aiController;
    public bool isPlayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isPlayer)
        { 
            if (controller == null)
            controller = animator.GetComponentInParent<Controller>();

            if (delay > 0)
            {
                controller.StartCoroutine(DeleyedOpen(delay, animator, status));
            }
            else
            {
                animator.SetBool(boolName, status);
            }
        }
        else
        {
            if (aiController == null)
                aiController = animator.GetComponentInParent<AIController>();

            if (delay > 0)
            {
                Debug.Log("Controller: " + (controller != null ? "No nulo" : "Nulo") + " game object " + animator.gameObject.name);
                aiController.StartCoroutine(DeleyedOpen(delay, animator, status));
            }
            else
            {
                animator.SetBool(boolName, status);
            }
        }
    }

    IEnumerator DeleyedOpen(float d, Animator animator, bool targetStatus)
    { 
        yield return new WaitForSeconds(d);
        //animator.SetBool(boolName, targetStatus);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (resetOnExit)
        {
            //animator.SetBool(boolName, !status);
            //if (controller == null)
            //    controller = animator.GetComponentInParent<Controller>();

            //controller.StartCoroutine(DeleyedOpen(delay, animator, !status));
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
