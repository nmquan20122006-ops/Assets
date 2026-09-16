using UnityEngine;
[RequireComponent(typeof(Animator))]
public class AttackController : MonoBehaviour
{
    private Animator _animator;
    private WeaponManager _weaponManager;
    private PlayerInputManager _inputManager;
    private PlayerMovement _playerMovement;
    [SerializeField] private int maxComboStep;
    [SerializeField] private float comboTimeoutDuration;
    private float comboTimeoutTimer;
    private int currentComboStep;
    private bool isAttacking;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _weaponManager = GetComponent<WeaponManager>();
        _inputManager = GetComponent<PlayerInputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (!isAttacking && comboTimeoutTimer > 0)
        {
            comboTimeoutTimer -= Time.deltaTime;
            if (comboTimeoutTimer <= 0 && currentComboStep > 0)
            {
                ResetCombo();
            }
        }
        if (_inputManager.attackRequested && CanAttack())
        {
            ExecuteAttack();
        }
    }
    private bool CanAttack()
    {
        return !isAttacking && !_inputManager.isLocked;
    }
    private void ExecuteAttack()
    {
        if (!CanAttack()) return;
        if (comboTimeoutTimer > 0)
        {
            currentComboStep = (currentComboStep % maxComboStep) + 1;
        }
        else if (comboTimeoutTimer <= 0)
        {
            currentComboStep = 1;
        }
        comboTimeoutTimer = 0f;
        isAttacking = true;
        _inputManager.ConsumeAttackBuffer();
        _animator.SetInteger("ComboStep", currentComboStep);
        _animator.SetTrigger("Attack");
    }
    public void ResetCombo()
    {
        currentComboStep = 0;
        comboTimeoutTimer = 0f;
        _animator.SetInteger("ComboStep", currentComboStep);
    }
    public void AE_OnAttackEnd()
    {
        isAttacking = false;
        comboTimeoutTimer = comboTimeoutDuration;
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
        ResetCombo(); 
    }
}