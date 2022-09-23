using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Vector3 savePos;
    Vector3 pos;
    bool isCut = false;
    

    private void Awake()
    {
        pos = transform.localPosition;
    }
    private float curL = 0f;
    private void Update()
    {
        pos = transform.position;
        if (isCut)
        {
            pos.y -= 1f * Time.deltaTime;
            transform.position = pos;
        }
        Debug.Log(transform.localPosition);
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.SetParent(null);
            isCut = true;
        }
    }
}
