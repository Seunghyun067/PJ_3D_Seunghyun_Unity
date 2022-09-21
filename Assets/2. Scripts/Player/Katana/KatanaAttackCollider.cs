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
        if (other.gameObject.layer == LayerMask.NameToLayer("Parrying"))
        {
            player.parringAction?.Invoke();
            return;
        }
        IDamable damagable = other.gameObject.GetComponent<IDamable>();

        
        
        if (null == damagable)
            return;

        Vector3 pos = other.transform.position;
        pos.y += 1f;

        Vector3 rot = (bottom.position - top.position).normalized;
        rot.y = 1;
        Quaternion q =  Quaternion.LookRotation(rot);

        var hitEffect = ObjectPooling.Instance.PopObject("SparksCore");
        hitEffect.transform.position = pos;
        hitEffect.transform.rotation = q;

        //hitEffect.SetActive(true);

        damagable.TakeDamage(5);
        myCollider.enabled = false;

    }

    
}
