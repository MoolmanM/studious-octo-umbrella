using UnityEngine;

public abstract class Attack : ScriptableObject
{
  [SerializeField] private bool useRange = false;
  [SerializeField] public float rangeX;
  [SerializeField] public float rangeY;

  public abstract void Execute(RectTransform attacker, Enemy target, Enemy[] inRange);

  public bool HasRange => useRange;
}
