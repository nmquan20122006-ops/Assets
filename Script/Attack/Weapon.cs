using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    private WeaponHitbox weaponHitbox;
    [SerializeField] private float attackSpeed = 1;
    [SerializeField] private float knockBackForce = 8f;
    [SerializeField] private float knockBackDuration = 0.15f;
    [SerializeField] private float knockBackLift = 0.2f;
    public int Damage => damage;
    public float KnockBackDuration => knockBackDuration;
    public float KnockBackForce => knockBackForce;
    public float cooldownDuration => attackSpeed > 0 ? 1f / attackSpeed : 1f;
    public float comboTimeOutDuration => cooldownDuration * 1.5f;
    public float KnockBackLift => knockBackLift;
    private void Awake()
    {
        weaponHitbox = GetComponentInChildren<WeaponHitbox>();
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
