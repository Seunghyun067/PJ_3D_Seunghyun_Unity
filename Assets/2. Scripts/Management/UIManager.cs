using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private ToolTip tooltip;
    [SerializeField] private ConversationController conver;
    [SerializeField] private Image backGround;

    [SerializeField] private GameObject deadUI;

    public void SetUI(ConversationController conver, Image backGround, GameObject deadUI)
    {
        this.conver = conver;
        this.backGround = backGround;
        this.deadUI = deadUI;

        deadUI.SetActive(false);
        GameManager.Instance.player.deadEvent += () => deadUI.SetActive(true);

    }

    private void Start()
    {
        tooltip?.gameObject.SetActive(false);
        deadUI.SetActive(false);
        GameManager.Instance.player.deadEvent += () => deadUI.SetActive(true);
    }

    public void DeadUIHide()
    {
        deadUI.SetActive(false);
    }

    public void BackGroundBlack()
    {
        Color backColor = backGround.color;
        backColor.a = 1f;
        backGround.color = backColor;
    }
    public void BackGroundNone()
    {
        Color backColor = backGround.color;
        backColor.a = 0f;
        backGround.color = backColor;
    }

    public void ConversationStart(ConversationInformation conver, UnityAction endConverEvent = null)
    {
        this.conver.StartConversation(conver, endConverEvent);
        
    }

    public void TooltipOn(string str)
    {
        Debug.Log("ToolTip ON");
        tooltip.gameObject.SetActive(true);
        //tooltip.SetText(str);
    }
}
