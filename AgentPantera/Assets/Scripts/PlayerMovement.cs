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
    
    [Header("Grounded")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private float castDistance;
    [SerializeField] private LayerMask groundLayer;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    
    private Rigidbody2D _rigidbody2D;

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

        if (IsGrounded())
        {
            _rigidbody2D.linearVelocityX = moveInput * movementSpeed;

            return;
        }
        
        // slow down if no input
        if (moveInput == 0)
            _rigidbody2D.linearVelocityX = Mathf.MoveTowards(_rigidbody2D.linearVelocityX, 0, slowFactor * Time.fixedDeltaTime);
        
        // slow down even more when input is in opposite direction
        if (moveInput * _rigidbody2D.linearVelocityX < 0)
            _rigidbody2D.linearVelocityX = Mathf.MoveTowards(_rigidbody2D.linearVelocityX, 0, slowFactor * 2 * Time.fixedDeltaTime);
    }

    private void HandleJumping()
    {
        var jumpButtonPressed = _jumpAction.IsPressed();

        if (jumpButtonPressed && IsGrounded())
            _rigidbody2D.linearVelocityY = jumpForce;
    }

    private void AccelerateOnFalling()
    {
        if (_rigidbody2D.linearVelocityY < 0)
            _rigidbody2D.linearVelocityY *= fallMultiplier;
    }

    private bool IsGrounded() => Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
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
