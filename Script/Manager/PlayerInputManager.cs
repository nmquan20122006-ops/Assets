using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputManager : MonoBehaviour
{
    [field: SerializeField] private float jumpBufferTime { get; set; }
    [field: SerializeField] public bool rightInput { get; set; }
    [field: SerializeField] public bool leftInput { get; set; }
    [field: SerializeField] public bool downInput { get; set; }
    [field: SerializeField] public bool isLocked { get; set; }
    [field: SerializeField] public bool ShiftPressed { get; set; }
    [field: SerializeField] public bool LMousePressed {  get; set; }
    [field: SerializeField] public float attackBufferTime {  get; set; }
    public bool jumpReleased => Keyboard.current.spaceKey.wasReleasedThisFrame;

    private float jumpBufferTimer;
    private float attackBufferTimer;
    public bool jumpRequested => jumpBufferTimer > 0f;
    public bool attackRequested => attackBufferTimer > 0f;

    void Start()
    {
        rightInput = false;
        leftInput = false;
        downInput = false;
        jumpBufferTimer = 0f;
        isLocked = false;
        downInput = false;
        ShiftPressed = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isLocked)
        {
            ResetInput(); return;
        }
        HandleMovementInput();
        HandleJumpBuffer();
        HandleAttackBuffer();
        HandleAttackInput();
    }
    private void HandleMovementInput()
    {
        if (Keyboard.current == null) return;
        rightInput = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        leftInput = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
        ShiftPressed = Keyboard.current.leftShiftKey.isPressed;
        bool rawDownInput = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
        downInput = rawDownInput && !rightInput && !leftInput;
    }
    private void HandleAttackInput()
    {
        if (Mouse.current == null) return;
        LMousePressed = Mouse.current.leftButton.wasPressedThisFrame;
    }
    private void HandleJumpBuffer()
    {
        if (Keyboard.current == null) return;
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            jumpBufferTimer = jumpBufferTime;
        }
        if (jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

    }
    private void HandleAttackBuffer()
    {
        if (Mouse.current == null) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            attackBufferTimer = attackBufferTime;
        } else if (attackBufferTimer > 0)
        {
            attackBufferTimer -= Time.deltaTime;
        }
    }
    public void ConsumeJumpBuffer()
    {
        jumpBufferTimer = 0f;
    }
    public void ConsumeAttackBuffer()
    {
        attackBufferTimer = 0f;
    }
    public void SetInputLock(bool isLock)
    {
        isLocked = isLock;
    }
    private void ResetInput()
    {
        rightInput = false;
        leftInput = false;
        downInput = false;
        jumpBufferTimer = 0f;
        attackBufferTimer = 0f;
        ShiftPressed = false;
    }
}