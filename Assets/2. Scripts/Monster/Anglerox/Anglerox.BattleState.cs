using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Anglerox : Monster<AngleroxState, Anglerox>
{    
    private class TraceState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
            owner.animator.SetBool("Trace", true);
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            while (true)
            {
                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                moveDir.y = 0;
                //owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed);
                float dist = Vector3.Distance(owner.target.transform.position, owner.transform.position);
                if (dist <= 4.5f && dist >= 4f)
                {
                    owner.ChangeState(AngleroxState.JUMP_ATTACK);
                    yield break;
                }

                else if (dist <= 2.0f)
                {
                    owner.ChangeState(AngleroxState.ATTACK);
                    yield break;
                }

                yield return null;
            }
           
        }
        public override void OnStateExit(Anglerox owner)
        {

        }
    }
    private class AttackState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
            int rand = Random.Range(1, 3);
            owner.animator.SetInteger("RandomAttack", rand);
            owner.animator.SetTrigger("Attack");

            if(rand == 1)
                owner.colliders["AttackL"].triggerEnterEvent = owner.AttackColliderEvent;
            else
                owner.colliders["AttackR"].triggerEnterEvent = owner.AttackColliderEvent;
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            float attackTime = 1f;

            while (attackTime > 0f)
            {
                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;

                Vector3 dirToTarget = (owner.target.transform.position - owner.transform.position);

                if (Vector3.Dot(owner.transform.forward, dirToTarget) > Mathf.Cos(10f * 0.5f * Mathf.Deg2Rad))
                    owner.animator.SetBool("AttackWalk", false);
                else
                {
                    owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime * 0.5f);
                    owner.animator.SetBool("AttackWalk", true);
                    Debug.Log("Turn");
                }
                moveDir.y = 0;

                attackTime -= Time.deltaTime;
                yield return null;
            }

            owner.ChangeState(AngleroxState.TRACE);
        }
        public override void OnStateExit(Anglerox owner)
        {
        }
    }
    private class HitState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            yield return new WaitForSeconds(1f);
            owner.ChangeState(AngleroxState.TRACE);
        }
        public override void OnStateExit(Anglerox owner)
        {

        }
    }

    private class JumpAttackState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
            owner.colliders["AttackR"].triggerEnterEvent = owner.StrongAttackColliderEvent;
            owner.animator.SetTrigger("JumpAttack");
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            float attackTime = 1f;

            while (attackTime > 0f)
            {
                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                attackTime -= Time.deltaTime;
                yield return null;
            }
            owner.ChangeState(AngleroxState.TRACE);
        }
        public override void OnStateExit(Anglerox owner)
        {

        }
    }

    private class DieState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            owner.DeadEffect();
            yield return new WaitForSeconds(1f);
        }
        public override void OnStateExit(Anglerox owner)
        {
            
        }
    }
}
