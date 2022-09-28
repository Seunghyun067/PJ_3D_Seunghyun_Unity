using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerStateBase
{
    bool isRoll = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        animator.applyRootMotion = true;
        player.katana.ParryingColliderActive(false);
        animator.SetBool("Run", false);
        isRoll = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isRoll && Input.GetButtonDown("Roll"))
        {
            Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            Vector3 forwardVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;
            Vector3 rightVec = new Vector3(player.camTransform.right.x, 0f, player.camTransform.right.z).normalized;
            Vector3 lookVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized;

            if (lookVec.magnitude > 0)
                animator.rootRotation = Quaternion.LookRotation(lookVec);
            animator.SetTrigger("Roll");
            isRoll = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
