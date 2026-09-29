using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IHealable
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    [SerializeField] private bool _isTakingDamage = false;
    [SerializeField] private bool _isDead = false;
    public bool isTakingDamage => _isTakingDamage;
    public bool isDead => _isDead;
    public int currentHealth => _currentHealth;
    public int maxHealth => _maxHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    virtual public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _isTakingDamage = true;
        if (_currentHealth <= 0)
        {
            Die();
        }
        _isTakingDamage = false;
    }
    protected void Die()
    {
        _isDead = true;
        _isTakingDamage = false;
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }

}
