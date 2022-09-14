using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int comboAttackCount { get; set; } = 0;

    [SerializeField] public TrailRenderer katanaTrail;
    [SerializeField] public Transform camTransform;

    [SerializeField] public float moveSpeed { get; }= 5;
    [SerializeField] public float rotSpeed { get; } = 5;

    void Start()
    {
        if (katanaTrail)
            katanaTrail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
