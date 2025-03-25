using UnityEngine;
using UnityEngine.InputSystem;

public class Rotator : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private float _playerRotationSensitivity = 10f;

    [Header("Camera")]
    [SerializeField] private float _cameraRotationSensitivity = 10f;
    [SerializeField] private float _minRotationCamera = 10f;
    [SerializeField] private float _maxRotationCamera = 70f;

    private Transform _player;
    private Transform _camera;
    private float _playerRotationByY = 0;
    private float _cameraRotationByX = 0;

    public void Initialize(Transform player, Transform camera, PlayerInput playerInput)
    {
        _player = player;
        _camera = camera;

        playerInput.Player.LookHorizontal.performed += OnLookHorizontal;
        playerInput.Player.LookVertical.performed += OnLookVertical;
    }

    public void Rotate()
    {
        RotatePlayer();
        RotateCamera();
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