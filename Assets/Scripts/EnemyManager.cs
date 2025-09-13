using UnityEngine;

public class EnemyManager : MonoBehaviour
{
  [SerializeField] private GameObject enemyUIPrefab;
  [SerializeField] private Transform enemyParent;
  [SerializeField] private Attack[] attacks;

  public void SpawnEnemy(Enemy enemyData)
  {
    var ui = uiPool.Get(enemyData);
    ui.OnEnemySelected += HandleEnemySelected;
  }

  private void HandleEnemySelected(Enemy enemy)
  {
    Enemy[] inRange = GetEnemiesInRange(enemy);

    attacks[0].Execute(player, enemy, inRange);
  }

  private Enemy[] GetEnemiesInRange(Enemy target)
  {
    return Array.Empty<Enemy>();
  }
}
