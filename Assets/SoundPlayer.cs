using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.clip = clip;
        StartCoroutine(PlayCo());
    }

    IEnumerator PlayCo()
    {
        //³¡³ª¸é Destroy(
    }

}
