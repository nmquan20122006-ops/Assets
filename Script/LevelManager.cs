using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel;
    [SerializeField] private long _currentExp;
    [SerializeField] private int _maxLevel;
    [SerializeField] private int _expCurve;
    [SerializeField] private bool _isLevelUp;

    public int currentLevel => _currentLevel;
    public long currentExp => _currentExp;
    public int maxLevel => _maxLevel;
    public int expCurve => _expCurve;

    public System.Action OnExpChanged;
    public System.Action OnLevelUp;
    public System.Action OnMaxLevel;

    private IExpCurveStratergy stratergy;
    public void Initialized(int initLevel, int initExp, int maxLevel) 
    { 

    }

    public void AddExp(long amount)
    {
        if (amount <= 0 || _currentLevel >= maxLevel) return;
        int oldLevel = _currentLevel;
        _currentExp += amount;
        OnExpChanged?.Invoke();

        while (currentExp >= GetRequiredExp(currentLevel + 1) && currentLevel < maxLevel) 
        {
            _currentExp -= GetRequiredExp(currentLevel + 1);
            _currentLevel++;
            
        }
        int levelGained = currentLevel - oldLevel;
        if (_currentLevel >= maxLevel)
        {
            _currentExp = 0;
            OnMaxLevel?.Invoke();
        }
        else
        {
            _isLevelUp = levelGained > 0;
            OnLevelUp?.Invoke();
        }

    }
    private long GetRequiredExp(int level)
    {
        return 100 + (level - 1) * 50;
    }
    public float GetProgressNormalized()
    {
        // tra vet mot ty le % 0f->1f de cho UI hien thi thanh exp
        return 0f;
    }
    private bool CanLevelUp()
    {
        //kiem tra xem co the level up hay khong
        return false;
    }

}
