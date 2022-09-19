using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryAttackState : PlayerStateBase
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.Initialize(animator);
        animator.applyRootMotion = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Vector3 lookVec;
        if (player.TargetTransform)
        {
            lookVec = player.TargetTransform.transform.position - player.transform.position;
            lookVec.y = 0;
            lookVec = lookVec.normalized;
        }
        else
            lookVec = new Vector3(player.camTransform.forward.x, 0f, player.camTransform.forward.z).normalized;

        animator.rootRotation = Quaternion.Slerp(player.transform.localRotation, Quaternion.LookRotation(lookVec), player.rotSpeed * 2 *  Time.deltaTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.katana.KatanaTrailActive(false);
        player.katana.AttackColliderActive(false);
    }
}
