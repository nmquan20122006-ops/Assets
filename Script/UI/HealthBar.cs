using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private UnityEngine.UI.Slider _healthSlider;
    private void OnEnable()
    {
        if (_character != null)
        {
            _character.eOnHealthChanged += UpdateUI;
            UpdateUI(_character.currentHealth, _character.maxHealth);
        }
    }
    private void OnDisable()
    {
        if (_character != null)
        {
            _character.eOnHealthChanged -= UpdateUI;
          
        }
    }
    private void UpdateUI(int currentHealth,int maxHealth)
    {
        if (_healthSlider == null) return;
        _healthSlider.value = (float)currentHealth / maxHealth;

    }
}
