using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineCameraShake : MonoBehaviour
{
    //make camera shake easy to use - can access this from anywhere else
    public static CinemachineCameraShake Instance
    {
        get;
        private set;
    }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float cameraShakeTimer;
    private float cameraShakeTimerTotal;
    public bool useSimpleCameraShake = false;
    private float startingAmplitudeIntensity = 0f;
    private float startingFrequencyIntensity = 0f;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float amplitudeIntensity, float frequencyIntensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitudeIntensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyIntensity;

        //after specified time: drop to 0 or slowly decreases intensity over time
        startingAmplitudeIntensity = amplitudeIntensity;
        startingFrequencyIntensity = frequencyIntensity;
        cameraShakeTimerTotal = time;
        cameraShakeTimer = time;
    }

    private void Update()
    {
        Debug.Log("cameraShakeTimer: " + cameraShakeTimer);
        Debug.Log("cameraShakeTimerTotal: " + cameraShakeTimerTotal);

        if (useSimpleCameraShake == true)
        {
            cameraShakeTimer -= Time.deltaTime;
            if (cameraShakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f; //stop camera shake immediately after timer stopped
            }
        }
        else //smoothly decreases camera shake to 0 - better for explosion
        {
            if (cameraShakeTimer > 0)
            {
                cameraShakeTimer -= Time.deltaTime;
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                //as the cameraShakeTimerTotal goes down: intensity goes from 1 to 0 instead of an abrupt stop if useSimpleCameraShake 
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingAmplitudeIntensity, 0f, 1 - (cameraShakeTimer / cameraShakeTimerTotal));
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(startingFrequencyIntensity, 0f, 1 - (cameraShakeTimer / cameraShakeTimerTotal));
            }
        }
    }
}