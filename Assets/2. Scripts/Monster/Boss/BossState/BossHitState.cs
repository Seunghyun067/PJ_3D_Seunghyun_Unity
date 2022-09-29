using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitState : BossBaseState
{

    bool isFreeLook = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        boss.AttackColliderActive(false);

        

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") + " " + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        if (!isFreeLook && animator.GetCurrentAnimatorStateInfo(0).IsName("Hit") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            boss.scene.FreeView();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.AttackColliderActive(true);
        boss.scene.TopView();
    }
}
