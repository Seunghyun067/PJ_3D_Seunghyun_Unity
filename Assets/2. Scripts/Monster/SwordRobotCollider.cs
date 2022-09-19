using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordRobotCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        other.gameObject.GetComponent<IDamable>().TakeDamage(5);
    }
}
