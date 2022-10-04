using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineFreeLook mainCM;
    [SerializeField] private CinemachineFreeLook topCM;
    private CinemachineBasicMultiChannelPerlin cameraNoise;

    private Coroutine shakeCoroutine;

    private bool isLock = false;
    public bool IsLock
    {
        get { return isLock; }

        set
        {
            isLock = value;
            mainCM.enabled = !isLock;
        }
    }

    public void MainCMLock(bool isLock)
    {
        mainCM.enabled = isLock;
    }

    public void SetMainCM()
    {
        mainCM = FindObjectOfType<CinemachineFreeLook>();
    }

    public void MainCMChange()
    {
        mainCM = topCM;
    }

    void Awake()
    {
        if(mainCM == null)
        {
            mainCM = FindObjectOfType<CinemachineFreeLook>();
        }
        
        cameraNoise = mainCM.GetRig(1).GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    public void CameraShaking(float duration, float amplitude, float frequency = 1f)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);
        shakeCoroutine = StartCoroutine(CameraShakeCo(duration, amplitude, frequency));
    }

    void Update()
    {
    }

    IEnumerator CameraShakeCo(float duration, float amplitude, float frequency = 1f)
    {
        float shakeTime = 0f;
        cameraNoise.m_AmplitudeGain = amplitude;
        cameraNoise.m_FrequencyGain = 1f;
        while (shakeTime <= duration)
        {
            Debug.Log(shakeTime);
            shakeTime += Time.deltaTime;
            yield return null;
        }
        cameraNoise.m_AmplitudeGain = 0f;
    }
}
