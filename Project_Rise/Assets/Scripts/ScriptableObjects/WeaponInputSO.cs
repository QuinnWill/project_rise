using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "WeaponInput", menuName = "Player/Input/WeaponInput")]
public class WeaponInputSO : ScriptableObject
{
    public InputAction.CallbackContext Primary;
    public InputAction.CallbackContext Secondary;

    public event UnityAction<InputAction.CallbackContext> PrimaryStarted;
    public event UnityAction<InputAction.CallbackContext> whaterver;
}
