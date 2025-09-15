using UnityEngine;
using System.Collections.Generic;

public class EnemyUIPool : MonoBehaviour
{
    [SerializeField] private GameObject enemyUIPrefab;
    [SerializeField] private Transform enemyParent;

    private Queue<EnemyUIController> pool = new Queue<EnemyUIController>();

    public EnemyUIController Get(Enemy enemyData)
    {
        EnemyUIController ui;
        if (pool.Count > 0)
        {
            ui = pool.Dequeue();
            ui.gameObject.SetActive(true);
        }
        else
        {
            GameObject obj = Instantiate(enemyUIPrefab, enemyParent);
            ui = obj.GetComponent<EnemyUIController>();
        }

        ui.Bind(enemyData);
        return ui;
    }

    public void Return(EnemyUIController ui)
    {
        ui.gameObject.SetActive(false);
        pool.Enqueue(ui);
    }
}
