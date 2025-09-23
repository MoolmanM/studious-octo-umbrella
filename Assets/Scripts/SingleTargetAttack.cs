using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Single Target Attack")]
public class SingleTargetAttack : Attack
{
    public float damage;

    public override void Execute(RectTransform attacker, Enemy target, Enemy[] inRange)
    {
        if (target != null && !target.IsDead)
        {
            target.TakeDamage(damage);
        }
    }
}
