using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameAudio { AIR_GO, EXPLOSION }

    [SerializeField] public PlayerController player;
    [SerializeField] private ConversationInformation[] conver;
    [SerializeField] private bool isTest = true;
    [SerializeField] private AudioClip[] audios;
    private AudioSource audioSource;



    // Start is called before the first frame update
    public bool IsKeyHold { get; set; } = false;
    private int curConverIndex = 0;

    public void TimeSleep(float scale, float time)
    {
        StartCoroutine(TimeSleepCoroutine(scale, time));
    }

    public void SetPlayer(PlayerController p)
    {
        player = p;
        player.deadEvent += () => Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator TimeSleepCoroutine(float scale, float time)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        player.deadEvent += () => Cursor.lockState = CursorLockMode.None;
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (isTest)
            return;
        Time.timeScale = 0f;
        UIManager.Instance.ConversationStart(conver[curConverIndex++], () => StartCoroutine(ShootDown()));
        UIManager.Instance.BackGroundBlack();
        //var obj = ObjectPooling.Instance.PopObject("SwordRobot");
        //obj.transform.position = new Vector3(1f, 0f, 5f);
    }

    public void PlayFailed()
    {
        player.PlayerKill();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    IEnumerator ShootDown()
    {
        audioSource.clip = audios[(int)GameAudio.AIR_GO];
        audioSource.Play();

        yield return new WaitForSecondsRealtime(3f);
        audioSource.clip = audios[(int)GameAudio.EXPLOSION];
        audioSource.Play();
        yield return new WaitForSecondsRealtime(1f);
        UIManager.Instance.ConversationStart(conver[curConverIndex], () =>
        {
            Debug.Log("마지막 대화가 끝남");
            Time.timeScale = 1f;
            TimelineManager.Instance.PlayTimeline("GameStart");
        });
    }

    public void ReturnSavePoint()
    {
        Debug.Log("다시해");
        MonsterManager.Instance.ResetActiveMonsters();
        TimelineManager.Instance.ReturnSavePoint();
        UIManager.Instance.DeadUIHide();
        Cursor.lockState = CursorLockMode.Locked;
        player.DeadReturn();
        MonsterManager.Instance.StopAllCoroutines();
        UIManager.Instance.BackGroundNone();

    }

    public void AircraftOn()
    {
        if (MonsterManager.Instance.activeMonsters.Count == 0)
            TimelineManager.Instance.PlayTimeline("CityEnd");
        else
            TimelineManager.Instance.PlayTimeline("CityFailed");
    }
}