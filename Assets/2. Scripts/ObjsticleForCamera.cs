using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjsticleForCamera : MonoBehaviour
{    
    [SerializeField] Material changeMtr;
    [SerializeField] Renderer myRenderer;
    private Material mainMtr;

    private void Awake()
    {
        if (myRenderer == null)
            myRenderer = GetComponentInChildren<Renderer>();
        mainMtr = myRenderer.material;
    }

    public void Hit()
    {
        myRenderer.material = changeMtr;
        
    }

    public void NonHit()
    {
        myRenderer.material = mainMtr;
    }


}
