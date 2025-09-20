using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int testSpawnCount = 5;
    [SerializeField] private Player player;
    [SerializeField] private Attack selectedAttack;

    [SerializeField] private bool autoAttackEnabled = true;
    [SerializeField] private float attackInterval = 2f;

    [SerializeField] private EnemyUIPool uiPool;
    [SerializeField] private int initialPoolSize = 100;

    private readonly List<Enemy> activeEnemies = new List<Enemy>();
    private readonly List<EnemyUIController> activeUIs = new List<EnemyUIController>();

    public event Action OnAllEnemiesCleared;

    public int ActiveEnemiesCount => activeEnemies.Count;

    public string GetEnemyStats()
    {
        if (activeEnemies.Count == 0) return "None";

        var aliveEnemies = activeEnemies.Where(e => !e.IsDead).ToList();
        if (aliveEnemies.Count == 0) return "None";

        int minLevel = aliveEnemies.Min(e => e.Level);
        int maxLevel = aliveEnemies.Max(e => e.Level);
        float avgLevel = (float)aliveEnemies.Average(e => e.Level);

        return $"Avg: {avgLevel:F1} (Range: {minLevel}-{maxLevel})";
    }

    private float lastAttackTime = 0f;

    private void Awake()
    {
        if (uiPool != null)
        {
            uiPool.PreWarm(initialPoolSize);
        }
        else
        {
            Debug.LogError("UI Pool is not assigned.");
        }

        if (autoAttackEnabled)
        {
            StartCoroutine(AutoAttackLoop());
        }
    }

    private IEnumerator AutoAttackLoop()
    {
        while (true)
        {
            if (activeEnemies.Count > 0 && selectedAttack != null && player != null)
            {
                var aliveEnemies = activeEnemies.Where(e => !e.IsDead).ToList();
                if (aliveEnemies.Count > 0)
                {
                    Enemy target = DistanceUtils.GetClosestEnemy(player, aliveEnemies);
                    if (target != null && Time.time > lastAttackTime + attackInterval)
                    {
                        Enemy[] inRange = GetEnemiesInRange(target);
                        selectedAttack.Execute(player.RectTransform, target, inRange);
                        lastAttackTime = Time.time;
                    }
                }
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }

    public void SpawnEnemy(GameObject objEnemy)
    {
        if (uiPool == null || objEnemy == null)
        {
            Debug.LogError("UI Pool or full enemy object is not assigned.");
            return;
        }

        Enemy enemy = objEnemy.GetComponent<Enemy>();
        if (enemy == null)
        {
            Debug.LogError("Full enemy object missing Enemy component.");
            return;
        }

        activeEnemies.Add(enemy);

        EnemyUIController ui = objEnemy.GetComponent<EnemyUIController>();
        if (ui == null)
        {
            Debug.LogError("Full enemy object missing EnemyUIController component.");
            return;
        }

        ui.OnEnemySelected += HandleEnemySelected;
        activeUIs.Add(ui);

        // Subscribe to death for cleanup
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(Enemy deadEnemy)
    {
        activeEnemies.Remove(deadEnemy);

        if (activeEnemies.Count == 0)
        {
            OnAllEnemiesCleared?.Invoke();
        }

        // Find the full object for this enemy
        var objEnemy = activeUIs.Select(ui => ui.gameObject).FirstOrDefault(obj => obj.GetComponent<Enemy>() == deadEnemy);
        if (objEnemy != null)
        {
            EnemyUIController ui = objEnemy.GetComponent<EnemyUIController>();
            if (ui != null)
                ui.OnEnemySelected -= HandleEnemySelected;
            activeUIs.Remove(ui);
            uiPool.Return(objEnemy);
        }
    }

    public void ReturnEnemyUI(GameObject objEnemy)
    {
        if (objEnemy == null) return;

        EnemyUIController ui = objEnemy.GetComponent<EnemyUIController>();
        if (ui == null) return;

        // Unsubscribe and remove
        ui.OnEnemySelected -= HandleEnemySelected;
        activeUIs.Remove(ui);

        Enemy boundEnemy = objEnemy.GetComponent<Enemy>();
        if (boundEnemy != null && !boundEnemy.IsDead)
        {
            activeEnemies.Remove(boundEnemy);
        }

        uiPool.Return(objEnemy);
    }

    private void HandleEnemySelected(Enemy enemy)
    {
        if (selectedAttack == null || player == null)
        {
            Debug.LogWarning("Selected attack or player not assigned.");
            return;
        }

        Enemy[] inRange = GetEnemiesInRange(enemy);
        selectedAttack.Execute(player.RectTransform, enemy, inRange);
    }

    private Enemy[] GetEnemiesInRange(Enemy target)
    {
        if (target == null || selectedAttack == null || !selectedAttack.HasRange)
            return new Enemy[0];

        var aliveEnemies = activeEnemies.Where(e => !e.IsDead).ToList();
        var inRangeList = DistanceUtils.GetEnemiesInEllipse(target, selectedAttack.rangeX, selectedAttack.rangeY, aliveEnemies);
        return inRangeList.ToArray();
    }

    [ContextMenu("Test Spawn")]
    public void TestSpawn()
    {
        if (uiPool == null)
        {
            Debug.LogError("UI Pool not assigned.");
            return;
        }

        for (int i = 0; i < testSpawnCount; i++)
        {
            GameObject enemyObj = uiPool.Get();
            SpawnEnemy(enemyObj);
        }
    }

    private void OnDisable()
    {
        // Clean up active enemies
        foreach (var enemy in activeEnemies.ToArray())
        {
            if (enemy != null)
                enemy.OnDeath -= HandleEnemyDeath;
        }
        activeEnemies.Clear();

        if (activeUIs.Count == 0) return;

        foreach (var ui in activeUIs.ToArray())
        {
            if (ui == null) continue;
            ui.OnEnemySelected -= HandleEnemySelected;
            if (uiPool != null)
                uiPool.Return(ui.gameObject);
        }
        activeUIs.Clear();
    }
}
