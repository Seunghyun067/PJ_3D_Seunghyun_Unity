using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class AngleroxColliderEvent : MonoBehaviour
{
    public string collName;
    public UnityAction<Collider, Collider> triggerEnterEvent;

    public Collider myCollider;

    public void Init()
    {
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEnterEvent?.Invoke(other, myCollider);
        
    }
}
