using UnityEngine;
using UnityEngine.InputSystem;

public class Jumper : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 5f;

    private CharacterController _characterController;

    public void Initialize(CharacterController characterController, PlayerInput playerInput)
    {
        _characterController = characterController;
        playerInput.Player.Jump.performed += Jump;
    }

    private void Jump(InputAction.CallbackContext context)
    {

    }
}