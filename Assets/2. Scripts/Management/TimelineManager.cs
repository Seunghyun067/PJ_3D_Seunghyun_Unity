using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Cinemachine;

[RequireComponent(typeof(PlayableDirector))]

public class TimelineManager : Singleton<TimelineManager>
{
    [SerializeField] List<TimelineAsset> timelineInputs;

    Dictionary<string, TimelineAsset> timelines = new Dictionary<string, TimelineAsset>();

    private PlayableDirector director;

    VolumeProfile vp;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();

        foreach (var timeline in timelineInputs)
            timelines.Add(timeline.name, timeline);
    }

    public void PlayTimeline(string str)
    {
        
        director.playableAsset = timelines[str];
        director.Play();
    }
    public void PlayTimeline(TimelineAsset timelineAsset)
    {
        director.playableAsset = timelines[timelineAsset.name];
        director.Play();        
    }

    public void StartHold()
    {
        GameManager.Instance.IsKeyHold = true;
        CameraManager.Instance.IsLock = true;
        StartCoroutine(VolumeManager.Instance.VigStart(0.3f));

    }
    public void EndPlayTimeline()
    {
        director.Stop();
        GameManager.Instance.IsKeyHold = false;
        CameraManager.Instance.IsLock = false;
        StartCoroutine(VolumeManager.Instance.VigEnd());
        Debug.Log("EndPlayTimeline");
    }

    public void NextScene(string nextSceneName)
    {
        LoadingSceneManager.LoadScene(nextSceneName);

    }

}
