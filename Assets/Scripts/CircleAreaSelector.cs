using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Area Selectors/Circle")]
public class CircleAreaSelector : ScriptableObject, IAreaSelector
{
    public float range = 5f;

    public Enemy[] GetEnemiesInArea(Enemy target, Enemy[] allEnemies)
    {
        var inRange = DistanceUtils.GetEnemiesInCircle(target, range, allEnemies.ToList());
        return inRange.ToArray();
    }
}