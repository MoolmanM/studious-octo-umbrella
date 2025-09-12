using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/AOE Attack")]
public class AOEAttack : Attack
{
  public float damage;
  public float range;

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] allEnemies)
  {
    foreach (var enemy in allEnemies)
    {
      float dist = Vector2.Distance(target.RectTransform.anchoredPosition, enemy.RectTransform.anchoredPosition);
      if (dist <= range)
        enemy.TakeDamage(damage);
    }
  }
}
