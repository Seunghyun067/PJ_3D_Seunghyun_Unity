using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttackCollider : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;

    [SerializeField] private Transform top;
    [SerializeField] private Transform bottom;

    private PlayerController player;
    private Collider myCollider;

    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            return;
        IDamable damagable = other.gameObject.GetComponentInParent<IDamable>();

        
        
        if (null == damagable)
            return;
        

        Vector3 myPos = player.transform.position;
        myPos.y = 0;
        Vector3 targetPos = other.gameObject.transform.root.position;
        targetPos.y = 0;
        Vector3 hitNormal = (myPos - targetPos).normalized;


        damagable.TakeDamage(5);
        damagable.HitEffect(other.bounds.center, Quaternion.LookRotation(hitNormal));
        myCollider.enabled = false;

    }

    
}
