using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerStateBase
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        animator.applyRootMotion = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        //
        //float spd = moveSpeed;
        //if (Input.GetButton("Walk"))
        //{
        //    moveRate -= Time.deltaTime;
        //    if (moveRate < 0.3f)
        //        moveRate = 0.3f;
        //
        //    moveInput *= moveRate;
        //    spd *= moveRate;
        //}
        //else
        //{
        //    moveRate += Time.deltaTime;
        //    if (moveRate > 1f)
        //        moveRate = 1f;
        //
        //    moveInput *= moveRate;
        //    spd *= moveRate;
        //}
        //
        //animator.SetFloat("verSpeed", moveInput.z);
        //animator.SetFloat("horSpeed", moveInput.x);
        animator.SetFloat("MoveSpeed", moveInput.magnitude);
        //animator.SetBool("isMove", isMove = moveInput.magnitude > 0.1f);
        //
        Vector3 forwardVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;
        Vector3 rightVec = new Vector3(player.camTransform.right.x, 0f, player.camTransform.right.z).normalized;
        
        Vector3 moveVec = (moveInput.x * rightVec * player.moveSpeed + Vector3.up * 0f + moveInput.z * forwardVec * player.moveSpeed);
        moveVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized * player.moveSpeed + Vector3.up * 0f;
        
        //if (!isMove) return;
        Vector3 lookVec = (moveInput.x * rightVec + moveInput.z * forwardVec).normalized;
        
        if (lookVec.magnitude > 0)
            player.transform.localRotation = Quaternion.Slerp(player.transform.localRotation, Quaternion.LookRotation(lookVec), player.rotSpeed * Time.deltaTime);

        controller.Move(moveVec * Time.deltaTime);

        if (Input.GetButtonDown("Attack"))
            animator.SetTrigger("Attack");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
