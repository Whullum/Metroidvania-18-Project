using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float _hangTimeCounter;
    private float _horizontalMovement;
    private bool _isFacingRight = true; // Needs to be set to true if the character starts facing to the right.
    private bool _jumpButtonPressed;
    private bool _jumpButtonReleased;
    private bool _doubleJump = false;
    private bool _canDash = true;
    private bool _isDashing;
    private Rigidbody2D _rBody;

    [Header("Movement values")]
    [Range(1, 1000)]
    [Tooltip("The character movement speed.")]
    [SerializeField] private float _movementSpeed = 200.0f;
    [Range(0, 10)]
    [Tooltip("The character gravity scale of the rigidbody.")]
    [SerializeField] private float _gravityScale = 5.0f;
    [Header("Jump values")]
    [Range(1, 100)]
    [Tooltip("The force with wich the character jumps.")]
    [SerializeField] private float _jumpForce = 15.0f;
    [Range(0, 1)]
    [Tooltip("The influence on the chacter jump if the player holds the jump button down. 0 = the jump is shorter. 1 = helding the key down does not make any difference.")]
    [SerializeField] private float _jumpReleaseRatio = 0.5f;
    [Range(0, 0.5f)]
    [Tooltip("Time the player is able to jump after the character stopped touching the ground.")]
    [SerializeField] private float _hangTime = 0.1f;
    [Header("Dash values")]
    [Range(1, 1000)]
    [Tooltip("The force of the dash. Greater force means greater traveled distance.")]
    [SerializeField] private float _dashForce = 100.0f;
    [Range(0.01f, 3.0f)]
    [Tooltip("The time dashing. The greater the time, greater the traveled distance.")]
    [SerializeField] private float _dashTime = 0.05f;
    [Range(0.1f, 10.0f)]
    [Tooltip("Cooldown time of the dash.")]
    [SerializeField] private float _dashCooldown = 1.0f;
    [Header("GroundCheck values")]
    [Tooltip("Ground check origin where the physics engine checks if the character is touching the ground.")]
    [SerializeField] private Transform _groundCheck;
    [Range(0.01f, 0.3f)]
    [Tooltip("Radius of the ground check.")]
    [SerializeField] private float _groundCheckRadius = 0.1f;
    
    [Header("Debug")]
    [SerializeField] private bool _showDebugInfo;

    private void Awake()
    {
        _rBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        InitializeRigidbody();
    }

    private void Update()
    {
        CheckInput();
        Flip();
    }

    private void FixedUpdate()
    {
        if (_isDashing) { return; }

        CheckGrounded();
        Move();
        Jump();
    }

    /// <summary>
    /// Sets the default properties for the player rigidbody.
    /// </summary>
    private void InitializeRigidbody()
    {
        _rBody.gravityScale = _gravityScale;
        _rBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        _rBody.sleepMode = RigidbodySleepMode2D.NeverSleep;
        _rBody.interpolation = RigidbodyInterpolation2D.Interpolate;
        _rBody.freezeRotation = true;
    }

    /// <summary>
    /// Checks for player input for moving the character.
    /// </summary>
    private void CheckInput()
    {
        if (_canDash && Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(Dash());

        _horizontalMovement = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W))
        {
            _jumpButtonPressed = true;
            _jumpButtonReleased = false;
        }

        if (Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.W))
            _jumpButtonReleased = true;
    }

    /// <summary>
    /// Moves the character accordingly to the player input.
    /// </summary>
    private void Move()
    {
        _rBody.velocity = new Vector2(_horizontalMovement * _movementSpeed * Time.deltaTime, _rBody.velocity.y);
    }

    /// <summary>
    /// Flips the character depending on player input.
    /// </summary>
    private void Flip()
    {
        if (_isFacingRight && _horizontalMovement < 0f || !_isFacingRight && _horizontalMovement > 0f)
        {
            _isFacingRight = !_isFacingRight;

            Vector3 localScale = transform.localScale;

            localScale.x *= -1;

            transform.localScale = localScale;
        }
    }

    /// <summary>
    /// Checks if the player can jump and if it does makes the jump. Also it checks the double jump variable and excecutes one 
    /// if the character is able to do so.
    /// </summary>
    private void Jump()
    {
        if (_jumpButtonPressed)
        {
            _jumpButtonPressed = false;

            if (_hangTimeCounter > 0f)
            {
                _rBody.velocity = new Vector2(_rBody.velocity.x, _jumpForce);
                _doubleJump = true;
                _hangTimeCounter = 0;
            }
            else
            {
                if (_doubleJump)
                {
                    _rBody.velocity = new Vector2(_rBody.velocity.x, _jumpForce);
                    _doubleJump = false;
                }
            }
        }
        if (_jumpButtonReleased && _rBody.velocity.y > 0f)
            _rBody.velocity = new Vector2(_rBody.velocity.x, _rBody.velocity.y * _jumpReleaseRatio);
    }

    /// <summary>
    /// Checks if the character is touching the ground.
    /// </summary>
    private void CheckGrounded()
    {
        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, ~LayerMask.GetMask("Player"));

        if (isGrounded)
            _hangTimeCounter = _hangTime;
        else
            _hangTimeCounter -= Time.deltaTime;
    }

    /// <summary>
    /// Makes the character dash.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Dash()
    {
        _canDash = false;
        _isDashing = true;

        _rBody.gravityScale = 0f;
        _rBody.velocity = new Vector2(transform.localScale.x * _dashForce, 0);

        yield return new WaitForSeconds(_dashTime);

        _isDashing = false;
        _rBody.gravityScale = _gravityScale;

        yield return new WaitForSeconds(_dashCooldown);

        _canDash = true;
    }

    private void OnDrawGizmos()
    {
        if(!_showDebugInfo) { return; }

        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
}
