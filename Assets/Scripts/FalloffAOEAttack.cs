using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Falloff AOE Attack")]
public class FalloffAOEAttack : Attack
{
  public float maxDamage;
  public float minDamage;
  public float range;

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] inRange)
  {
    foreach (var enemy in inRange)
    {
      //float t = dist / range;
      //float dmg = Mathf.Lerp(maxDamage, minDamage, t);
      //enemy.TakeDamage(dmg);
    }
  }
}
