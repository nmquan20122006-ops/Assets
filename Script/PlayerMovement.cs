using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float sprint;
    public float slowdownSpeed;
    public float jumpforce;

    private bool isFacingRight = true;
    private Rigidbody2D rb;
    private float inputX = 0f;
    public float currentSpeed;

    private Animator animator;
    public LayerMask groundLayer;
    public Transform groundCheck;
    private PlayerInputManager _inputManager;

    public bool isGrounded;
    public float radius;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
    }
    public void HandleGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VelocityY", rb.velocity.y);
    }
    private void HandleHorizontalMovement()
    {
        rb.velocity = new Vector2(inputX * currentSpeed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(inputX * currentSpeed));
    }
    private void HandleVerticalMovement()
    {
        if (_inputManager.jumpRequested && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            animator.SetTrigger("Jump");
            _inputManager.ConsumeJumpBuffer();
        }
    }
    private void HandleInputX()
    {
        if (_inputManager.leftInput && !_inputManager.rightInput) { inputX = -1f; }
        else if (_inputManager.rightInput && !_inputManager.leftInput) { inputX = 1f; }
        else inputX = 0f;
    }
    private void HandleCurrentSpeed()
    {
        currentSpeed = _inputManager.ShiftPressed ? speed * sprint : speed;
    }
    private void HandleFlip(float inputX)
    {
        if (inputX > 0&&!isFacingRight)
        {
            Flip();
        }
        else if (inputX < 0&&isFacingRight)
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
    
}