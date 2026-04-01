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

    [Header("Magnet Reward")]
    [SerializeField] GameObject magnetPickupPrefab;
    [SerializeField] Vector2 spawnMin = new Vector2(-8f, -4f);
    [SerializeField] Vector2 spawnMax = new Vector2(8f, 4f);
    [SerializeField] int rewardMultiplierThreshold = 4;

    [Header("PowerUp Limit")]
    [SerializeField] int maxPowerUpsOnScreen = 2;
    [SerializeField] float powerUpSpawnCooldown = 5f;

    int comboCount = 0;
    float timer = 0f;
    bool magnetSpawnedThisChain = false;
    float nextAllowedPowerUpSpawnTime = 0f;

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
            bool spawned = SpawnMagnetPickupRandom();
            if (spawned)
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

    bool SpawnMagnetPickupRandom()
    {
        if (magnetPickupPrefab == null) return false;

        if (Time.time < nextAllowedPowerUpSpawnTime)
            return false;

        PowerUpPickup[] existingPowerUps = FindObjectsOfType<PowerUpPickup>();
        if (existingPowerUps.Length >= maxPowerUpsOnScreen)
            return false;

        Vector2 pos = new Vector2(
            Random.Range(spawnMin.x, spawnMax.x),
            Random.Range(spawnMin.y, spawnMax.y)
        );

        Instantiate(magnetPickupPrefab, pos, Quaternion.identity);

        nextAllowedPowerUpSpawnTime = Time.time + powerUpSpawnCooldown;
        return true;
    }
}