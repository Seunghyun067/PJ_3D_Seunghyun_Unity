using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    public bool IsKeyHold { get; set; } = false;

    public void TimeSleep(float scale, float time)
    {
        StartCoroutine(TimeSleepCoroutine(scale, time));
    }

    IEnumerator TimeSleepCoroutine(float scale, float time)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //var obj = ObjectPooling.Instance.PopObject("SwordRobot");
        //obj.transform.position = new Vector3(1f, 0f, 5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            ReturnSavePoint();
    }

    public void ReturnSavePoint()
    {
        MonsterManager.Instance.ResetActiveMonsters();
        TimelineManager.Instance.ReturnSavePoint();
    }

    public void AircraftOn()
    {
        if (MonsterManager.Instance.activeMonsters.Count == 0)
            TimelineManager.Instance.PlayTimeline("CityEnd");
        else
            TimelineManager.Instance.PlayTimeline("CityFailed");
    }
}
