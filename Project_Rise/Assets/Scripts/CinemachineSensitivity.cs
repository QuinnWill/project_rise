using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineSensitivity : CinemachineExtension
{

    [SerializeField] CinemachineVirtualCamera _VirtualCamera;

    protected override void OnEnable()
    {
        base.OnEnable();
        PlayerPreferenceEditor.SensitivityCallback.AddListener(OnSensitivityChanged);
    }

    protected virtual void OnDisable()
    {
        PlayerPreferenceEditor.SensitivityCallback.RemoveListener(OnSensitivityChanged);
    }

    protected override void Awake()
    {
        base.Awake();

    }

    private void OnSensitivityChanged(float value)
    { 
        
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        { 
            
        }
    }
}
