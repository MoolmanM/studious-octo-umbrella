using UnityEngine;
using TMPro;
using System;

public class DebugUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private bool showDebug = true;

    private WaveSpawner waveSpawner;
    private EnemyManager enemyManager;

    private float updateInterval = 0.5f;
    private float lastUpdateTime = 0f;

    private void Awake()
    {
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        enemyManager = FindAnyObjectByType<EnemyManager>();

        if (debugText == null)
        {
            debugText = GetComponent<TextMeshProUGUI>();
        }

        if (waveSpawner == null || enemyManager == null)
        {
            Debug.LogWarning("DebugUI: WaveSpawner or EnemyManager not found. Debug info disabled.");
            showDebug = false;
        }
    }

    private void Update()
    {
        if (!showDebug || Time.time < lastUpdateTime + updateInterval) return;

        UpdateDebugText();
        lastUpdateTime = Time.time;
    }

    private void UpdateDebugText()
    {
        if (!showDebug || debugText == null) return;

        string info = "Debug Info:\n";

        if (waveSpawner != null)
        {
            info += $"Packs Cleared: {waveSpawner.TotalPacksCleared}\n";
            if (waveSpawner.CurrentPack != null)
            {
                info += $"Current Pack: {waveSpawner.CurrentPack.PackName} (Health x{waveSpawner.CurrentPack.HealthMultiplier:F1})\n";
                info += $"Spawned This Pack: {waveSpawner.CurrentPackSpawned} / {waveSpawner.CurrentPack.EnemyCountMin}-{waveSpawner.CurrentPack.EnemyCountMax}\n";
            }
            else
            {
                info += "Current Pack: None\n";
            }
        }

        if (enemyManager != null)
        {
            info += $"Active Enemies: {enemyManager.ActiveEnemiesCount}\n";
            var stats = enemyManager.GetEnemyStats();
            info += $"Enemy Levels: {stats}\n";
        }

        debugText.text = info;
    }
}
