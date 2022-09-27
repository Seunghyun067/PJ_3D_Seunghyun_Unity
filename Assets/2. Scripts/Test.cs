using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] GameObject target;
    private void Update()
    {
        Vector3 look = target.transform.position;
        look.y = transform.position.y;
        transform.LookAt(look);
    }

}
