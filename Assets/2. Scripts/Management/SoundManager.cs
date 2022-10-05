using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private string resourcesSoundPath;
    Dictionary<string, AudioClip> sounds = new Dictionary<string, AudioClip>();

    public GameObject soundPrafab;

    private void Awake()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sound");

        foreach(var clip in clips)
        {
            if(sounds.ContainsKey(clip.name))
            {
                Debug.LogError("같은 이름의 사운드가 있습니다");
                return;
            }
            sounds.Add(clip.name, clip);
        }
    }

    public void SoundPlay(string clipName, AudioSource audio)
    {
        audio.clip = sounds[clipName];
        audio.Play();
    }

    public void SoundPlay(AudioClip clip)
    {
        Instantiate(soundPrafab, transform).GetComponent<SoundPlayer>().Play(clip);
    }
    public void SoundPlay(string clipName)
    {
        Instantiate(soundPrafab, transform).GetComponent<SoundPlayer>().Play(Resources.Load<AudioClip>("/"+ clipName));
    }
}
