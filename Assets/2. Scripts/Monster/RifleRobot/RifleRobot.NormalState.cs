using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class RifleRobot : Monster<RifleRobotState, RifleRobot>
{
    private abstract class BaseState : State<RifleRobot> {}

    private class IdleState : BaseState
    {
        public override void OnStateEnter(RifleRobot owner)
        {
        }
        public override IEnumerator OnStateUpdate(RifleRobot owner)
        {
            while (true)
            {
                owner.FindTarget();

                if (owner.target)
                {
                    owner.ChangeState(RifleRobotState.TRACE);
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }           
        }
        public override void OnStateExit(RifleRobot owner)
        {

        }        
    }

    

    
}
