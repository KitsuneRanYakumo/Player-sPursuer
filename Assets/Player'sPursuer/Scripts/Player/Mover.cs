using UnityEngine.InputSystem;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _strafeSpeed = 8f;
    [SerializeField] private float _groundedGravity = -0.5f;
    [SerializeField] private float _airborneGravityMultiplier = 1f;

    private Transform _camera;
    private CharacterController _characterController;
    private Vector2 _inputDirection;
    private float _verticalVelocity;
    private Vector3 _resultDirection;

    public void Initialize(Transform camera, CharacterController characterController, PlayerInput playerInput)
    {
        _camera = camera;
        _characterController = characterController;

        playerInput.Player.Move.performed += OnMove;
        playerInput.Player.Move.canceled += OnMove;
    }

    public void UpdateMovement()
    {
        CalculateMovement();
        ApplyGravity();
        MoveCharacter();
    }

    private void CalculateMovement()
    {
        Vector3 forward = Vector3.ProjectOnPlane(_camera.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(_camera.right, Vector3.up).normalized;

        _resultDirection = _inputDirection.y * _moveSpeed * forward +
                          _inputDirection.x * _strafeSpeed * right;
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded)
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

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }
}