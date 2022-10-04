using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FindTargetOfOverlapSphere))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public abstract class Monster<T1, T2> : MonoBehaviour, ITargetable, IDamable  where T2 : MonoBehaviour
{
    [SerializeField] protected GameObject targetedObject = null;
    [SerializeField] protected Transform bodyPoint = null;
    [SerializeField] protected int maxHP;
    protected int curHp;
    [SerializeField] protected float moveSpeed = 1f;
    [SerializeField] protected float rotSpeed = 20f;

    [SerializeField] protected Renderer[] myRenderer;
    protected PlayerController target;
    protected Animator animator;
    protected CharacterController controller;

    protected StateMachine<T1, T2> stateMachine;
    protected FindTargetOfOverlapSphere findTarget;
    public bool isDead { get; set; } = false;
    protected PlayerController player;
    protected bool isTargetDead = false;

    [SerializeField] protected AudioClip[] audios;
    protected AudioSource audioSource;

    public void SoundPlay(int tag)
    {
        audioSource.clip = audios[tag];
        audioSource.Play();
    }

    protected IEnumerator DissolveEnable(float timeScale = 1f)
    {
        float dissolveValue = 1f;

        while (dissolveValue > 0f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue -= Time.deltaTime * timeScale);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 0f);
        yield return null;
    }
    protected IEnumerator DissolveDisable(float timeScale = 1f)
    {
        //Invoke("DeadObject", 2f);
        float dissolveValue = 0f;

        while (dissolveValue < 1f)
        {
            for (int i = 0; i < myRenderer.Length; ++i)
                myRenderer[i].material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * timeScale);
            yield return null;
        }
        for (int i = 0; i < myRenderer.Length; ++i)
            myRenderer[i].material.SetFloat("_Dissolve", 1f);
        ObjectPooling.Instance.PushObject(this.gameObject);
        MonsterManager.Instance.activeMonsters.Remove(this.gameObject);
        yield return null;
    }

    public void DissolveDisableStart()
    {
        StartCoroutine(DissolveDisable());
    }

    public void DeadObject()
    {
        ObjectPooling.Instance.PushObject(this.gameObject);
        MonsterManager.Instance.activeMonsters.Remove(this.gameObject);
    }

    public void ChangeState(T1 nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    protected void FindTarget()
    {
        if (isTargetDead)
            return;

        Collider[] colls = null;

        if (!findTarget.FindTarget(ref colls))
        {
            target = null;
            return;
        }

        foreach(var coll in colls)
        {
            int rayerNumber = coll.gameObject.layer;

            if (rayerNumber == LayerMask.NameToLayer("Player"))
            {
                target = player;
                break;
            }
            else if(rayerNumber == LayerMask.NameToLayer("Monster"))
            {
                target = player;

                if(target)
                    break;
                continue;
            }
            
        }

        
    }

    public void NonTarget()
    {
        if (isDead) return;

        targetedObject?.SetActive(false);
    }

    public void OnTarget()
    {
        if (isDead) return;
        targetedObject?.SetActive(true);
    }

    public virtual void TakeDamage(int damage, Transform transform = null)
    {
        maxHP -= damage;
    }
    public abstract void HitEffect(Vector3 position, Quaternion rotaiton);

    public void Initialize()
    {
        targetedObject?.SetActive(false);
        findTarget = GetComponent<FindTargetOfOverlapSphere>();
        findTarget.Init();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();

        myRenderer = GetComponentsInChildren<Renderer>();
        player.deadEvent += () =>
        {
            isTargetDead = true;
            target = null;
        };

        player.deadReturnEvent += () =>
        {
            isTargetDead = false;
            target = player;
        };
    }

    public bool IsTarget()
    {
        return !isDead;
    }
}
