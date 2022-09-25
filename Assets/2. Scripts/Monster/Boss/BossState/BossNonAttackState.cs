using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNonAttackState : BossBaseState
{
    [SerializeField] private float attackDelayTime = 5f;
    private float curDelayTime = 0f;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialize(animator);
        curDelayTime = attackDelayTime;
}
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = Vector3.Distance(boss.target.transform.position, boss.transform.position);
        Debug.Log(dist);

        if (dist >= 15f)
            return;

        if (dist <= 11f)
        {
            animator.SetTrigger("NearAttack");
        }

        curDelayTime -= Time.deltaTime;
        if (curDelayTime < 0f)
        {
            animator.SetTrigger("Attack");
            curDelayTime = attackDelayTime;
        }

    }
}
