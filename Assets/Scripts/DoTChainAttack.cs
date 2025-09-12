using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Attacks/DoT Chain Attack")]
public class DoTChainAttack : Attack
{
  public float damagePerSecond;
  public float duration;
  public float chainRange;

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] allEnemies)
  {
    target.StartCoroutine(DamageOverTime(target));

    target.OnDeath += (deadEnemy) =>
    {
      foreach (var enemy in allEnemies)
      {
        if (enemy.IsDead) continue;
        float dist = Vector2.Distance(deadEnemy.RectTransform.anchoredPosition, enemy.RectTransform.anchoredPosition);
        if (dist <= chainRange)
          enemy.StartCoroutine(DamageOverTime(enemy));
      }
    };
  }

  private IEnumerator DamageOverTime(Enemy enemy)
  {
    float elapsed = 0f;
    while (elapsed < duration && !enemy.IsDead)
    {
      enemy.TakeDamage(damagePerSecond * Time.deltaTime);
      elapsed += Time.deltaTime;
      yield return null;
    }
  }
}
