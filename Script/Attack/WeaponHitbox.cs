using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    private Weapon weapon;
    private void Awake()
    {
        weapon = GetComponentInParent<Weapon>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(weapon.Damage, transform.position, weapon.KnockBackLift, weapon.KnockBackForce, weapon.KnockBackDuration);
            }
        }
    }
}
