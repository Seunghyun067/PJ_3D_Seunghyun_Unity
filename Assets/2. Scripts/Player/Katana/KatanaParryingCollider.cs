using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaParryingCollider : MonoBehaviour
{
    private PlayerController player;
    private Collider myCollider;
    [SerializeField] private GameObject Effect;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<PlayerController>();
        myCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Parrying"))
            return;


        Instantiate(Effect, transform.position, Quaternion.identity);
        GameManager.Instance.TimeSleep(0.1f, 0.5f);
        player.parringAction?.Invoke();
        other.enabled = false;
        myCollider.enabled = false;

    }


}
