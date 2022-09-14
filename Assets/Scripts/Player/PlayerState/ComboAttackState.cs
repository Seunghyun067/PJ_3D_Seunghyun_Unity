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
        Debug.Log(animName + " Enter");

        if (player.katanaTrail)
            player.katanaTrail.enabled = true;
        animator.SetBool("ComboAttack", isCombo = false);
        animator.applyRootMotion = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isCombo && Input.GetButtonDown("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= normalizeTime)
            animator.SetBool("ComboAttack", isCombo = true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player.katanaTrail)
            player.katanaTrail.enabled = animator.applyRootMotion = isCombo;
        Debug.Log(animName + "Exit");
    }
}
