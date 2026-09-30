using UnityEngine;

public class PlayerLevelManager : MonoBehaviour
{
    [SerializeField] private int _currentLevel;
    [SerializeField] private long _currentExp;
    [SerializeField] private int _maxLevel;
    private bool _isLevelUp;
    public int currentLevel => _currentLevel;
    public long currentExp => _currentExp;
    public int maxLevel => _maxLevel;
    public System.Action<long, long> OnExpChanged;
    public System.Action<int> OnLevelUp;
    public System.Action OnMaxLevel;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AddExp(20);
        }
    }
    public void Initialized(int initLevel, int initExp, int maxLevel)
    {

    }

    public void AddExp(long amount)
    {
        if (amount <= 0 || _currentLevel >= maxLevel) return;
        int oldLevel = _currentLevel;
        _currentExp += amount;
        OnExpChanged?.Invoke(_currentExp, GetRequiredExp(currentLevel));

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
            OnLevelUp?.Invoke(currentLevel);
        }

    }
    public long GetRequiredExp(int level)
    {
        return 100 + (level - 1) * 50;
    }

}
