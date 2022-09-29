using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class StartScene : MonoBehaviour
{
    [SerializeField] GameObject[] terrains;
    [SerializeField] GameObject player;
    [SerializeField] GameObject aircraft;
    [SerializeField] CinemachineVirtualCamera topview;
    [SerializeField] CinemachineFreeLook freeLook;
    void Start()
    {
        CameraManager.Instance.SetMainCM();
        aircraft.transform.position = Vector3.zero;
    }

    public void TopView()
    {
        freeLook.enabled = false;
        topview.enabled = true;
    }

    public void FreeView()
    {
        freeLook.enabled = true;
        topview.enabled = false;
    }

    public void BossStart()
    {
        TimelineManager.Instance.PlayTimeline("BossStart");
    }


    public void TerrainHide()
    {
        foreach (var obj in terrains)
            obj.SetActive(false);
    }
}
