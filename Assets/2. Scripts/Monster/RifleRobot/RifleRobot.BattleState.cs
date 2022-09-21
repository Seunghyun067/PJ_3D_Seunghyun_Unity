using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RifleRobot : Monster<RifleRobotState, RifleRobot>
{
    private class TraceState : BaseState
    {

        public override void OnStateEnter(RifleRobot owner)
        {
            
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
                
                owner.animator.SetFloat("MoveWeight", owner.moveWeight = Mathf.Lerp(owner.moveWeight, 2f, Time.deltaTime * 3f));
                owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed * owner.moveWeight);

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
            owner.animator.SetBool("Attack", true);
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            float attackTime = 0.5f;
            float attackDelay = 2f;
            while (true)
            {
                if (attackTime < 0f)
                {
                    Debug.Log("Shoot");
                    owner.ShootLaser();
                    attackTime = attackDelay;
                }

                attackTime -= Time.deltaTime;

                float dist = Vector3.Distance(owner.target.transform.position, owner.transform.position);
                float moveWeight = 0f;
                if (dist > 11f)
                {
                    owner.ChangeState(RifleRobotState.TRACE);
                    yield break;
                }

                Vector3 moveDir = (owner.target.transform.position - owner.transform.position).normalized;
                moveDir.y = 0;
                Vector3 lookDir = moveDir;
                owner.transform.localRotation = Quaternion.Slerp(owner.transform.localRotation, Quaternion.LookRotation(moveDir), owner.rotSpeed * Time.deltaTime);

                if (dist >= 5f && dist <= 6f)
                    moveWeight = 0f;
                else if (dist < 5f)
                {
                    moveWeight = 1f;
                    moveDir *= -1;
                }
                else if (dist > 6f)
                    moveWeight = 1f;

                owner.animator.SetFloat("MoveWeight", owner.moveWeight = Mathf.Lerp(owner.moveWeight, moveWeight, Time.deltaTime * 3f));
                owner.controller.Move(moveDir * Time.deltaTime * owner.moveSpeed * owner.moveWeight);

                yield return null;
            }
        }
        public override void OnStateExit(RifleRobot owner)
        {
            owner.animator.SetBool("Attack", false);
        }
    }
    private class HitState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
            owner.StopAllCoroutines();
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