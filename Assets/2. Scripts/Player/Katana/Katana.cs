using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private Collider attackCollider;
    [SerializeField] private Collider parryingCollider;

    private void Awake()
    {
        trail.enabled = false;
        attackCollider.enabled = false;
        parryingCollider.enabled = false;
        
    }

    

    public void KatanaTrailActive(bool isActive)
    {
        trail.enabled = isActive;
    }
    public void AttackColliderActive(bool isActive)
    {
        attackCollider.enabled = isActive;
    }
    public void ParryingColliderActive(bool isActive)
    {
        parryingCollider.enabled = isActive;
    }
}
