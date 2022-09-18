using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollState : PlayerStateBase
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        animator.applyRootMotion = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }


}
