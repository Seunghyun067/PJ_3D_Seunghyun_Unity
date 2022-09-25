using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Anglerox : Monster<AngleroxState, Anglerox>
{
    private abstract class BaseState : State<Anglerox> {}

    private class IdleState : BaseState
    {
        public override void OnStateEnter(Anglerox owner)
        {
        }
        public override IEnumerator OnStateUpdate(Anglerox owner)
        {
            while (true)
            {
                owner.FindTarget();

                if (owner.target)
                {
                    owner.ChangeState(AngleroxState.TRACE);
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }           
        }
        public override void OnStateExit(Anglerox owner)
        {

        }        
    }

    

    
}
