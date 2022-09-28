using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : PlayerStateBase
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
        if (GameManager.Instance.IsKeyHold)
            return;

        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        animator.SetFloat("MoveSpeed", moveInput.magnitude);
                
        Vector3 forwardVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;
        Vector3 rightVec = new Vector3(player.camTransform.right.x, 0f, player.camTransform.right.z).normalized;
        
        Vector3 moveVec = (moveInput.x * rightVec * player.moveSpeed + Vector3.up * 0f + moveInput.z * forwardVec * player.moveSpeed);
        moveVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized * player.moveSpeed + Vector3.up * 0f;
        
        Vector3 lookVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized;
        
        if (lookVec.magnitude > 0)
            animator.rootRotation = Quaternion.Slerp(player.transform.localRotation, Quaternion.LookRotation(lookVec), player.rotSpeed * Time.deltaTime);

        
        if (Input.GetButtonDown("Attack"))
        {
            animator.SetTrigger("Attack");
            animator.SetBool("Run", false);
        }


        if (!isRoll && Input.GetButtonDown("Roll"))
        {
            animator.SetTrigger("Roll");
            isRoll = true;
        }


        if (Input.GetButtonDown("Parry"))
        {
            animator.SetBool("Parry", true);
            animator.SetTrigger("ParryT");
        }

        if (moveInput.magnitude >= 0.99f && Input.GetButton("Run"))
        {
            animator.SetBool("Run", true);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
