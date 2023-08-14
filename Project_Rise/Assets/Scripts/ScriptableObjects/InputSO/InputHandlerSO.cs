using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputHandler", menuName = "Player/Input/InputHandler")]
public class InputHandlerSO : ScriptableObject, PlayerControls.IMovementActions, PlayerControls.IInteractionActions, PlayerControls.IPersistentActions
{

    public PlayerControls _playerControls;

    public event UnityAction<InputAction.CallbackContext> JumpEvent;
    public event UnityAction<InputAction.CallbackContext> MoveEvent;
    public event UnityAction<InputAction.CallbackContext> CrouchEvent;
    public event UnityAction<InputAction.CallbackContext> PrimaryEvent;
    public event UnityAction<InputAction.CallbackContext> SecondaryEvent;
    public event UnityAction<InputAction.CallbackContext> PauseEvent;

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();

            _playerControls.Movement.SetCallbacks(this);
            _playerControls.Interaction.SetCallbacks(this);
            _playerControls.Persistent.SetCallbacks(this);
        }

        _playerControls.Movement.Enable();
        _playerControls.Interaction.Enable();
        _playerControls.Persistent.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Movement.Disable();
        _playerControls.Interaction.Disable();
        _playerControls.Persistent.Disable();
    }

    #region Enable/Disable
    public void EnableAll()
    {
        _playerControls.Movement.Enable();
        _playerControls.Interaction.Enable();
        _playerControls.Persistent.Enable();
    }

    public void DisableAll()
    {
        _playerControls.Movement.Disable();
        _playerControls.Interaction.Disable();
        _playerControls.Persistent.Disable();
    }

    public void EnableMovement() { _playerControls.Movement.Enable(); }
    public void DisableMovement() { _playerControls.Movement.Disable(); }

    public void EnableInteraction() { _playerControls.Interaction.Enable(); }
    public void DisableInteraction() { _playerControls.Interaction.Disable(); }

    public void EnablePersistent() { _playerControls.Persistent.Enable(); }
    public void DisablePersistent() { _playerControls.Persistent.Disable(); }
    #endregion

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke(context);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchEvent?.Invoke(context);
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        PrimaryEvent?.Invoke(context);
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        SecondaryEvent?.Invoke(context);
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        PauseEvent?.Invoke(context);
    }
}
