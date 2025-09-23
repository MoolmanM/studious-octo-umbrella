using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Area Selectors/Ellipse")]
public class EllipseAreaSelector : ScriptableObject, IAreaSelector
{
    public float rangeX = 5f;
    public float rangeY = 3f;

    public Enemy[] GetEnemiesInArea(Enemy target, Enemy[] allEnemies)
    {
        var inRange = DistanceUtils.GetEnemiesInEllipse(target, rangeX, rangeY, allEnemies.ToList());
        return inRange.ToArray();
    }
}