using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float speed = 5f;
    bool isClouseImp = false;

    private void Awake()
    {
        img.fillAmount = 0f;
    }

    IEnumerator EnableCo()
    {
        Time.timeScale = 0f;
        while (img.fillAmount < 1f)
        {
            img.fillAmount += Time.unscaledDeltaTime * speed;
            yield return null;
        }
        isClouseImp = true;
    }

    private void Update()
    {
        if (isClouseImp && Input.GetMouseButtonDown(0))
            StartCoroutine(DisableCo());
    }

    IEnumerator DisableCo()
    {
        text.text = "";
        while (img.fillAmount > 0f)
        {
            img.fillAmount -= Time.unscaledDeltaTime * speed;
            yield return null;
        }
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    public void SetText(string str)
    {
        text.text = str;
    }
    
    private void OnEnable()
    {
        img.fillAmount = 0f;
        StartCoroutine(EnableCo());
        isClouseImp = false;
    }

}
