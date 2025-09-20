using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Enemy/Enemy Pack")]
public class EnemyPack : ScriptableObject
{
    [SerializeField] private string packName = "Normal";
    [SerializeField] private int enemyCountMin = 5;
    [SerializeField] private int enemyCountMax = 10;
    [SerializeField] private int levelMin = 1;
    [SerializeField] private int levelMax = 1;
    [SerializeField] private float spawnDelay = 0.1f;
    [SerializeField] private float healthMultiplier = 1f;

    public string PackName => packName;
    public int EnemyCountMax => enemyCountMax;
    public int EnemyCountMin => enemyCountMin;
    public int LevelMin => levelMin;
    public int LevelMax => levelMax;
    public float SpawnDelay => spawnDelay;
    public float HealthMultiplier => healthMultiplier;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemyPack[] packs;
    [SerializeField] private EnemyUIPool enemyPool;
    [SerializeField] private Transform enemyParent;

    private EnemyManager enemyManager;
    private bool isSpawning = false;
    private int totalPacksCleared = 0;
    private EnemyPack currentPack;
    private int currentPackSpawned = 0;

    public int TotalPacksCleared => totalPacksCleared;
    public EnemyPack CurrentPack => currentPack;
    public int CurrentPackSpawned => currentPackSpawned;

    private void Awake()
    {
        enemyPool = FindAnyObjectByType<EnemyUIPool>();
        enemyParent = GameObject.Find("EnemiesContainer")?.transform;
        enemyManager = FindAnyObjectByType<EnemyManager>();

        if (enemyManager != null)
        {
            enemyManager.OnAllEnemiesCleared += OnAllEnemiesCleared;
        }

        if (packs != null && packs.Length > 0 && !isSpawning)
        {
            StartNextPack();
        }
    }

    private void OnDestroy()
    {
        if (enemyManager != null)
        {
            enemyManager.OnAllEnemiesCleared -= OnAllEnemiesCleared;
        }
    }

    public void StartNextPack()
    {
        if (isSpawning || packs == null || packs.Length == 0) return;

        EnemyPack pack = packs[Random.Range(0, packs.Length)];
        currentPack = pack;
        currentPackSpawned = 0;
        StartCoroutine(SpawnPack(pack));
    }

    private IEnumerator SpawnPack(EnemyPack pack)
    {
        isSpawning = true;

        int enemyCount = Random.Range(pack.EnemyCountMin, pack.EnemyCountMax + 1);
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject fullObj = enemyPool.Get();
            Enemy enemy = fullObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                int baseLevel = Random.Range(pack.LevelMin, pack.LevelMax + 1);
                int level = baseLevel + (totalPacksCleared / 3); // Scale with total packs cleared
                enemy.Initialize(level, pack.HealthMultiplier);
                if (enemyManager != null)
                    enemyManager.SpawnEnemy(fullObj);
            }

            currentPackSpawned++;
            yield return new WaitForSeconds(pack.SpawnDelay);
        }

        isSpawning = false;
    }

    private void OnAllEnemiesCleared()
    {
        totalPacksCleared++;
        currentPack = null;
        currentPackSpawned = 0;
        StartNextPack();
    }
}
