using System;
using Unity.Burst.Intrinsics;
using UnityEngine;

public abstract class Location : ScriptableObject
{
    // For now just create a method to generate random names for this string.
    [SerializeField] public string locationName = "";
    [SerializeField] public string[] possibleNames = new string[] { "Dark Forest", "Mystic Caves", "Ancient Ruins", "Frozen Tundra", "Volcanic Wasteland" };
    [SerializeField] public int minEnemyLevel = 1;
    [SerializeField] public int maxEnemyLevel = 1;
    [SerializeField] public Enemy[] possibleEnemies;
    [SerializeField] public int maxSimultaneousEnemies = 10; // This will be determined by the enemyQuantity amongst other things.
    [SerializeField] public int maxPacks = 10; // This will be determined by the enemyQuantity amongst other things. 
    [SerializeField] public bool hasBoss = false;
    [SerializeField] public float enemyQuantity = 1f;
    [SerializeField] public float enemyCommonQuantity = 1f;
    [SerializeField] public float enemyUncommonQuantity = 1f;
    [SerializeField] public float enemyRareQuantity = 1f; // Probably capping this at 1? Not sure what quant will do here.
    [SerializeField] public int commonPacks = 1;
    [SerializeField] public int uncommonPacks = 1;
    [SerializeField] public int rarePacks = 1;
    [SerializeField] public float enemyRarity = 1f; // common, rare, epic, legendary, etc. 
    [SerializeField] public Enemy bossEnemy; // Could do it like this to have a specific boss per location. or just generate a random one from a list.
    [SerializeField] public bool isCleared = false;
    [SerializeField] public int locationLevel = 1;

    //[SerializeField] public Rarity locationRarity;

    public void GenerateRandomName()
    {
        if (possibleNames.Length > 0)
        {
            locationName = possibleNames[UnityEngine.Random.Range(0, possibleNames.Length)];
        }
    }

    public void CalculateMaxEnemiesAndPacks()
    {
        maxSimultaneousEnemies = Mathf.RoundToInt(enemyQuantity * locationLevel * 2f);
        maxPacks = Mathf.RoundToInt(enemyQuantity * locationLevel * 0.5f);

        // Determine amount of packs for each rarity
        commonPacks = Mathf.RoundToInt(enemyQuantity * (1 + enemyCommonQuantity * (locationLevel * 0.5f)));
    }

    // Spawn initial enemies when entering the area.
    // When all enemies are defeated, spawn boss.


    // After boss is defeated, area is cleared. Which takes the player back to some menu probably,
    // for but for now we will just enter the next area automatically
}
