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
        List<Enemy> inRange = new List<Enemy>();

        if (attacks[index].HasRange && target != null)
        {
          inRange = GetEnemiesInRange(attacks[index].rangeX, attacks[index].rangeY);
        }

        if (target != null)
          attacks[index].Execute(player, target, inRange.ToArray());
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

      Vector3 playerScreenPos = RectTransformUtility.WorldToScreenPoint(null, player.position);
      Vector3 enemyScreenPos = RectTransformUtility.WorldToScreenPoint(null, enemy.RectTransform.position);
      float dist = Vector3.Distance(playerScreenPos, enemyScreenPos);

      if (dist < minDist)
      {
        minDist = dist;
        closest = enemy;
      }
    }
    return closest;
  }

  private List<Enemy> GetEnemiesInRange(float radiusx, float radiusy)
  {
    List<Enemy> inRange = new List<Enemy>();

    Enemy closest = GetClosestEnemy();
    Vector2 closestPos = closest.RectTransform.position;

    foreach (var enemy in enemies)
    {
      if (enemy.IsDead) continue;

      Vector2 targetPos = enemy.RectTransform.position;
      Vector2 halfSize = enemy.RectTransform.rect.size * 0.5f;

      Vector2 diff = closestPos - targetPos;
      Vector2 clamped = new Vector2(
          Mathf.Clamp(diff.x, -halfSize.x, halfSize.x),
          Mathf.Clamp(diff.y, -halfSize.y, halfSize.y)
      );

      // Nearest oint on rect (in world spave)
      Vector2 nearestPoint = targetPos + clamped;

      // Convert into ellipse
      float dx = (nearestPoint.x - closestPos.x) / radiusx;
      float dy = (nearestPoint.y - closestPos.y) / radiusy;
      float ellipseDist = dx * dx + dy * dy;

      if (ellipseDist <= 1f)
      {
        inRange.Add(enemy);
      }
    }
    return inRange;
  }
}
