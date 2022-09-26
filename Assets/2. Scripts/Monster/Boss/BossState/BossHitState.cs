using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitState : BossBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        boss.AttackColliderActive(false);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.AttackColliderActive(true);
    }
}
