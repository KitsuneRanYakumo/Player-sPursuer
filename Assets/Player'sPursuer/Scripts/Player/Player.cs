using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private Mover _mover;
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Jumper _jumper;

    private CharacterController _characterController;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = new PlayerInput();

        _jumper.Initialize(_characterController, _playerInput);
        _mover.Initialize(_camera, _characterController, _playerInput, _jumper);
        _rotator.Initialize(transform, _camera, _playerInput);
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void Update()
    {
        _mover.UpdateMovement();
        _rotator.Rotate();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }

    private void OnDestroy()
    {
        _mover.Dispose();
        _rotator.Dispose();
        _jumper.Dispose();

        _playerInput?.Dispose();
        _playerInput = null;
    }
}