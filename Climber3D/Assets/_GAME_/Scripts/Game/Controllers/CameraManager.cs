using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CameraManager : PersistentSingleton<CameraManager>
{
    #region Public
    [FoldoutGroup("Cameras")]
    [SerializeField] CinemachineVirtualCamera gameplayCam;
    [FoldoutGroup("Cameras")]
    [SerializeField] CinemachineVirtualCamera successCam;
    [FoldoutGroup("Cameras")]
    [SerializeField] CinemachineVirtualCamera failCam;
    #endregion
   
    #region Local
    private Transform _targetTransform;
    private float _shakeTimer;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannelPerlin;
    #endregion

    private void Update()
    {
        if (_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime / Time.timeScale;
            if (_shakeTimer <= 0f)
            {
                _cinemachineBasicMultiChannelPerlin = gameplayCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    private void OnEnable()
    {
        EventManager.LevelLoadedEvent.AddListener(SelectGameplayCam);
        EventManager.LevelSuccessEvent.AddListener(SelectSuccessCam);
        EventManager.LevelFailEvent.AddListener(SelectFailCam);
        EventManager.ShakeCameraEvent.AddListener(ShakeCamera);
    }

    private void OnDisable()
    {
        EventManager.LevelLoadedEvent.RemoveListener(SelectGameplayCam);
        EventManager.LevelSuccessEvent.RemoveListener(SelectSuccessCam);
        EventManager.LevelFailEvent.RemoveListener(SelectFailCam);
        EventManager.ShakeCameraEvent.RemoveListener(ShakeCamera);
    }

    private void SelectGameplayCam(LevelLoadedEventData arg0)
    {
        SelectGameplayCam();
    }

    public void Init(Transform target)
    {
        _targetTransform = target;
        
        successCam.Follow = _targetTransform.transform;
        successCam.LookAt = _targetTransform.transform;
        gameplayCam.Follow = _targetTransform.transform;
        gameplayCam.LookAt = _targetTransform.transform;
        failCam.Follow = _targetTransform.transform;
        failCam.LookAt = _targetTransform.transform;

        SelectGameplayCam();
    }
    
    public void SelectGameplayCam()
    {
        gameplayCam.Priority = 11;
        failCam.Priority = 10;
        successCam.Priority = 10;

    }
	
    public void SelectFailCam()
    {
        failCam.Priority = 11;
        gameplayCam.Priority = 10;
        successCam.Priority = 10;
    }

    public void SelectSuccessCam()
    {
        gameplayCam.Priority = 10;
        failCam.Priority = 9;
        successCam.Priority = 12;
    }

    public void SetFailCameraTarget(Transform _target)
    {
        failCam.Follow = _target;
        failCam.LookAt = _target;
    }

    public void SetsuccessCameraTarget(Transform _target)
    {
        successCam.Follow = _target;
        successCam.LookAt = _target;
    }

    private void ShakeCamera(ShakeCameraData eventData)
    {
        _cinemachineBasicMultiChannelPerlin = gameplayCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = eventData.Intensity;
        _shakeTimer = eventData.Time;
    }


}
