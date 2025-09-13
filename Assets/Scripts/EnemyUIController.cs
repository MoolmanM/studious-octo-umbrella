using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyUIController : MonoBehaviour
{
  [SerializeField] private Slider healthSlider;
  [SerializeField] private Button attackButton;

  private Enemy boundEnemy;

  public event Action<Enemy> OnEnemySelected;

  public void Bind(Enemy enemy)
  {
    boundEnemy = enemy;

    attackButton.onClick.RemoveAllListeners();
    attackButton.onClick.AddListener(() => OnEnemySelected?.Invoke(boundEnemy));
  }
}
