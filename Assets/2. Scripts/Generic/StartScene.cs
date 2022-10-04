using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class StartScene : MonoBehaviour
{
    [SerializeField] GameObject[] terrains;
    [SerializeField] GameObject player;
    [SerializeField] Boss boss;
    [SerializeField] GameObject aircraft;
    [SerializeField] CinemachineVirtualCamera topview;
    [SerializeField] CinemachineFreeLook freeLook;
    [SerializeField] GameObject angleroxGroup;
    [SerializeField] List<Anglerox> angleroxs = new List<Anglerox>();

    [SerializeField] ConversationInformation converInfo;
    [SerializeField] ConversationController conver;
    [SerializeField] Image backGround;
    [SerializeField] GameObject deadUI;
    bool isBoss = false;

    void Start()
    {
        CameraManager.Instance.SetMainCM();
        aircraft.transform.position = Vector3.zero;
        GameManager.Instance.SetPlayer(player.GetComponent<PlayerController>());
        UIManager.Instance.SetUI(conver, backGround, deadUI);
    }

    public void ReturnSavePoint()
    {
        Debug.Log("´Ù½ÃÇØ");
        MonsterManager.Instance.ResetActiveMonsters();
        TimelineManager.Instance.ReturnSavePoint();
        UIManager.Instance.DeadUIHide();
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<PlayerController>().DeadReturn();
        MonsterManager.Instance.StopAllCoroutines();
        UIManager.Instance.BackGroundNone();
        AngleSet();
        if(angCo != null)
        StopCoroutine(angCo);

        if (isBoss)
            boss.BossSet();
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

    public void StartConver()
    {
        UIManager.Instance.ConversationStart(converInfo);
    }


    public void TerrainHide()
    {
        foreach (var obj in terrains)
            obj.SetActive(false);
    }
    public void AngStart()
    {
        foreach (var ang in angleroxGroup.GetComponentsInChildren<Anglerox>())
            ang.DissolveDisableStart();
        Invoke("AngleStart", 3f);
    }
    Coroutine angCo;
    public void AngleStart()
    {
        angCo = StartCoroutine(AngleroxStage());
    }

    public void AngleSet()
    {
        foreach (var ang in angleroxs)
        {
            ObjectPooling.Instance.PushObject(ang.gameObject);
        }
    }

    public void BossStateStart()
    {
        boss.isStart = true;
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    IEnumerator AngleroxStage()
    {
        int angCount = 0;
        float delayTime = 6f;
        while (true)
        {
            if (GameManager.Instance.player.IsDead)
                yield break;

            if (angCount != 8)
            {
                angleroxs.Add(ObjectPooling.Instance.PopObject("Anglerox", player.transform.position + -player.transform.forward * 3f).GetComponent<Anglerox>());
                ++angCount;

                if (angCount == 3)
                    delayTime = 2f;

                yield return new WaitForSeconds(delayTime);
                continue;
            }
            else
            {
                foreach (var ang in angleroxs)
                {
                  ang.RunAwayStart();
                    //TimelineManager.Instance.PlayTimeline("BossStart");
                }

                Invoke("BossStart", 3f);
                yield break;
            }

        }
    }
    public void BossStart()
    {
        TimelineManager.Instance.PlayTimeline("BossStart");
        isBoss = true;
    }
}
