using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwordRobot : Monster<SwordRobotState, SwordRobot>
{
    public override void HitEffect(Vector3 position, Quaternion rotaiton)
    {
        var hitEffect = ObjectPooling.Instance.PopObject("SparksCore", position, rotaiton);
    }

    private class TraceState : BaseState
    {
        public override void OnStateEnter(SwordRobot owner)
        {
            owner.animator.SetBool("Trace", true);
        }
        public override IEnumerator OnStateUpdate(SwordRobot owner)
        {
            while (true)
            {
                if (!owner.target)
                {
                    owner.animator.SetBool("Trace", false);
                    owner.ChangeState(SwordRobotState.IDLE);
                    yield break;
                }

                if (Vector3.Distance(owner.target.transform.position, owner.transform.position) <= 1.25f)
                {
                    owner.ChangeState(SwordRobotState.ATTACK);
                    yield break;
                }

                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                moveDir.y = 0;
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed);

                

                yield return null;
            }
           
        }
        public override void OnStateExit(SwordRobot owner)
        {

        }
    }
    private class AttackState : BaseState
    {
        public override void OnStateEnter(SwordRobot owner)
        {
            owner.animator.SetInteger("RandomAttack", Random.Range(1, 3));
            owner.animator.SetTrigger("Attack");
        }
        public override IEnumerator OnStateUpdate(SwordRobot owner)
        {
            yield return new WaitForSeconds(1f);
            owner.ChangeState(SwordRobotState.TRACE);
        }
        public override void OnStateExit(SwordRobot owner)
        {
        }
    }
    private class HitState : BaseState
    {
        public override void OnStateEnter(SwordRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(SwordRobot owner)
        {
            Vector3 moveDir = -(owner.target.transform.position - owner.transform.position).normalized;
            moveDir.y = 0;
            float hitTime = 1f;
            while (hitTime > 0f)
            {
                hitTime -= Time.deltaTime;

                if (hitTime > 0.7f)
                    owner.controller.Move(moveDir * Time.deltaTime * 3f);

                yield return null;
            }
            owner.ChangeState(SwordRobotState.TRACE);
        }
        public override void OnStateExit(SwordRobot owner)
        {

        }
    }
    private class DieState : BaseState
    {
        public override void OnStateEnter(SwordRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(SwordRobot owner)
        {
            owner.DeadEffect();
            yield return new WaitForSeconds(1f);
        }
        public override void OnStateExit(SwordRobot owner)
        {
            
        }
    }
}
