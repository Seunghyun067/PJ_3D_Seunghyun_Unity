using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : Singleton<VolumeManager>
{
    private Volume v;
    private VolumeProfile vp;

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

        while (vig.intensity.value > 0f)
        {
            vig.intensity.value -= Time.deltaTime;
            yield return null;
        }
        vig.intensity.value = 0f;
    }

    public void Vig()
    {
        if (!v) SetVolume();

        if(vp.TryGet<Vignette>(out var vig))
        {
            vig.intensity.value = 0.1f;
        }
    }



    
   
}
