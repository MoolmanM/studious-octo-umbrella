using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/AOE Attack")]
public class AOEAttack : Attack
{
  public float damage;

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] inRange)
  {
    foreach (var enemy in inRange)
    {
      enemy.TakeDamage(damage);
    }
  }
}
