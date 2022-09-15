using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    
    private PlayerController player;
    private void Awake()
    {
        player = GetComponent<PlayerController>();
    }

    public void SwordTrailOn()
    {
        player.katanaTrail.enabled = true;
        player.katanaTrail2.enabled = true;
        player.katanaCollider.enabled = true;
    }
    void SwordTrailOff()
    {
        player.katanaTrail.enabled = false;
        player.katanaTrail2.enabled = false;
        player.katanaCollider.enabled = false;
    }
}
