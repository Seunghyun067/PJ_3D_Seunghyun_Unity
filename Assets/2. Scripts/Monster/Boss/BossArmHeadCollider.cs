using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossArmHeadCollider : MonoBehaviour, ITargetable, IDamable
{
    [SerializeField] protected GameObject targetedObject = null;

    private Boss boss;
    public UnityAction<BossArmHeadCollider> deadEvent;
    Renderer[] myRenderer;
    Collider[] myColliders;
    private int hp = 20;
    private bool isDead;

    public void Set()
    {
        isDead = false;
        hp = 20;

        foreach (var renderer in myRenderer)
            renderer.material.SetFloat("_Dissolve", 0f);
        gameObject.SetActive(true);
    }

    public void CollidersTriggerOn(bool isTrigger)
    {
        foreach (var coll in myColliders)
            coll.isTrigger = isTrigger;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        collision.gameObject.GetComponent<IDamable>().TakeDamage(5, transform);
        collision.gameObject.GetComponent<PlayerController>().HitTrigger("HitDown");
    }
  

    private void Awake()
    {
        targetedObject.transform.SetParent(null);
        myRenderer = GetComponentsInChildren<Renderer>();
        myColliders = GetComponentsInChildren<Collider>();
        foreach (var renderer in myRenderer)
            renderer.material.SetFloat("_NoiseScale", 585f);
        targetedObject.transform.SetParent(gameObject.transform);
        targetedObject.SetActive(false);
        boss = transform.GetComponentInParent<Boss>();
    }

    IEnumerator De(int index = 0)
    {
        
        float dissolveValue = 0f;
        bool isNext = true;

        while (dissolveValue < 1f)
        {
            myRenderer[index].material.SetFloat("_Dissolve", dissolveValue += Time.deltaTime * 2f);

            if (isNext && dissolveValue > 0.1f && myRenderer.Length > index + 1)
            {
                isNext = false;
                StartCoroutine(De(index + 1));
            }
            yield return null;
        }
        myRenderer[index].material.SetFloat("_Dissolve", 1f);
        myColliders[index].enabled = false;

        if (index + 1 == myRenderer.Length)
        {
            deadEvent?.Invoke(this);
            gameObject.SetActive(false);
            Debug.Log("NonActive");
        }

        yield return null;
    }

    public bool IsTarget()
    {
        return !isDead;
    }

    public void OnTarget()
    {
        if (isDead) return;

        targetedObject?.SetActive(true);
    }

    public void NonTarget()
    {
        if (isDead) return;

        targetedObject?.SetActive(false);
    }

    public void TakeDamage(int damage, Transform transform = null)
    {
        hp -= damage;
        boss.GetComponent<Boss>().SoundPlay(Boss.AudioTag.HIT);
        if (hp > 0)
            return;
        boss.GetComponent<Boss>().SoundPlay(Boss.AudioTag.ARM_DIE);
        targetedObject?.SetActive(false);
        isDead = true;
        StartCoroutine(De());
        Debug.Log("트리거 엔터");
    }

    public void HitEffect(Vector3 position, Quaternion rotation)
    {
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();

        ObjectPooling.Instance.PopObject(bloodTag, position, rotation);
    }
}
