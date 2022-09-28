using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public UnityEvent triggerEvent;
    [SerializeField] bool TriggerDead = false;
    [SerializeField] LayerMask layermask;

    private void OnTriggerEnter(Collider other)
    {
        if ((layermask & 1 << other.gameObject.layer) == 0)
            return;

        triggerEvent.Invoke();

        if (TriggerDead)
            Destroy(gameObject);
    }

}
