using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

enum PackRarity { common, uncommon, rare, unique }

class Pack
{
    public PackRarity Rarity { get; }
    public int Size { get; }
    public int Cost { get; set; }
    public int Weight { get; set; }

    public Pack(PackRarity rarity, int size, int cost, int weight)
    {
        Rarity = rarity;
        Size = size;
        Cost = cost;
        Weight = weight;
    }

}

public abstract class Location : ScriptableObject
{
    // For now just create a method to generate random names for this string.
    [SerializeField] public string locationName = "";
    //[SerializeField] public string[] possibleNames = new string[] { "Dark Forest", "Mystic Caves", "Ancient Ruins", "Frozen Tundra", "Volcanic Wasteland" };
    [SerializeField] public int minEnemyLevel = 1;
    [SerializeField] public int maxEnemyLevel = 1;
    [SerializeField] public Enemy[] possibleEnemies;
    [SerializeField] public int maxSimultaneousEnemies = 10; // This will be determined by the enemyQuantity amongst other things.
    [SerializeField] public int maxPacks = 10; // This will be determined by the enemyQuantity amongst other things. 
    [SerializeField] public bool hasBoss = false;
    [SerializeField] public float enemyQuantity = 1f;
    [SerializeField] public Enemy bossEnemy; // Could do it like this to have a specific boss per location. or just generate a random one from a list.
    [SerializeField] public bool isCleared = false;
    [SerializeField] public int locationLevel = 1;

    //[SerializeField] public Rarity locationRarity;

    static System.Random rng = new System.Random();
    private List<Pack> TestSpawnPacks(int baseBudget = 100, float densityMod = 0.0f)
    {
        int budget = (int)(baseBudget * (1 + densityMod));

        var packs = new List<Pack>
        {
            new Pack(PackRarity.common, 1, 1, 70),
            new Pack(PackRarity.uncommon, 1, 2, 20),
            new Pack(PackRarity.rare, 1, 3, 10)
        };

        while (budget > 0)
        {
            // Weight random choice
            int totalWeight = packs.Sum(p => p.Weight);
            int roll = rng.Next(0, totalWeight);
            Pack chosen = null;

            foreach (var pack in packs)
            {
                if (roll < pack.Weight)
                {
                    chosen = pack;
                    break;
                }
                roll -= pack.Weight;
            }

            if (chosen.Cost <= budget)
            {
                packs.Add(chosen);
                budget -= chosen.Cost;
            }
            else
            {
                // Budget spent
                break;
            }
        }

        return packs;
    }
    // Spawn initial enemies when entering the area.
    // When all enemies are defeated, spawn boss.


    // After boss is defeated, area is cleared. Which takes the player back to some menu probably,
    // for but for now we will just enter the next area automatically
    void Start()
    {
        TestSpawnPacks(100, 0f);
    }
}
