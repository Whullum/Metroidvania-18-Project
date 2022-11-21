using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _vCam;
    private CinemachineBasicMultiChannelPerlin _noisePerlin;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _noisePerlin = _vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        CameraEvents.CameraShake += StartShake;
    }

    private void OnDisable()
    {
        CameraEvents.CameraShake -= StartShake;
    }

    /// <summary>
    /// Shakes the camera with a fixed duration and force.
    /// </summary>
    /// <param name="shakeTime">The duration of the shake.</param>
    /// <param name="force">The force of the shake.</param>
    private void StartShake(float shakeTime, float force)
    {
        if(_noisePerlin == null)
        {
            Debug.LogError("Camera Controller ERROR : Cinemachine noise profile needs to be set.");
            return;
        }

        _noisePerlin.m_AmplitudeGain = force / 10;
        _noisePerlin.m_FrequencyGain = force;

        CancelInvoke("StopShake");
        Invoke("StopShake", shakeTime);
    }

    /// <summary>
    /// Stops shaking the camera.
    /// </summary>
    private void StopShake()
    {
        _noisePerlin.m_AmplitudeGain = 0f;
        _noisePerlin.m_FrequencyGain = 0f;
    }
}
