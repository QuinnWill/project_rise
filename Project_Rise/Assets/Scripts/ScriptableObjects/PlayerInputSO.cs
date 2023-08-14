using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerInput", menuName = "Player/Input/PlayerInput")]
public class PlayerInputSO : ScriptableObject
{
    public Vector2 MoveDir;
    public Vector2 MouseDelta;
    public InputAction.CallbackContext Jump;
    public InputAction.CallbackContext Crouch;
}
