using UnityEngine;
using System.Collections.Generic;

public class EnemyUIPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyUIPrefab;
    [SerializeField] private Transform enemyParent;

    private Queue<GameObject> pool = new Queue<GameObject>();

    public GameObject Prefab => enemyUIPrefab;
    public Transform Parent => enemyParent;

    public GameObject Get()
    {
        GameObject enemyObj;
        if (pool.Count > 0)
        {
            enemyObj = pool.Dequeue();
            enemyObj.SetActive(true);
        }
        else
        {
            enemyObj = Instantiate(enemyUIPrefab, enemyParent);
        }

        EnemyUIController ui = enemyObj.GetComponent<EnemyUIController>();
        if (ui != null)
            ui.Bind(); // Auto-bind to local Enemy

        return enemyObj;
    }

    public void Return(GameObject enemyObj)
    {
        if (enemyObj != null)
        {
            EnemyUIController ui = enemyObj.GetComponent<EnemyUIController>();
            if (ui != null)
                ui.Unbind();
            enemyObj.SetActive(false);
            pool.Enqueue(enemyObj);
        }
    }

    public void PreWarm(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0)
            {
                GameObject obj = Instantiate(enemyUIPrefab, enemyParent);
                EnemyUIController newUi = obj.GetComponent<EnemyUIController>();
                if (newUi != null)
                    newUi.Bind(); // Auto-bind for pre-warm
                Return(obj);
            }
        }
    }
}
