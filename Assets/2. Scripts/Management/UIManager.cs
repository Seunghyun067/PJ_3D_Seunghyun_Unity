using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private ToolTip tooltip;

    private void Awake()
    {
        tooltip.gameObject.SetActive(false);
    }

    public void TooltipOn(string str)
    {

        tooltip.gameObject.SetActive(true);

    }
}
