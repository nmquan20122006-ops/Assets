using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 25f;
    public float sprint = 1.8f;
    public float jumpforce = 15f;
    public float slideSpeed = 10f;

    [SerializeField] private Vector2 playerVector;
    private Rigidbody2D rb;
    private float inputX;
    private float currentSpeed;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public bool isGrounded;
    public bool isSliding;
    public float radius = 0.2f;
    public float slideDuration = 0.5f;
    public float slideDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");

        bool isShiftPressed = Input.GetKey(KeyCode.LeftShift);
        currentSpeed = isShiftPressed? speed*sprint
            : speed;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius ,groundLayer);

        
        animator.SetFloat("Speed", Mathf.Abs(inputX*currentSpeed));

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpforce);
            animator.SetTrigger("Jump");
            
        }

        if (Input.GetKeyDown(KeyCode.LeftControl)&&isGrounded&&!isSliding)
        {
            StartCoroutine(performSlide());
        }

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VelocityY", rb.velocity.y);
        //animator.SetBool("IsSliding", isSliding);

        float moveInputSprite = Input.GetAxisRaw("Horizontal");

        if (inputX > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if(inputX < 0)
        {
            spriteRenderer.flipX = true;
        }
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(inputX * currentSpeed, rb.velocity.y);

        if (isSliding)
        {
            rb.velocity = new Vector2(slideDirection * slideSpeed, rb.velocity.y);
            return;
        }
        playerVector = transform.position;
    }
    private IEnumerator performSlide()
    {
        isSliding = true;

        slideDirection = spriteRenderer.flipX ? -1 : 1;
        if (animator != null)
        {
            //animator.SetTrigger("Slide");
        }
        yield return new WaitForSeconds(slideDuration);
        isSliding = false;

    }
}
