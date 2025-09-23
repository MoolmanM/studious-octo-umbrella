using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyUIController : MonoBehaviour
{
  [SerializeField] private Slider healthSlider;
  [SerializeField] private Button attackButton;

  private Enemy boundEnemy;

  public event Action<Enemy> OnEnemySelected;

  public void Bind(Enemy enemy = null)
  {
    if (boundEnemy != null)
      Unbind();

    if (enemy != null)
      boundEnemy = enemy;
    else
      boundEnemy = GetComponent<Enemy>(); // Auto-bind to local Enemy if no parameter

    if (boundEnemy == null)
    {
      Debug.LogWarning("No Enemy to bind to.");
      return;
    }

    if (healthSlider != null)
    {
      healthSlider.value = boundEnemy.CurrentHealth / boundEnemy.MaxHealth;
      boundEnemy.OnHealthChanged += UpdateHealthBar;
    }

    boundEnemy.OnDeath += HandleDeath;

    attackButton.onClick.RemoveAllListeners();
    attackButton.onClick.AddListener(() => OnEnemySelected?.Invoke(boundEnemy));
  }

  public void Unbind()
  {
    if (boundEnemy != null)
    {
      boundEnemy.OnHealthChanged -= UpdateHealthBar;
      boundEnemy.OnDeath -= HandleDeath;
      boundEnemy = null;
    }

    attackButton.onClick.RemoveAllListeners();
  }

  private void UpdateHealthBar(float percentage)
  {
    if (healthSlider != null)
      healthSlider.value = percentage;
  }

  private void HandleDeath(Enemy deadEnemy)
  {
    if (deadEnemy == boundEnemy)
    {
      Unbind();
    }
  }

  public Enemy GetBoundEnemy()
  {
    return boundEnemy;
  }
}
