using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyUIPrefab;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Attack[] attacks;

    [SerializeField] private EnemyUIPool uiPool;

    private readonly List<EnemyUIController> activeUIs = new List<EnemyUIController>();

    public void SpawnEnemy(Enemy enemyData)
    {
        if (uiPool == null)
        {
            Debug.LogError("UI Pool is not assigned.");
            return;
        }
        var ui = uiPool.Get(enemyData);
        ui.OnEnemySelected += HandleEnemySelected;
        activeUIs.Add(ui);
    }

    public void ReturnEnemyUI(EnemyUIController ui)
    {
        if (ui == null) return;

        ui.OnEnemySelected -= HandleEnemySelected;
        activeUIs.Remove(ui);
        uiPool.Return(ui);
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

    private void OnDisable()
    {
        if (activeUIs.Count == 0) return;

        foreach (var ui in activeUIs.ToArray())
        {
            if (ui == null) continue;
            ui.OnEnemySelected -= HandleEnemySelected;
            if (uiPool != null)
                uiPool.Return(ui);
        }
    }
}
