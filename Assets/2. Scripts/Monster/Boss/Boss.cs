using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Boss : MonoBehaviour
{
    public StartScene scene;
    private Animator animator;
    private List<BossArmHeadCollider> bossArms = null;
    [SerializeField] private List<Renderer> bodyRenderer = null;

    [HideInInspector]  public PlayerController target;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        bossArms = new List<BossArmHeadCollider>(GetComponentsInChildren<BossArmHeadCollider>());
        target = FindObjectOfType<PlayerController>();
        foreach (var arm in bossArms) arm.deadEvent += DeadArm;

        foreach (var renderer in bodyRenderer)
            renderer.material.SetFloat("_NoiseScale", 50f);
    }

    public void AttackColliderActive(bool isActive)
    {
        foreach (var arm in bossArms) arm.CollidersTriggerOn(!isActive);
    }

   

    public void DeadArm(BossArmHeadCollider deadArm)
    {
        bossArms.Remove(deadArm);

        if (bossArms.Count == 0)
        {

            animator.SetTrigger("Dead");
            StartCoroutine(BossDeadCo());
        }
    }

    IEnumerator BossDeadCo()
    {
        float dissolveValue = 0f;

        while (dissolveValue < 1f)
        {
            foreach (var renderer in bodyRenderer)
                renderer.material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * 0.05f);

          
            yield return null;
        }

        foreach (var renderer in bodyRenderer)
            renderer.material.SetFloat("_Dissolve", 1f);
   
        yield return null;
    }
}

