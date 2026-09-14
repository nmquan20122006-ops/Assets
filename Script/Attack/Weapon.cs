using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    private WeaponHitbox weaponHitbox;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private float knockBackForce = 8f;
    [SerializeField] private float knockBackDuration = 0.15f;
    [SerializeField] private float knockBackLift = 0.2f;
    public int Damage => damage;
    public float KnockBackDuration => knockBackDuration;
    public float KnockBackForce => knockBackForce;
    private float cooldownDuration => attackSpeed > 0 ? 1f / attackSpeed : 1f;
    public float KnockBackLift => knockBackLift;
    private void Awake()
    {
        weaponHitbox = GetComponentInChildren<WeaponHitbox>();
    }
    public bool CanAttack()
    {
        if (!canAttack) return false;
        canAttack = false;
        StartCoroutine(AttackCooldown());
        return true;
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(cooldownDuration);
        canAttack = true;
    }
    public void EnableHitbox()
    {
        weaponHitbox.gameObject.SetActive(true);
    }
    public void DisableHitbox()
    {
        weaponHitbox.gameObject.SetActive(false);
    }
    public void SetWeaponData(PlayerWeaponData data) 
    {
        damage = data.damage;
        attackSpeed = data.attackSpeed;
        knockBackDuration= data.knockBackDuration;
        knockBackForce= data.knockBackForce; 
        knockBackLift= data.knockBackLift;
    }
}
