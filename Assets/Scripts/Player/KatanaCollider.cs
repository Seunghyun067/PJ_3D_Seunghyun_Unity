using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollider : MonoBehaviour
{
    private PlayerController player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        if (player)
            Debug.Log(player.attackDamage.ToString() + "�÷��̾� ������Ʈ�� ã�ҽ����𤿤� ");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<Monster>())
            return;

        Debug.Log(player.attackDamage.ToString() + "��ŭ�� ������ ���� ");
        StartCoroutine(GameManager.Instance.TimeSleepCoroutine(0, 0.2f));
    }

    
}
