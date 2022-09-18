using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackState : PlayerStateBase
{
    
    [SerializeField] private float normalizeTime = 0.5f;
    [SerializeField] private string animName;

    private bool isCombo = false;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.Initialize(animator);

        animator.SetBool("ComboAttack", isCombo = false);
        animator.applyRootMotion = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 lookVec;

        

        if(player.TargetTransform)
        {
            lookVec = player.TargetTransform.transform.position - player.transform.position;
            lookVec.y = 0;
            lookVec = lookVec.normalized;
        }
        else
            lookVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;



        animator.rootRotation = Quaternion.Slerp(player.transform.localRotation, Quaternion.LookRotation(lookVec), player.rotSpeed * Time.deltaTime);

        if (!isCombo && Input.GetButtonDown("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= normalizeTime)
            animator.SetBool("ComboAttack", isCombo = true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
