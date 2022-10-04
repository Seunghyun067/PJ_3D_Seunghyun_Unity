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
        player.katana.KatanaTrailActive(true);
        player.katana.AttackColliderActive(true);
        player.SoundPlay(PlayerController.AudioTag.SWORD);
    }
    void SwordTrailOff()
    {
        player.katana.KatanaTrailActive(false);
        player.katana.AttackColliderActive(false);
    }
}
