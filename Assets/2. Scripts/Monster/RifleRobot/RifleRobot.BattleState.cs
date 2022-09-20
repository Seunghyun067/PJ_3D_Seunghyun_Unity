using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RifleRobot : Monster<RifleRobotState, RifleRobot>
{
    private class TraceState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
            owner.animator.SetBool("Trace", true);
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            while (true)
            {

                if (Vector3.Distance(owner.target.transform.position, owner.transform.position) <= 10f)
                {
                    owner.ChangeState(RifleRobotState.ATTACK);
                    yield break;
                }

                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                moveDir.y = 0;
                owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed);                

                yield return null;
            }
           
        }
        public override void OnStateExit(RifleRobot owner)
        {

        }
    }
    private class AttackState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
            
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            float attackTime = 0f;
            float attackDelay = 2f;
            while (true)
            {
                if (attackTime < 0f)
                {
                    owner.animator.SetTrigger("Attack");
                    attackTime = attackDelay;
                }

                attackTime -= Time.deltaTime;

                float dist = Vector3.Distance(owner.target.transform.position, owner.transform.position);

                if (dist > 11f)
                {
                    owner.ChangeState(RifleRobotState.TRACE);
                    yield break;
                }

                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                Vector3 lookDir = moveDir;                

                if (dist >= 5f && dist <= 6f)
                {
                    moveDir = Vector3.zero;
                    owner.animator.SetBool("Trace", false);
                }
                else if (dist < 5f)
                {
                    owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                    moveDir.y = 0;
                    owner.controller.Move(-moveDir * Time.deltaTime * owner.moveSpeed);
                    owner.animator.SetBool("Trace", true);
                }
                else if(dist > 6f)
                {
                    owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);
                    moveDir.y = 0;
                    owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed);
                    owner.animator.SetBool("Trace", true);
                }

                yield return null;
            }
        }
        public override void OnStateExit(RifleRobot owner)
        {
        }
    }
    private class HitState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            Vector3 moveDir = -(owner.target.transform.position - owner.transform.position).normalized;
            moveDir.y = 0;
            float hitTime = 1f;
            while (hitTime > 0f)
            {
                hitTime -= Time.deltaTime;

                if (hitTime > 0.7f)
                    owner.controller.Move(moveDir * Time.deltaTime * 1f);

                yield return null;
            }
            owner.ChangeState(RifleRobotState.TRACE);
        }
        public override void OnStateExit(RifleRobot owner)
        {

        }
    }
    private class DieState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            owner.DeadEffect();
            yield return new WaitForSeconds(1f);
        }
        public override void OnStateExit(RifleRobot owner)
        {
            
        }
    }
}
