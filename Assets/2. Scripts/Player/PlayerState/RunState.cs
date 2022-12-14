using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerStateBase
{
    bool isAttack = false;
    bool isRoll = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        animator.applyRootMotion = true;
        isAttack = false;
        isRoll = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        animator.SetFloat("MoveSpeed", moveInput.magnitude);
        Vector3 forwardVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;
        Vector3 rightVec = new Vector3(player.camTransform.right.x, 0f, player.camTransform.right.z).normalized;

        Vector3 moveVec = (moveInput.x * rightVec * player.moveSpeed + Vector3.up * 0f + moveInput.z * forwardVec * player.moveSpeed);
        moveVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized * player.moveSpeed + Vector3.up * 0f;

        Vector3 lookVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized;

        if (lookVec.magnitude > 0)
            animator.rootRotation = Quaternion.Slerp(player.transform.localRotation, Quaternion.LookRotation(lookVec), player.rotSpeed * Time.deltaTime);


        if (!isAttack && Input.GetButtonDown("Attack"))
        {
            isAttack = true;
            animator.SetTrigger("RunAttack");
        }

        if (!isRoll && Input.GetButtonDown("Roll"))
        {
            animator.SetTrigger("Roll");
            isRoll = true;
        }

        if (Input.GetButtonUp("Run"))
        {
            animator.SetBool("Run", false);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Run", false);
    }
}
