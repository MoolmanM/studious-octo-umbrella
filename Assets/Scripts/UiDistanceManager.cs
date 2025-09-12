using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDistanceManager : MonoBehaviour
{
  [SerializeField] private RectTransform player;
  [SerializeField] private List<Enemy> enemies;
  [SerializeField] private List<Button> attackButtons;
  [SerializeField] private List<AttackData> attackDatas;

  private void Start()
  {
    for (int i = 0; i < attackButtons.Count; i++)
    {
      int index = i;
      attackButtons[i].onClick.AddListener(() => AttackClosestEnemy(attackDatas[index]));
    }
  }

  private Enemy GetClosetEnemy()
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

  private void AttackClosestEnemy(AttackData attack)
  {
    Enemy target = GetClosetEnemy();
    if (target == null) return;

    foreach (var enemy in enemies)
    {
      if (enemy.IsDead) continue;

      float distToTarget = Vector2.Distance(
          target.RectTransform.anchoredPosition,
          enemy.RectTransform.anchoredPosition);

      if (distToTarget <= attack.range)
      {
        enemy.TakeDamage(attack.damage);
      }
    }
  }
}


