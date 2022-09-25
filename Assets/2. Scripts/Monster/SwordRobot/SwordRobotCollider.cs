using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRobotCollider : MonoBehaviour
{
    SwordRobot robot;
    private void Awake()
    {
        robot = GetComponentInParent<SwordRobot>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        other.gameObject.GetComponent<IDamable>().TakeDamage(5, robot.transform);
        other.GetComponent<PlayerController>().HitTrigger("Hit");
    }
}
