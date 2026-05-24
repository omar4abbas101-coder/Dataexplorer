using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("GameSettings")]
    public GameSettings settings;
    public GameDifficultyScrObj difficulty;

    [Header("Game State")]
    [SerializeField] int startHP = 3;
    [SerializeField] string gameOverSceneName = "GameOver";
    [SerializeField] float invincibleSecondsAfterHit = 1f;
    [HideInInspector] public bool pause = true;
    [SerializeField] GameObject playerDeathEffect;
    [SerializeField] Transform playerTransform;

    [Header("UI")]
    [SerializeField] UIManager uiManager;

    [Header("Level")]
    Vector3 bottomLeft;
    Vector3 topRight;

    [Header("Spawners")]
    public EnemySpawner enemySpawner;
    public SpawnHazard hazardSpawner;
    public LaserSpawner laserSpawner;
    public PowerUpManager powerUpManager;

    [Header("Waves")]
    public WaveManager waveManager;
    public WaveScrObj currentWave;

    int score;
    int hp;

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

        DefineScreenCoords();

        difficulty = settings.difficulty;
        Debug.Log("GameManager: Difficulty is set to: " + difficulty.name);
    }

    void Start()
    {
        score = 0;
        hp = startHP;
        isGameOver = false;
        isInvincible = false;

        RefreshUI();

        waveManager.waves = difficulty.waves;
        StartCoroutine(waveManager.NextWaveTransition());
    }

    void DefineScreenCoords()
    {
        Camera camera = Camera.main;

        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
    }

    public float GetScreenTop() => topRight.y;
    public float GetScreenBottom() => bottomLeft.y;
    public float GetScreenLeft() => bottomLeft.x;
    public float GetScreenRight() => topRight.x;

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