using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Falloff AOE Attack")]
public class FalloffAOEAttack : Attack
{
  public float maxDamage;
  public float minDamage;
  public float range;

  public override void Execute(RectTransform attacker, Enemy target, Enemy[] allEnemies)
  {
    foreach (var enemy in allEnemies)
    {
      float dist = Vector2.Distance(target.RectTransform.anchoredPosition, enemy.RectTransform.anchoredPosition);
      if (dist <= range)
      {
        float t = dist / range;
        float dmg = Mathf.Lerp(maxDamage, minDamage, t);
        enemy.TakeDamage(dmg);
      }
    }
  }
}
