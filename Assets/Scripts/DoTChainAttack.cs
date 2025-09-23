using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Attacks/DoT Chain Attack")]
public class DoTChainAttack : Attack
{
  public float damagePerSecond;
  public float duration;

  private static HashSet<Enemy> activeDOTs = new HashSet<Enemy>();

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] inRange)
  {
    ApplyDOT(target, inRange);
  }

  private void ApplyDOT(Enemy enemy, Enemy[] inRange)
  {
    if (enemy == null || enemy.IsDead || activeDOTs.Contains(enemy))
      return;

    activeDOTs.Add(enemy);
    enemy.StartCoroutine(DamageOverTime(enemy, inRange));

    enemy.OnDeath += (deadEnemy) =>
    {
      enemy.OnDeath -= (d) => { };
      activeDOTs.Remove(enemy);
      ChainToOthers(deadEnemy, inRange);
    };
  }

  private void ChainToOthers(Enemy deadEnemy, Enemy[] inRange)
  {
    foreach (var other in inRange)
    {
      if (other != null && !other.IsDead && !activeDOTs.Contains(other))
      {
        ApplyDOT(other, inRange);
      }
    }
  }

  private IEnumerator DamageOverTime(Enemy enemy, Enemy[] inRange)
  {
    float elapsed = 0f;
    while (elapsed < duration && !enemy.IsDead)
    {
      if (enemy != null)
        enemy.TakeDamage(damagePerSecond * Time.deltaTime);
      elapsed += Time.deltaTime;
      yield return null;
    }

    if (enemy != null && !enemy.IsDead)
      activeDOTs.Remove(enemy);
  }
}
