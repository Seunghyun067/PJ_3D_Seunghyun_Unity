using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]

public class Sound<T> where T : System.Enum
{

    [SerializeField] private AudioClip[] inAudios;
    private Dictionary<T, AudioClip> audios = new Dictionary<T, AudioClip>();
    
    private AudioSource audioSource = null;



    public void SoundPlay(T type) 
    {
        audioSource.clip = audios[type];
        audioSource.Play();
    }

}
