using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    protected int maxHealth = 3;

    [field: SerializeField, ReadOnly]
    public int CurrentHealth { get; protected set; }

    public event Action<int> OnHealthChanged;
    public event Action OnDeath;

    protected bool _isDead;

    [Button]
    public void Damage(int damage = 1)
    {
        int newHealthAmount = CurrentHealth - damage;
        CurrentHealth = Mathf.Max(0, newHealthAmount);
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(CurrentHealth);
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
            OnHealthChanged.Invoke(CurrentHealth);
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
