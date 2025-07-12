using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : NetworkBehaviour
{
    private Vector3 _velocity;
    private Vector2 _moveInput;
    private bool _jumpPressed;

    private CharacterController _controller;

    public InputActions playerControls;

    public float PlayerSpeed = 50000f;
    public float JumpForce = 50f;
    public float GravityValue = -9.81f;

    private void Awake()
    {
        playerControls = new InputActions();
    }

    private void OnEnable()
    {
        playerControls.Gameplay.Enable();
        playerControls.Gameplay.Jump.performed += OnJump;
        playerControls.Gameplay.Move.performed += OnMove;
        playerControls.Gameplay.Move.canceled += OnMove;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _jumpPressed = true;
        Debug.Log("jump");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        Debug.Log("move");
    }

    public override void Spawned()
    {
        _controller = GetComponent<CharacterController>();
        Debug.Log($"HasStateAuthority: {HasStateAuthority}, HasInputAuthority: {HasInputAuthority}");
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority || _controller == null) return;

        Vector3 moveDirection = new Vector3(-_moveInput.y*10, 0, _moveInput.x*10);
        moveDirection = transform.TransformDirection(moveDirection) * PlayerSpeed;

        if (_controller.isGrounded)
        {
            _velocity.y = 0f;

            if (_jumpPressed)
            {
                _velocity.y = JumpForce;
                _jumpPressed = false;
            }
        }
        _velocity.y += GravityValue * Runner.DeltaTime;

        // Combine movement
        Vector3 finalMove = moveDirection;
        finalMove.y = _velocity.y;

        _controller.Move(finalMove * Runner.DeltaTime);

    }

    public PlayerInput CollectInput()
    {
        var input = new PlayerInput();
        input.Buttons.Set(MyButtons.Forward, _moveInput.y > 0.1f);
        input.Buttons.Set(MyButtons.Backward, _moveInput.y < -0.1f);
        input.Buttons.Set(MyButtons.Left, _moveInput.x < -0.1f);
        input.Buttons.Set(MyButtons.Right, _moveInput.x > 0.1f);
        input.Buttons.Set(MyButtons.Jump, _jumpPressed);
        return input;
    }
}