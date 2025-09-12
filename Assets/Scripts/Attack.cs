using UnityEngine;

public abstract class Attack : ScriptableObject
{
  public string attackName;
  public abstract void Execute(RectTransform attacker, Enemy target, Enemy[] allEnemies);
}
