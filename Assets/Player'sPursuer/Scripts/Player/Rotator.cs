using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotator : MonoBehaviour, IDisposable
{
    [Header("Player")]
    [SerializeField] private float _playerRotationSensitivity = 10f;

    [Header("Camera")]
    [SerializeField] private float _cameraRotationSensitivity = 10f;
    [SerializeField] private float _minRotationCamera = 10f;
    [SerializeField] private float _maxRotationCamera = 70f;

    private Transform _player;
    private Transform _camera;
    private PlayerInput _playerInput;
    private float _playerRotationByY = 0;
    private float _cameraRotationByX = 0;

    public void Initialize(Transform player, Transform camera, PlayerInput playerInput)
    {
        _player = player;
        _camera = camera;
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

    public void Rotate()
    {
        RotatePlayer();
        RotateCamera();
    }

    public void Dispose()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.LookHorizontal.performed -= OnLookHorizontal;
            _playerInput.Player.LookVertical.performed -= OnLookVertical;
        }
    }

    private void SubscribeToEvents()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.LookHorizontal.performed += OnLookHorizontal;
            _playerInput.Player.LookVertical.performed += OnLookVertical;
        }
    }

    private void RotatePlayer()
    {
        _player.eulerAngles = Vector3.up * _playerRotationByY;
    }

    private void RotateCamera()
    {
        _camera.localEulerAngles = Vector3.right * _cameraRotationByX;
    }

    private void OnLookHorizontal(InputAction.CallbackContext context)
    {
        _playerRotationByY += context.ReadValue<float>() * _playerRotationSensitivity * Time.deltaTime;
    }

    private void OnLookVertical(InputAction.CallbackContext context)
    {
        _cameraRotationByX -= context.ReadValue<float>() * _cameraRotationSensitivity * Time.deltaTime;
        _cameraRotationByX = Mathf.Clamp(_cameraRotationByX, _minRotationCamera, _maxRotationCamera);
    }
}