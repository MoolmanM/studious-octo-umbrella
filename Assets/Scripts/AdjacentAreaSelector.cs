using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "Area Selectors/Adjacent")]
public class AdjacentAreaSelector : ScriptableObject, IAreaSelector
{
    public int maxAdjacent = 3;

    public Enemy[] GetEnemiesInArea(Enemy target, Enemy[] allEnemies)
    {
        // Sort by distance and take closest
        var sorted = allEnemies.OrderBy(e =>
            Vector3.Distance(target.transform.position, e.transform.position));
        return sorted.Take(maxAdjacent).ToArray();
    }
}
