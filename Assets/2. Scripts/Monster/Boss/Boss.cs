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

    private int curArm = 6;

    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;
    public bool isStart = false;

    public enum AudioTag { ROAR, NEAR, NORMAL, HIT, ARM_DIE }

    public void SoundPlay(AudioTag tag)
    {
        audioSource.clip = audios[(int)tag];
        audioSource.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        bossArms = new List<BossArmHeadCollider>(GetComponentsInChildren<BossArmHeadCollider>());
        target = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        foreach (var arm in bossArms) arm.deadEvent += DeadArm;

        foreach (var renderer in bodyRenderer)
            renderer.material.SetFloat("_NoiseScale", 50f);
    }

    public void BossSet()
    {
        foreach (var arm in bossArms)
        {
            arm.gameObject.SetActive(true);
            arm.Set();
        }
    }

    public void AttackColliderActive(bool isActive)
    {
        foreach (var arm in bossArms) arm.CollidersTriggerOn(!isActive);
    }

   

    public void DeadArm(BossArmHeadCollider deadArm)
    {
        --curArm;
        
        if (curArm == 0)
        {

            animator.SetTrigger("Dead");
            StartCoroutine(BossDeadCo());
        }
    }

    IEnumerator BossDeadCo()
    {
        SoundPlay(AudioTag.ROAR);
        float dissolveValue = 0f;

        while (dissolveValue < 1f)
        {
            foreach (var renderer in bodyRenderer)
                renderer.material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * 0.05f);

          
            yield return null;
        }

        foreach (var renderer in bodyRenderer)
            renderer.material.SetFloat("_Dissolve", 1f);

        
        TimelineManager.Instance.PlayTimeline("Stage2End");
        TimelineManager.Instance.StartHold();
        gameObject.SetActive(false);
   
        yield return null;
    }
}

