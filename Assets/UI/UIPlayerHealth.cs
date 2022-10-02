using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private RectTransform _maxHealthTransform;
    [SerializeField] private RectTransform _currentHealthTransform;
    [SerializeField] private float _spriteSize = 16f;

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        _currentHealthTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealth * _spriteSize);
        _maxHealthTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth * _spriteSize);
    }

    private void Start()
    {
        LevelManager.Current.PlayerHealth.OnHealthChanged += UpdateHealth;
        UpdateHealth(LevelManager.Current.PlayerHealth.CurrentHealth, LevelManager.Current.PlayerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        //LevelManager.Current.PlayerHealth.OnHealthChanged -= UpdateHealth;
    }
}
