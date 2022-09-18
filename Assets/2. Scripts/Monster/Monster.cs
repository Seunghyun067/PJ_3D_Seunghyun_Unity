using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour, ITargetable
{
    [SerializeField] private GameObject target;

    public void NonTarget()
    {
        target.SetActive(false);
    }

    public void OnTarget()
    {
        target.SetActive(true);
    }

    void Start()
    {
        target.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
