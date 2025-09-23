using UnityEngine;

public abstract class Attack : ScriptableObject
{
    [SerializeField] private IAreaSelector areaSelector;
    [SerializeField] public float attackSpeed = 1f;
    [SerializeField] public float manaCost = 1f;

    public abstract void Execute(RectTransform attacker, Enemy target, Enemy[] inRange);

    public Enemy[] GetEnemiesInRange(Enemy target, Enemy[] allEnemies)
    {
        return areaSelector?.GetEnemiesInArea(target, allEnemies) ?? new Enemy[0];
    }
}
