using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimationEvent : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    public void AttackStart()
    {
        attackCollider.enabled = true;
    }

    public void AttackEnd()
    {
        attackCollider.enabled = false;
    }
}
