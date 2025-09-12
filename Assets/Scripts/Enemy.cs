using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Enemy : MonoBehaviour
{
  [SerializeField] private float maxHealth = 100f;
  [SerializeField] private Slider healthBar;

  private float currentHealth;

  public event Action<Enemy> OnDeath;

  private void Awake()
  {
    currentHealth = maxHealth;
    UpdateUI();
  }

  public void TakeDamage(float amount)
  {
    currentHealth -= amount;
    if (currentHealth < 0) currentHealth = 0;

    UpdateUI();

    if (currentHealth <= 0)
      Die();
  }

  private void UpdateUI()
  {
    if (healthBar != null)
      healthBar.value = currentHealth / maxHealth;
  }

  private void Die()
  {
    OnDeath?.Invoke(this);
    gameObject.SetActive(false);
  }

  public bool IsDead => currentHealth <= 0;
  public RectTransform RectTransform => (RectTransform)transform;
}
