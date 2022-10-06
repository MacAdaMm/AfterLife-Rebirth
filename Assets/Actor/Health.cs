using System;
using UnityEngine;
using UnityEngine.Events;



public class Health : MonoBehaviour, IHittable, ISaveable
{
    [System.Serializable]
    public class HealthData
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }

        public HealthData(int current, int max)
        {
            CurrentHealth = current;
            MaxHealth = max;
        }
    }

    [SerializeField]
    private bool _disableOnDeath;

    [field: SerializeField]
    public int CurrentHealth { get; protected set; }
    [field: SerializeField]
    public int MaxHealth { get; protected set; }

    public UnityEvent OnHit;
    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    public bool IsDead { get; protected set; }

    public void Damage(int damage = 1)
    {
        int newHealthAmount = CurrentHealth - damage;
        CurrentHealth = Mathf.Max(0, newHealthAmount);
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
        }

        OnHit.Invoke();

        if (CurrentHealth == 0 && IsDead == false)
        {
            Kill();
        }
    }

    public void HealMax()
    {
        CurrentHealth = MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    public void Kill()
    {
        CurrentHealth = 0;
        IsDead = true;
        if (OnDeath != null)
        {
            OnDeath.Invoke();
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(CurrentHealth, MaxHealth);
        }
        if (_disableOnDeath)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            return;
        }

        CurrentHealth = MaxHealth;
    }

    public object CaptureState()
    {
        return new HealthData(CurrentHealth, MaxHealth);
    }

    public void RestoreState(object state)
    {
        var healthData = (HealthData)state;
        CurrentHealth = healthData.CurrentHealth;
        MaxHealth = healthData.MaxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}
