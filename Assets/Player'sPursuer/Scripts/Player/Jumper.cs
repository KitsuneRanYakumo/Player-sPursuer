using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jumper : MonoBehaviour, IDisposable
{
    [SerializeField] private float _jumpForce = 5f;

    private CharacterController _characterController;
    private PlayerInput _playerInput;

    public event Action<float> JumpButtonPressed;

    public void Initialize(CharacterController characterController, PlayerInput playerInput)
    {
        _characterController = characterController;
        _playerInput = playerInput;

        Dispose();
        SubscribeToEvents();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        Dispose();
    }

    private void OnDestroy()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.Jump.performed -= Jump;
        }
    }

    private void SubscribeToEvents()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.Jump.performed += Jump;
        }
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded)
        {
            JumpButtonPressed?.Invoke(_jumpForce);
        }
    }
}