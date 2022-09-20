using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationEvent : MonoBehaviour
{
    [SerializeField] private SwordRobot owner;

    private void Awake()
    {
        owner = GetComponentInParent<SwordRobot>();
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
