using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private LineRenderer render;

    private void Awake()
    {
        render = GetComponent<LineRenderer>();
    }
    private float curL = 0f;
    private void Update()
    {
        Vector3 dir = Vector3.forward;
        render.SetPosition(1, dir * curL);
        curL += Time.deltaTime;
    }
}
