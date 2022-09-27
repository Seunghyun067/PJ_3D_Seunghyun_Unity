using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Image img;
    [SerializeField] private Image imgBack;
    [SerializeField] private float lerpSpeed;

    private float curFillAmount = 1f;
    private float backFillAmount = 1f;

    private void Awake()
    {
        CurFillAmount = BackFillAmount = 1f;
        player.TransHpEvent += PlayerHpEvent;        
    }

    public void PlayerHpEvent(int hp, int maxHp)
    {
        CurFillAmount = (float)hp / maxHp;
    }
    public float CurFillAmount 
    { 
        get { return curFillAmount; } 
        set
        {
            img.fillAmount = value;
            curFillAmount = img.fillAmount;
        }
    }
    public float BackFillAmount
    {
        get { return backFillAmount; }
        set
        {
            imgBack.fillAmount = value;
            backFillAmount = imgBack.fillAmount;
        }
    }
    void Update()
    {

        if (curFillAmount >= backFillAmount)
        {
            backFillAmount = curFillAmount;
            return;
        }
            
        BackFillAmount -= Time.deltaTime * lerpSpeed;

        if (curFillAmount > backFillAmount)
            BackFillAmount = curFillAmount;
    }
}
