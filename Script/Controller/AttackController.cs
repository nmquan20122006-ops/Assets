using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement))]
public class AttackController : MonoBehaviour
{
    private Animator _animator;
    private WeaponManager _weaponManager;
    private PlayerInputManager _inputManager;
    private PlayerMovement _playerMovement;

    [SerializeField] private int _maxComboStep;
    [SerializeField] private float _comboTimeoutDuration;
    [SerializeField] private float _attackMoveMultiple;
    [SerializeField] private float _airAttackFallSpeed;

    private float _comboTimeoutTimer;
    private int _currentComboStep;

    [SerializeField] public float _weaponCooldownDuration;
    private float _cooldownTimer;
    public bool isAttacking { get; private set; }//importan bool
    [field:SerializeField] public bool isAirAttack { get; private set; }//very importan bool
    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _weaponManager = GetComponent<WeaponManager>();
        _inputManager = GetComponent<PlayerInputManager>();
    }
    private void Start()
    {
        _comboTimeoutDuration = _weaponManager.CurrentWeapon.comboTimeOutDuration;
        _weaponCooldownDuration = _weaponManager.CurrentWeapon.cooldownDuration;
    }
    private void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
        if (!isAttacking && _comboTimeoutTimer > 0)
        {
            _comboTimeoutTimer -= Time.deltaTime;
            if (_comboTimeoutTimer <= 0 && _currentComboStep > 0)
            {
                ResetCombo();
            }
        }
        if (_inputManager.attackRequested && CanAttack())
        {
            ExecuteAttack();
        }
        ApplyAirAttackFallSpeed();
        ApplyAttackMoveMultiple();
    }
    private bool CanAttack()
    {
        return !isAttacking && !_inputManager.isLocked && _cooldownTimer <= 0f;
    }
    private void ExecuteAttack()
    {
        if (!CanAttack()) return;
        UpdateComboStep();
        ApplyAttackState();
        TriggerAttackAnimation();
    }
    private void UpdateComboStep()
    {
        if (_currentComboStep > 0 && _comboTimeoutTimer > 0)
        {
            _currentComboStep = (_currentComboStep % _maxComboStep) + 1;
        }
        else if (_comboTimeoutTimer <= 0)
        {
            _currentComboStep = 1;
        }
        _comboTimeoutTimer = 0f;
    }
    private void ApplyAttackState()
    {
        isAttacking = true;
        isAirAttack = !_playerMovement.g_isGrounded;
        _inputManager.ConsumeAttackBuffer();
    }
    private void TriggerAttackAnimation()
    {
        _animator.SetInteger("ComboStep", _currentComboStep);
        _animator.SetTrigger("Attack");
    }
    public void ResetCombo()
    {
        _currentComboStep = 0;
        _comboTimeoutTimer = 0f;
        _animator.SetInteger("ComboStep", _currentComboStep);
        _animator.ResetTrigger("Attack");
    }
    public void AE_OnAttackEnd()
    {
        isAttacking = false;
        isAirAttack = false;
        if (_currentComboStep < _maxComboStep)
        {
            _comboTimeoutTimer = _comboTimeoutDuration;
        }
        else
        {
            ResetCombo();
        }
        _cooldownTimer = _weaponCooldownDuration;
    }
    public void AE_OnEnableHitbox()
    {
        _weaponManager.CurrentWeapon.EnableHitbox();
    }
    public void AE_OnDisableHitbox()
    {
        _weaponManager.CurrentWeapon.DisableHitbox();
    }
    public void Interrupt()
    {
        isAttacking = false;
        isAirAttack = false;
       
        ResetCombo();
    }
    private void OnEnable()
    {
        if (_playerMovement == null) return;
        _playerMovement._onJumpRequested += Interrupt;
    }
    private void OnDisable()
    {
        if (_playerMovement == null) return;
        _playerMovement._onJumpRequested -= Interrupt;
    }
    private void ApplyAirAttackFallSpeed()
    {
        if (isAirAttack)
        {
            _playerMovement.SetOverrideFallSpeed(_airAttackFallSpeed);
        }
        else
        {
            _playerMovement.SetOverrideFallSpeed(null);
        }
    }
    private void ApplyAttackMoveMultiple()
    {
        if (isAttacking)
        {
            _playerMovement.speedMultipler = _attackMoveMultiple;
        }
        else
        {
            _playerMovement.speedMultipler = 1f;
        }
    }
}