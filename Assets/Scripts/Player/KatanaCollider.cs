using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollider : MonoBehaviour
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
        if (!other.GetComponent<Monster>())
            return;

        Debug.Log(player.attackDamage.ToString() + "만큼의 데미지 입힘 ");
        StartCoroutine(GameManager.Instance.TimeSleepCoroutine(0, 0.2f));
        myCollider.enabled = false;

    }

    
}
