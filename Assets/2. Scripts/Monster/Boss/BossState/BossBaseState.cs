using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : StateMachineBehaviour
{
    protected Transform trans;
    protected CharacterController controller;
    protected Boss boss;
    private bool isInit = false;

    public void Initialize(Animator animator)
    {
        if (isInit) return;

        trans = animator.GetComponent<Transform>();
        controller = animator.GetComponent<CharacterController>();
        boss = animator.GetComponent<Boss>();
        isInit = true;
    }
}
