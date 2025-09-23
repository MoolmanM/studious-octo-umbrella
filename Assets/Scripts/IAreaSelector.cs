using UnityEngine;

public interface IAreaSelector
{
    Enemy[] GetEnemiesInArea(Enemy target, Enemy[] allEnemies);
}