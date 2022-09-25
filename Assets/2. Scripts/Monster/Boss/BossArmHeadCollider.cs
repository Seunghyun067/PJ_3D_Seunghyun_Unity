using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossArmHeadCollider : MonoBehaviour
{
    public UnityAction<BossArmHeadCollider> deadEvent;
    Renderer[] myRenderer;
    Collider[] myCollider;

    private void OnTriggerEnter(Collider other)
    {
        string bloodTag = "Blood" + UnityEngine.Random.Range(1, 4).ToString();
        
        ObjectPooling.Instance.PopObject(bloodTag, other.bounds.center);
        StartCoroutine(De());
    }

    private void Awake()
    {
        myRenderer = GetComponentsInChildren<Renderer>();
        myCollider = GetComponentsInChildren<Collider>();
        foreach (var renderer in myRenderer)
            renderer.material.SetFloat("_NoiseScale", 585f);
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
        myCollider[index].enabled = false;

        if (index + 1 == myRenderer.Length)
        {
            deadEvent?.Invoke(this);
            gameObject.SetActive(false);
            Debug.Log("NonActive");
        }

        yield return null;
    }
}
