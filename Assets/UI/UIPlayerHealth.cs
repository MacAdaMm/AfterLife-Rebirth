using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerHealth : MonoBehaviour
{
    [SerializeField] private RectTransform maxHealthTransform;
    [SerializeField] private RectTransform currentHealthTransform;
    [SerializeField] private float spriteSize = 16f;

    private void UpdateHealth(int currentHealth, int maxHealth)
    {
        currentHealthTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, currentHealth * spriteSize);
        maxHealthTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxHealth * spriteSize);
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
