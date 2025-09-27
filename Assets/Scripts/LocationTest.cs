using UnityEngine;

public class LocationTest : MonoBehaviour
{
    [SerializeField] private float enemyQuantity = 1f;
    [SerializeField] private float enemyCommonQuantity = 1f;

    [ContextMenu("Test Common Packs Default")]
    private void TestCommonPacksDefault()
    {
        Debug.Log("Testing Common Packs with default values (enemyQuantity=1f, enemyCommonQuantity=1f)");
        Debug.Log("Location Level | Common Packs");
        Debug.Log("--------------|--------------");

        for (int locationLevel = 1; locationLevel <= 100; locationLevel++)
        {
            int commonPacks = Mathf.RoundToInt(1f * (1 + 1f * (locationLevel * 0.5f)));
            Debug.Log($"{locationLevel,13} | {commonPacks,12}");
        }
    }

    [ContextMenu("Test Common Packs Custom")]
    private void TestCommonPacksCustom()
    {
        Debug.Log($"Testing Common Packs with custom values (enemyQuantity={enemyQuantity}, enemyCommonQuantity={enemyCommonQuantity})");
        Debug.Log("Location Level | Common Packs");
        Debug.Log("--------------|--------------");

        for (int locationLevel = 1; locationLevel <= 100; locationLevel++)
        {
            int commonPacks = Mathf.RoundToInt(enemyQuantity * (1 + enemyCommonQuantity * (locationLevel * 0.5f)));
            Debug.Log($"{locationLevel,13} | {commonPacks,12}");
        }
    }

    [ContextMenu("Compare Different Values")]
    private void CompareDifferentValues()
    {
        Debug.Log("Comparing Common Packs for different enemyQuantity and enemyCommonQuantity values at Location Level 50");
        Debug.Log("enemyQuantity | enemyCommonQuantity | Common Packs");
        Debug.Log("--------------|---------------------|--------------");

        float[] quantities = { 0.5f, 1f, 1.5f, 2f };
        float[] commonQuantities = { 0.5f, 1f, 1.5f, 2f };

        foreach (float eq in quantities)
        {
            foreach (float ecq in commonQuantities)
            {
                int commonPacks = Mathf.RoundToInt(eq * (1 + ecq * (50 * 0.5f)));
                Debug.Log($"{eq,13} | {ecq,19} | {commonPacks,12}");
            }
        }
    }
}
