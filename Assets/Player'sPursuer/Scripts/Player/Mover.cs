using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class Mover : MonoBehaviour, IDisposable
{
    [Header("Speed")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _strafeSpeed = 8f;

    [Header("Gravity")]
    [SerializeField] private float _groundedGravity = -0.5f;
    [SerializeField] private float _airborneGravityMultiplier = 1f;

    [Header("Slope")]
    [SerializeField] private float _slopeForce = 5f;
    [SerializeField] private float _slopeRayLength = 1.5f;

    private Transform _transform;
    private Transform _camera;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private Jumper _jumper;

    private Vector2 _inputDirection;
    private float _verticalVelocity;
    private Vector3 _resultDirection;

    public void Initialize(Transform camera, CharacterController characterController, PlayerInput playerInput, Jumper jumper)
    {
        _transform = transform;
        _camera = camera;
        _characterController = characterController;
        _playerInput = playerInput;
        _jumper = jumper;

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

    public void UpdateMovement()
    {
        CalculateMovement();
        SlideDown();
        ApplyGravity();
        MoveCharacter();
    }

    public void Dispose()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.Move.performed -= OnMove;
            _playerInput.Player.Move.canceled -= OnMove;
        }

        if (_jumper != null)
        {
            _jumper.JumpButtonPressed -= AddJumpForce;
        }
    }

    private void SubscribeToEvents()
    {
        if (_playerInput != null)
        {
            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMove;
        }

        if (_jumper != null)
        {
            _jumper.JumpButtonPressed += AddJumpForce;
        }
    }

    private void CalculateMovement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_camera.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_camera.right, Vector3.up).normalized;

        _resultDirection = _inputDirection.y * _moveSpeed * forward +
                          _inputDirection.x * _strafeSpeed * right;
    }

    private void SlideDown()
    {
        if (Physics.Raycast(_transform.position, Vector3.down, out RaycastHit hitInfo, _slopeRayLength) == false)
            return;

        if (Vector3.Angle(hitInfo.normal, Vector3.up) > _characterController.slopeLimit)
        {
            _resultDirection.x += (1f - hitInfo.normal.y) * hitInfo.normal.x * _slopeForce;
            _resultDirection.z += (1f - hitInfo.normal.y) * hitInfo.normal.z * _slopeForce;
            _resultDirection.y -= _slopeForce;
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = _groundedGravity;
        }
        else
        {
            _verticalVelocity += Physics.gravity.y * _airborneGravityMultiplier * Time.deltaTime;
        }

        _resultDirection += _verticalVelocity * Vector3.up;
    }

    private void MoveCharacter()
    {
        _characterController.Move(_resultDirection * Time.deltaTime);
    }

    private void AddJumpForce(float force)
    {
        _verticalVelocity = force;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }
}