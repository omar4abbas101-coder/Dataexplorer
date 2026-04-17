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

    [Header("Reward PowerUps")]
    [SerializeField] GameObject magnetPickupPrefab;
    [SerializeField] GameObject rapidFirePickupPrefab;
    [SerializeField] Vector2 spawnMin = new Vector2(-8f, -4f);
    [SerializeField] Vector2 spawnMax = new Vector2(8f, 4f);

    [Header("Reward Spawn Control")]
    [SerializeField] int firstRewardComboHits = 9;
    [SerializeField] int rewardEveryXHits = 9;
    [SerializeField] int maxPowerUpsOnScreen = 2;

    int comboCount = 0;
    float timer = 0f;

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

        if (comboCount >= firstRewardComboHits)
        {
            int hitsAfterFirst = comboCount - firstRewardComboHits;

            if (hitsAfterFirst % rewardEveryXHits == 0)
            {
                SpawnRewardPowerUpRandom();
            }
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

    void SpawnRewardPowerUpRandom()
    {
        PowerUpPickup[] existingPowerUps = FindObjectsOfType<PowerUpPickup>();
        if (existingPowerUps.Length >= maxPowerUpsOnScreen) return;

        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        GameObject prefabToSpawn = null;

        bool spawnMagnet = Random.value < 0.5f;

        if (spawnMagnet)
        {
            prefabToSpawn = magnetPickupPrefab != null ? magnetPickupPrefab : rapidFirePickupPrefab;
        }
        else
        {
            prefabToSpawn = rapidFirePickupPrefab != null ? rapidFirePickupPrefab : magnetPickupPrefab;
        }

        if (prefabToSpawn == null) return;

        Instantiate(prefabToSpawn, pos, Quaternion.identity);
    }
}