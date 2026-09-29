using System.Collections;
using System.Runtime.CompilerServices;
using Unity.Profiling;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    //----------BASE STATS----------------------------------
    [Header("Base Stats")]
    [SerializeField] private float _speed;//base speed only belong to PlayerMovement
    [SerializeField] private float _sprint;// base sprint only belong to PlayerMovement
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _jumpForce;// base jump force only belong to PlayerMovement
    [SerializeField] private int maxJumps;
    //---------GROUND CHECK--------------------------------
    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _radius;
    //---------RUNTIME STATE-------------------------------
    private bool _isGrounded;
    private bool isFacingRight = true;
    private float inputX = 0f;
    private int _currentJumpStep;
    private float? _overrideFallSpeed = null;
    //---------COMPONENTS----------------------------------
    private PlayerInputManager _inputManager;
    private Animator _animator;
    private Rigidbody2D _rb;
    //---------EVENTS-------------------------------------
    public System.Action _onJumpRequested;
    //---------PROPERTIES---------------------------------
    public bool g_isGrounded => _isGrounded;
    private float _currentJumpForce => _jumpForce * jumpForceMultipler;
    //---------MODIFIER-----------------------------------
    public float jumpForceMultipler { get; set; } = 1f;// jump force multipler the modifier that is used to modify the base speed, used for other scripts
    public float speedMultipler { get; set; } = 1f; // speed multiple the modifier that is used to modify the base speed, used for other scripts

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _inputManager = GetComponent<PlayerInputManager>();
    }
    // Update is called once per frame
    void Update()
    {
        HandleInputX();
        HandleGrounded();
        HandleCurrentSpeed();
        HandleFlip(inputX);
        HandleVerticalMovement();
    }
    private void FixedUpdate()
    {
        HandleHorizontalMovement();
        ApplyCustomFallSpeed();
    }
    public void HandleGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _radius, _groundLayer);
        _animator.SetBool("IsGrounded", _isGrounded);
        _animator.SetFloat("VelocityY", _rb.velocity.y);
        if (_isGrounded && _rb.velocity.y <= 0.1f)
        {
            _currentJumpStep = 0; 
        }
    }
    private void HandleHorizontalMovement()
    {
        _rb.velocity = new Vector2(inputX * _currentSpeed, _rb.velocity.y);
        _animator.SetFloat("Speed", Mathf.Abs(inputX * _currentSpeed));
    }
    private void HandleVerticalMovement()
    {
        //normal jump
        if (_inputManager.jumpRequested && _currentJumpStep < maxJumps)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _currentJumpForce);
            _animator.SetTrigger("Jump");
            _currentJumpStep++;
            _inputManager.ConsumeJumpBuffer();
            _onJumpRequested?.Invoke();
        }
        //jumpcut
        if (_inputManager.jumpReleased && _rb.velocity.y > 0f && _currentJumpStep == 1)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * 0.4f);
        }
        //air attack
        /*if (_attackController.g_isAirAttack && _rb.velocity.y < 0)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * airAttackFallSpeed);
        }*/
    }
    private void HandleInputX()
    {
        if (_inputManager.leftInput && !_inputManager.rightInput) { inputX = -1f; }
        else if (_inputManager.rightInput && !_inputManager.leftInput) { inputX = 1f; }
        else inputX = 0f;
    }
    private void HandleCurrentSpeed()
    {
        float sprintMultiplier = _inputManager.ShiftPressed ? _sprint : 1f;
        _currentSpeed = _speed * sprintMultiplier * speedMultipler;
    }
    private void HandleFlip(float inputX)
    {
        if (inputX > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (inputX < 0 && isFacingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }
    public void SetOverrideFallSpeed(float? fallSpeed)
    {
        _overrideFallSpeed = fallSpeed;
    }
    private void ApplyCustomFallSpeed()
    {
        if (_overrideFallSpeed.HasValue && _rb.velocity.y < 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, -Mathf.Abs(_overrideFallSpeed.Value));
        }
    }
}