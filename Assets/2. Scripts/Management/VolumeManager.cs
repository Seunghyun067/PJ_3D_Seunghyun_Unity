using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : Singleton<VolumeManager>
{
    private Volume v;
    private VolumeProfile vp;
    private float curVigValue = 0f;

    private void Start()
    {
        SetVolume();
        
    }

    public T GetShared<T>() where T : VolumeComponent
    {
        T shared = null;
        vp.TryGet<T>(out var s);
        shared = s;
        return shared;
    }

    public void SetVolume()
    {
        v = FindObjectOfType<Volume>();
        vp = v.profile;
        Vignette vig = GetShared<Vignette>();
        curVigValue = vig.intensity.value;
        Debug.Log(curVigValue);
    }

    public IEnumerator VigStart(float dstIntensity)
    {
        if (!v) SetVolume();
        Vignette vig = GetShared<Vignette>();

        while(vig.intensity.value < dstIntensity)
        {
            vig.intensity.value += Time.deltaTime;
            yield return null;
        }
        vig.intensity.value = dstIntensity;
    }

    public IEnumerator VigEnd()
    {
        if (!v) SetVolume();
        Vignette vig = GetShared<Vignette>();

        while (vig.intensity.value > curVigValue)
        {
            vig.intensity.value -= Time.deltaTime * 0.5f;
            yield return null;
        }
        vig.intensity.value = curVigValue;
    }

    public void Vig()
    {
        if (!v) SetVolume();

        if(vp.TryGet<Vignette>(out var vig))
        {
            vig.intensity.value = 0.1f;
        }
    }

    public IEnumerator ChromaticStart(float dstIntensity, float spd)
    {
        if (!v) SetVolume();
        ChromaticAberration ch = GetShared<ChromaticAberration>();
        ch.intensity.max = dstIntensity;
        ch.intensity.value = 0f;


        while (ch.intensity.value < dstIntensity)
        {
            ch.intensity.value += Time.deltaTime * spd;
            yield return null;
        }
        ch.intensity.value = dstIntensity;
    }

    public IEnumerator ChromaticEnd(float spd)
    {
        if (!v) SetVolume();
        ChromaticAberration ch = GetShared<ChromaticAberration>();

        while (ch.intensity.value > 0f)
        {
            ch.intensity.value -= Time.deltaTime * 10f;
            yield return null;
        }
        ch.intensity.value = 0f;
    }





}
