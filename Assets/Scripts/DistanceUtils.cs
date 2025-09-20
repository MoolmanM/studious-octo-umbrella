using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DistanceUtils
{
  public static Enemy GetClosestEnemy(Player player, List<Enemy> enemies)
  {
    Enemy closest = null;
    float minDist = float.MaxValue;

    foreach (var enemy in enemies)
    {
      if (enemy.IsDead) continue;

      // TODO: Should probably not use player and enemy class, instead just use their transforms.

      //Vector3 playerScreenPos = RectTransformUtility.WorldToScreenPoint(null, player.ectTransform);
      Vector3 enemyScreenPos = RectTransformUtility.WorldToScreenPoint(null, enemy.RectTransform.position);
      //float dist = Vector3.Distance(playerScreenPos, enemyScreenPos); 
      float dist = 1;

      if (dist < minDist)
      {
        minDist = dist;
        closest = enemy;
      }
    }
    return closest;
  }

  public static List<Enemy> GetEnemiesInEllipse(Enemy closest, float radiusx, float radiusy, List<Enemy> aliveEnemies)
  {
    List<Enemy> inRange = new List<Enemy>();
    Vector2 closestPos = closest.RectTransform.position;

    foreach (var enemy in aliveEnemies)
    {
      if (enemy.IsDead) continue;

      Vector2 targetPos = enemy.RectTransform.position;
      Vector2 halfSize = enemy.RectTransform.rect.size * 0.5f;

      Vector2 diff = closestPos - targetPos;
      Vector2 clamped = new Vector2(
          Mathf.Clamp(diff.x, -halfSize.x, halfSize.x),
          Mathf.Clamp(diff.y, -halfSize.y, halfSize.y)
      );

      // Nearest point on rect (in world space)
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
