using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float baseHealth = 100f;
    [SerializeField] private float maxHealth;
    [SerializeField] private int level = 1;

    private float currentHealth;

    public event Action<Enemy> OnDeath;
    public event Action<float> OnHealthChanged;

    public float BaseHealth => baseHealth;
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public int Level => level;

    private void Awake()
    {
        Initialize(1);
    }

    public void Initialize(int newLevel, float healthMultiplier = 1f)
    {
        level = newLevel;
        maxHealth = baseHealth * level * healthMultiplier;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(1f);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0.01f) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth / maxHealth);

        if (currentHealth <= 0)
            Die();
    }


    private void Die()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    public bool IsDead => currentHealth <= 0;
    public RectTransform RectTransform => (RectTransform)transform;
}
