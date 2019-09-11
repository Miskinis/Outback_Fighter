using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;
using ECS.Components;
using ECS.Components.PlayerInputs;

public class PlayerCharacterController : MonoBehaviour
{
    private CharacterController _characterController;
    private PlayerInput _playerInput;

    public float speed = 3.0f;
    public float jumpSpeed = 3.0f;
    public float gravity = 9.81f;

    private Vector3 _moveDirection = Vector3.zero;
    private float _moveDirectionValue;
    private bool _jumpValue;
    private bool _crouchValue;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _playerInput = GetComponent<PlayerInput>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        _jumpValue = context.ReadValue<float>() > 0f;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        _crouchValue = context.ReadValue<float>() > 0f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveDirectionValue = context.ReadValue<float>();
    }

    private void Update()
    {
        print(_playerInput.actions["Move"].ReadValue<float>());
        if (_characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            _moveDirection =  new Vector3(_moveDirectionValue, 0f, 0f);
            _moveDirection *= speed;

            if (_jumpValue)
            {
                _moveDirection.y = jumpSpeed;
                _jumpValue = false;
            }

            if (_crouchValue)
            {
                //Crouch Animation
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        _moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        _characterController.Move(_moveDirection * Time.deltaTime);
    }
}
