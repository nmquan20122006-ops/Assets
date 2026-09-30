using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExpBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _lvText;
    private PlayerLevelManager _playerLevelManager;
    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _playerLevelManager = player.GetComponent<PlayerLevelManager>();
    }

    private void OnEnable()
    {
        if (_playerLevelManager != null)
        {
            _playerLevelManager.OnExpChanged += UpdateUI;
            UpdateUI(_playerLevelManager.currentExp, _playerLevelManager.GetRequiredExp(_playerLevelManager.currentLevel));
            _playerLevelManager.OnLevelUp += UpdateTextUI;
            UpdateTextUI(_playerLevelManager.currentLevel);
        }
    }
    private void OnDisable()
    {
        if (_playerLevelManager != null)
        {
            _playerLevelManager.OnExpChanged -= UpdateUI;
        }
    }
    private void UpdateUI(long currentExp,long requiredExpToLvUp)
    {
        if (_slider == null) return;
        _slider.value = (float)currentExp / requiredExpToLvUp;
    }
    private void UpdateTextUI(int currentLv)
    {
        if (_lvText == null) return;
        _lvText.text = currentLv.ToString();
    }
}
