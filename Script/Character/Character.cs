using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isTakingDamage = false;
    [SerializeField] private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    virtual public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        isTakingDamage = true;
        if (currentHealth <= 0)
        {
            Die();
        }
        isTakingDamage = false;
    }
    protected void Die()
    {
        isDead = true;
        isTakingDamage = false;
    }

    public bool IsTakingDamage()
    {
        return isTakingDamage;
    }

    public bool IsDead()
    {
        return isDead;
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

}
