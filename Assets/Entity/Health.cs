using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    protected int maxHealth = 3;

    [field: SerializeField]
    public int CurrentHealth { get; protected set; }
    public int MaxHealth => maxHealth;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    protected bool _isDead;

    public void Damage(int damage = 1)
    {
        int newHealthAmount = CurrentHealth - damage;
        CurrentHealth = Mathf.Max(0, newHealthAmount);
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(CurrentHealth, maxHealth);
        }

        if (CurrentHealth == 0 && _isDead == false)
        {
            Kill();
        }
    }

    public void Kill()
    {
        CurrentHealth = 0;
        _isDead = true;
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(CurrentHealth, maxHealth);
        }
    }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }

        CurrentHealth = maxHealth;
    }
}
