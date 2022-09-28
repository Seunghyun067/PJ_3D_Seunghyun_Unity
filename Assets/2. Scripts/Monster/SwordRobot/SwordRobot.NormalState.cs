using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SwordRobot : Monster<SwordRobotState, SwordRobot>
{
    private abstract class BaseState : State<SwordRobot> {}

    private class IdleState : BaseState
    {
        public override void OnStateEnter(SwordRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(SwordRobot owner)
        {
            while (true)
            {
                owner.FindTarget();

                if (owner.target)
                {
                    owner.ChangeState(SwordRobotState.TRACE);
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }           
        }
        public override void OnStateExit(SwordRobot owner)
        {
        }        
    }

    

    
}
