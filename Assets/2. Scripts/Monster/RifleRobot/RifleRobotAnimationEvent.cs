using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleRobotAnimationEvent : MonoBehaviour
{
    private RifleRobot owner;

    private void Awake()
    {
        owner = GetComponentInParent<RifleRobot>();
    }
    public void AttackStart()
    {
        owner.AttackColliderActive(true);
    }

    public void AttackEnd()
    {
        owner.AttackColliderActive(false);
    }
}
