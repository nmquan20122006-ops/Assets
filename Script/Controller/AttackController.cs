using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackController : MonoBehaviour
{
    private Animator animator;
    public WeaponManager weaponManager {  get; private set; }
    [SerializeField] private int comboStep = 0;
    [SerializeField] private bool isWindowOpen = false;
    [SerializeField] private bool hasQueuedNextAttack = false;
    private readonly int comboStepHash = Animator.StringToHash("ComboStep");
    private readonly int attackTriggerHash = Animator.StringToHash("Attack");
    void Start()
    {
        animator = GetComponent<Animator>();
        weaponManager = GetComponent<WeaponManager>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleAttackInput();
        }
    }

    public void EnableHitbox()
    {
        weaponManager.CurrentWeapon.EnableHitbox();
    }
    public void DisableHitbox()
    {
        weaponManager.CurrentWeapon.DisableHitbox();
    }
    private void HandleAttackInput()
    {
        if (weaponManager == null || weaponManager.CurrentWeapon == null) return;
        if (comboStep == 0)
        {
            if (weaponManager.CurrentWeapon.CanAttack())
            {
                ExecuteAttack(1);
            }
        }else if (isWindowOpen)
        {
            hasQueuedNextAttack = true;
            isWindowOpen = false;
        }
    }
    private void ExecuteAttack(int step)
    {
        comboStep = step;
        hasQueuedNextAttack = false;
        isWindowOpen = false;
        
        animator.SetInteger(comboStepHash,comboStep);animator.SetTrigger(attackTriggerHash);
    }
    public void OpenComboWindow()
    {
        isWindowOpen = true;
    }
    public void CheckNextCombo()
    {
        if (hasQueuedNextAttack && comboStep < 3)
        {
            ExecuteAttack(comboStep+1);
        }
        else
        {
            ResetCombo();
        }
    }

    public void ResetCombo()
    {
        comboStep = 0;
        isWindowOpen = false;
        hasQueuedNextAttack = false;
        animator.SetInteger(comboStepHash,0);
    }
}