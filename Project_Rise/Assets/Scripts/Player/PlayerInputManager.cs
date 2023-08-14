using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{

    public PlayerInputSO InputSO;

    public WeaponInputSO WeaponInputSO;

    private PlayerControls _playerControls;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Movement.Move.performed += OnMove;
        _playerControls.Movement.Jump.started += OnJump;
        _playerControls.Movement.Jump.performed += OnJump;
        _playerControls.Movement.Jump.canceled += OnJump;
        _playerControls.Movement.Crouch.started += OnCrouch;
        _playerControls.Movement.Crouch.performed += OnCrouch;
        _playerControls.Movement.Crouch.canceled += OnCrouch;
        _playerControls.Interaction.Primary.started += OnPrimary;
        _playerControls.Interaction.Primary.performed += OnPrimary;
        _playerControls.Interaction.Primary.canceled += OnPrimary;
        _playerControls.Interaction.Secondary.started += OnSecondary;
        _playerControls.Interaction.Secondary.performed += OnSecondary;
        _playerControls.Interaction.Secondary.canceled += OnSecondary;
    }

    private void OnDisable()
    {
        _playerControls.Movement.Move.performed -= OnMove;
        _playerControls.Movement.Jump.started -= OnJump;
        _playerControls.Movement.Jump.performed -= OnJump;
        _playerControls.Movement.Jump.canceled -= OnJump;
        _playerControls.Movement.Crouch.started -= OnCrouch;
        _playerControls.Movement.Crouch.performed -= OnCrouch;
        _playerControls.Movement.Crouch.canceled -= OnCrouch;
        _playerControls.Interaction.Primary.started -= OnPrimary;
        _playerControls.Interaction.Primary.performed -= OnPrimary;
        _playerControls.Interaction.Primary.canceled -= OnPrimary;
        _playerControls.Interaction.Secondary.started -= OnSecondary;
        _playerControls.Interaction.Secondary.performed -= OnSecondary;
        _playerControls.Interaction.Secondary.canceled -= OnSecondary;
        _playerControls.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        InputSO.MoveDir = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        InputSO.Jump = context;
    }


    public void OnCrouch(InputAction.CallbackContext context)
    {
        InputSO.Crouch = context;
    }

    public void OnPrimary(InputAction.CallbackContext context)
    {
        WeaponInputSO.Primary = context;
    }

    public void OnSecondary(InputAction.CallbackContext context)
    {
        WeaponInputSO.Secondary = context;
    }
}
