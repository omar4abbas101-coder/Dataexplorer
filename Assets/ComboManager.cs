using UnityEngine;
using TMPro;

public class ComboManager : MonoBehaviour
{
    public static ComboManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] TextMeshProUGUI comboText;

    [Header("Combo Settings")]
    [SerializeField] float comboResetTime = 2f;

    [Header("Multiplier Settings")]
    [SerializeField] int hitsPerMultiplierStep = 3;
    [SerializeField] int maxMultiplier = 5;

    [Header("Magnet Reward (spawns at combo x5)")]
    [SerializeField] GameObject magnetPickupPrefab;
    [SerializeField] Vector2 spawnMin = new Vector2(-8f, -4f);
    [SerializeField] Vector2 spawnMax = new Vector2(8f, 4f);
    [SerializeField] int rewardMultiplierThreshold = 5;

    int comboCount = 0;
    float timer = 0f;
    bool magnetSpawnedThisChain = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        UpdateUI();
    }

    void Update()
    {
        if (comboCount <= 0) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetCombo();
        }
    }

    public void RegisterHit()
    {
        comboCount++;
        timer = comboResetTime;

        int mult = GetMultiplier();
        if (!magnetSpawnedThisChain && mult >= rewardMultiplierThreshold)
        {
            SpawnMagnetPickupRandom();
            magnetSpawnedThisChain = true;
        }

        UpdateUI();
    }

    public int ApplyMultiplier(int baseScore)
    {
        return baseScore * GetMultiplier();
    }

    int GetMultiplier()
    {
        int mult = 1 + (comboCount / hitsPerMultiplierStep);
        return Mathf.Min(mult, maxMultiplier);
    }

    public void ResetCombo()
    {
        comboCount = 0;
        timer = 0f;
        magnetSpawnedThisChain = false;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (comboText == null) return;

        if (comboCount < 2)
        {
            comboText.text = "";
            return;
        }

        comboText.text = $"COMBO x{GetMultiplier()} ({comboCount})";
    }

    void SpawnMagnetPickupRandom()
    {
        if (magnetPickupPrefab == null) return;

        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        Instantiate(magnetPickupPrefab, pos, Quaternion.identity);
    }
}
