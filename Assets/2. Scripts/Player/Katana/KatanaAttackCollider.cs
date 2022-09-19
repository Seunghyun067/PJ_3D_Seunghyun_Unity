using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttackCollider : MonoBehaviour
{
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
        if (other.gameObject.layer == LayerMask.NameToLayer("Parrying"))
        {
            player.parringAction?.Invoke();
            return;
        }
        IDamable damagable = other.gameObject.GetComponent<IDamable>();
        
        if (null == damagable)
            return;
        
        damagable.TakeDamage(5);
        //StartCoroutine(GameManager.Instance.TimeSleepCoroutine(0.1f, 0.1f));
        myCollider.enabled = false;

    }

    
}
