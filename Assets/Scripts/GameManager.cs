using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GameSettings")]
    public GameSettings settings;
    public GameDifficultyScrObj difficulty;

    [Header("Game State")]
    //[SerializeField] int startHP = 5;
    [SerializeField] string gameOverSceneName = "GameOver";
    [SerializeField] float invincibleSecondsAfterHit = 1f;
    [HideInInspector] public bool pause = true;
    [SerializeField] GameObject playerDeathEffect;
    [SerializeField] Transform playerTransform;

    [Header("UI")]
    public UIManager uiManager;

    [Header("Spawners")]
    public EnemySpawner enemySpawner;
    public SpawnHazard hazardSpawner;
    public LaserSpawner laserSpawner;
    public PowerUpManager powerUpManager;
    public BossSpawner bossSpawner;

    [Header("Waves")]
    public WaveManager waveManager;
    public WaveScrObj currentWave;

    [Header("refs")]
    public GameObject player;

    int score;
    public int hp;

    bool isGameOver;
    float pauseBeforeFinish = 3f;
    bool isInvincible;
    Coroutine invRoutine;

    public int Score => score;
    public int HP => hp;
    public bool IsGameOver => isGameOver;
    public bool IsInvincible => isInvincible;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        difficulty = settings.difficulty;
        Debug.Log("GameManager: Difficulty is set to: " + difficulty.name);
    }

    void Start()
    {
        score = 0;
        hp = 5;    
        //Here I (Omar Abbas) removed start hp and assigned manually the hp value
        isGameOver = false;
        isInvincible = false;

        RefreshUI();

        waveManager.waves = difficulty.waves;
        StartCoroutine(waveManager.NextWaveTransition());
    }

    public float GetScreenTop() => Level.instance.GetScreenTop();
    public float GetScreenBottom() => Level.instance.GetScreenBottom();
    public float GetScreenLeft() => Level.instance.GetScreenLeft();
    public float GetScreenRight() => Level.instance.GetScreenRight();

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        RefreshUI();
    }

    public void PlayerHit(int damage)
    {
        if (isGameOver) return;
        if (isInvincible) return;

        hp -= damage;
        if (hp < 0) hp = 0;

        RefreshUI();

        if (hp <= 0)
        {
            if (playerDeathEffect != null && playerTransform != null)
            {
                Instantiate(playerDeathEffect, playerTransform.position, Quaternion.identity);
            }

            if (playerTransform != null)
            {
                playerTransform.gameObject.SetActive(false);
            }

            StartCoroutine(DelayedGameOver());
            return;
        }

        SetInvincible(invincibleSecondsAfterHit);
    }

    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(1f);
        GameOver();
    }

    public void TakeDamage(int damage)
    {
        PlayerHit(damage);
    }

    public void SetInvincible(float seconds)
    {
        if (invRoutine != null)
            StopCoroutine(invRoutine);

        invRoutine = StartCoroutine(InvRoutine(seconds));
    }

    IEnumerator InvRoutine(float seconds)
    {
        isInvincible = true;
        yield return new WaitForSeconds(seconds);
        isInvincible = false;
        invRoutine = null;
    }

    void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        PlayerPrefs.SetInt("FinalScore", score);

        Time.timeScale = 1f;
        SceneLoader.instance.LoadScene(gameOverSceneName);
    }

    public void GameFinished()
    {
        StartCoroutine(GoToFinalScene());

        PlayerPrefs.SetInt("FinalScore", score);
        waveManager.waveText.gameObject.SetActive(true);
        waveManager.waveText.text = "Congratz! you won!";
    }

    IEnumerator GoToFinalScene()
    {
        yield return new WaitForSeconds(pauseBeforeFinish);
        SceneLoader.instance.LoadScene("Final");
    }

    void RefreshUI()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>(true);

        if (uiManager != null)
            uiManager.Refresh(score, hp);
    }
}