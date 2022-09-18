using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Coroutine distorCo;
    Renderer myRenderer;

    private void Awake()
    {

        myRenderer = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (distorCo == null)
                distorCo = StartCoroutine(DistortionCo());
            else
            {
                StopCoroutine(distorCo);
                distorCo = StartCoroutine(DistortionCo());
            }
        }

    }

    IEnumerator DistortionCo()
    {
        float v = 0.5f;
        while (v > 0f)
        {
            myRenderer.material.SetFloat("_DistortionValue", v -= 0.05f);
            Debug.Log(v);
            yield return new WaitForSeconds(0.01f);
        }

        yield return null;
    }
}
