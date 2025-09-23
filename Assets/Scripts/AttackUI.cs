using UnityEngine;
using UnityEngine.UI;

public class AttackUI : MonoBehaviour
{
    [SerializeField] private EnemyManager enemyManager;

    [Header("Buttons")]
    [SerializeField] private Button qButton;
    [SerializeField] private Button wButton;
    [SerializeField] private Button eButton;
    [SerializeField] private Button rButton;

    [Header("Toggles")]
    [SerializeField] private Toggle qToggle;
    [SerializeField] private Toggle wToggle;
    [SerializeField] private Toggle eToggle;
    [SerializeField] private Toggle rToggle;

    private void Awake()
    {
        if (enemyManager == null)
        {
            enemyManager = FindAnyObjectByType<EnemyManager>();
        }

        qButton.onClick.AddListener(() => enemyManager.ExecuteAttack(0));
        wButton.onClick.AddListener(() => enemyManager.ExecuteAttack(1));
        eButton.onClick.AddListener(() => enemyManager.ExecuteAttack(2));
        rButton.onClick.AddListener(() => enemyManager.ExecuteAttack(3));

        qToggle.onValueChanged.AddListener((isOn) => enemyManager.SetAutoAttack(0, isOn));
        wToggle.onValueChanged.AddListener((isOn) => enemyManager.SetAutoAttack(1, isOn));
        eToggle.onValueChanged.AddListener((isOn) => enemyManager.SetAutoAttack(2, isOn));
        rToggle.onValueChanged.AddListener((isOn) => enemyManager.SetAutoAttack(3, isOn));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            enemyManager.ExecuteAttack(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            enemyManager.ExecuteAttack(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            enemyManager.ExecuteAttack(2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            enemyManager.ExecuteAttack(3);
        }
    }
}
