using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class QCameraInputProvider : MonoBehaviour, AxisState.IInputAxisProvider
{

    [SerializeField] InputActionReference _inputActionReference;

    public float GetAxisValue(int axis)
    {
        if (enabled)
        {
            var action = _inputActionReference.action;
            action.Enable();
            if (action != null)
            {
                switch (axis)
                {
                    case 0: return action.ReadValue<Vector2>().x * PlayerPreferenceEditor.GetSensitivity();
                    case 1: return action.ReadValue<Vector2>().y * PlayerPreferenceEditor.GetSensitivity();
                    case 2: return action.ReadValue<float>() * PlayerPreferenceEditor.GetSensitivity();
                }
            }
        }
        return 0;
    }

}
