using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDistanceManager : MonoBehaviour
{
  [SerializeField] private RectTransform player;
  [SerializeField] private List<Enemy> enemies;
  [SerializeField] private Button[] attackButtons;
  [SerializeField] private Attack[] attacks; // ScriptableObjects for each attack

  private void Start()
  {
    for (int i = 0; i < attackButtons.Length; i++)
    {
      int index = i; // local capture
      attackButtons[i].onClick.AddListener(() =>
      {
        Enemy target = GetClosestEnemy();
        if (target != null)
          attacks[index].Execute(player, target, enemies.ToArray());
      });
    }
  }

  private Enemy GetClosestEnemy()
  {
    Enemy closest = null;
    float minDist = float.MaxValue;

    foreach (var enemy in enemies)
    {
      if (enemy.IsDead) continue;

      float dist = Vector2.Distance(player.anchoredPosition, enemy.RectTransform.anchoredPosition);
      if (dist < minDist)
      {
        minDist = dist;
        closest = enemy;
      }
    }
    return closest;
  }
}
