using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();

        ObjectPooling.Instance.PopObject(bloodTag, other.bounds.center);
    }

}
