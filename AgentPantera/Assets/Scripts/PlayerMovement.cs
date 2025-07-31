using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float slowFactor;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fallMultiplier;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    private Rigidbody2D _rigidbody2D;

    
    [SerializeField] private bool _isGrounded;

    private void Awake()
    {
        _moveAction = inputActions.FindAction("Move");
        _jumpAction = inputActions.FindAction("Jump");
        
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleJumping();
        AccelerateOnFalling();
    }

    private void HandleMovement()
    {
        var moveInput = _moveAction.ReadValue<float>();
        
        if (_isGrounded)
            _rigidbody2D.linearVelocityX = moveInput * movementSpeed;
        
        // slow down if no input
        if (moveInput == 0)
            _rigidbody2D.linearVelocityX = Mathf.MoveTowards(_rigidbody2D.linearVelocityX, 0, slowFactor);

        
        // slow down even more when input is in opposite direction
        if (moveInput * _rigidbody2D.linearVelocityX < 0)
            _rigidbody2D.linearVelocityX = Mathf.MoveTowards(_rigidbody2D.linearVelocityX, 0, slowFactor * 2);
    }

    private void HandleJumping()
    {
        var jumpButtonPressed = _jumpAction.IsPressed();

        if (jumpButtonPressed && _isGrounded)
            _rigidbody2D.linearVelocityY = jumpForce;
    }

    private void AccelerateOnFalling()
    {
        if (_rigidbody2D.linearVelocityY < 0)
            _rigidbody2D.linearVelocityY *= fallMultiplier;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            _isGrounded = false;
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }
}
