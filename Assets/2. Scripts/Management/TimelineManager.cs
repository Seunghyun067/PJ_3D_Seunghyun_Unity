using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

[RequireComponent(typeof(PlayableDirector))]

public class TimelineManager : Singleton<TimelineManager>
{
    [SerializeField] List<TimelineAsset> timelineInputs;

    Dictionary<string, TimelineAsset> timelines = new Dictionary<string, TimelineAsset>();

    private PlayableDirector director;

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
    public void EndPlayTimeline()
    {
        director.Stop();
    }
}
