using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurnState : BossBaseState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 dirToTarget = (boss.target.transform.position - boss.transform.position);
        dirToTarget = dirToTarget.normalized;

        float dist = Vector3.Distance(boss.target.transform.position, boss.transform.position);

        Vector3 moveDir = (boss.target.transform.position - boss.transform.position).normalized;
        moveDir.y = 0;

        if (dist >= 15f)
            animator.rootPosition += moveDir * Time.deltaTime * 2f;

        if (Vector3.Dot(boss.transform.forward, dirToTarget) > Mathf.Cos(15f * 0.5f * Mathf.Deg2Rad))
        {
            animator.SetBool("TurnLeft", false);
        }
        animator.rootRotation = Quaternion.Slerp(boss.transform.localRotation, Quaternion.LookRotation(moveDir), Time.deltaTime);
    }
}
