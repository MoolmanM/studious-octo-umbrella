using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/DoT Chain Attack")]
public class DoTChainAttack : Attack
{
  public float damagePerSecond;
  public float duration;
  public float chainRange;

  private HashSet<Enemy> activeDOTs = new HashSet<Enemy>();

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] allEnemies)
  {
    ApplyDOT(target, allEnemies);
  }

  private void ApplyDOT(Enemy enemy, Enemy[] allEnemies)
  {
    if (enemy == null || enemy.IsDead || activeDOTs.Contains(enemy))
      return;

    activeDOTs.Add(enemy);
    enemy.StartCoroutine(DamageOverTime(enemy));

    void HandleDeath(Enemy deadEnemy)
    {
      enemy.OnDeath -= HandleDeath;
      activeDOTs.Remove(enemy);

      foreach (var other in allEnemies)
      {
        if (other.IsDead || activeDOTs.Contains(other)) continue;

        float dist = Vector2.Distance(deadEnemy.RectTransform.anchoredPosition, other.RectTransform.anchoredPosition);
        if (dist <= chainRange)
        {
          ApplyDOT(other, allEnemies);
        }
      }
    }

    enemy.OnDeath += HandleDeath;
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

    if (!enemy.IsDead)
      activeDOTs.Remove(enemy);
  }
}
