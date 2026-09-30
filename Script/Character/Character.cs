using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class Character : MonoBehaviour, IHealable
{
    [SerializeField] private int _maxHealth=100;
    [HideInInspector] private int _currentHealth;
    [SerializeField] private bool _isTakingDamage = false;
    [SerializeField] private bool _isDead = false;
    public bool isTakingDamage => _isTakingDamage;
    public bool isDead => _isDead;
    public int currentHealth => _currentHealth;
    public int maxHealth => _maxHealth;
    public System.Action<int, int> eOnHealthChanged;
    protected virtual void Start()
    {
        eOnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }
    private void Awake()
    {
        _currentHealth = _maxHealth;
    }
    virtual public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        _isTakingDamage = true;
        eOnHealthChanged?.Invoke(_currentHealth, _maxHealth);
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

    public bool Heal(int amount)
    {
        if (_isDead || _currentHealth >= _maxHealth || amount <= 0) return false;
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        eOnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        return true;
    }

}
