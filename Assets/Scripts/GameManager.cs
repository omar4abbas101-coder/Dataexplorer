using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game State")]
    [SerializeField] int startHP = 3;
    [SerializeField] string gameOverSceneName = "GameOver";
    [SerializeField] float invincibleSecondsAfterHit = 1f;
    [HideInInspector] public bool pause = true;

    [Header("UI")]
    [SerializeField] UIManager uiManager;

    [Header("Level")]
    Vector3 bottomLeft;
    Vector3 topRight;

    [Header("Spawners")]
    public EnemySpawner enemySpawner;
    public SpawnHazard hazardSpawner;

    [Header("Waves")]
    public WaveManager waveManager;
    public WaveScrObj currentWave;

    int score;
    int hp;

    bool isGameOver;
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
    }

    void Start()
    {
        score = 0;
        hp = startHP;
        isGameOver = false;
        isInvincible = false;

        RefreshUI();

        // Launching first wave
        StartCoroutine(waveManager.NextWaveTransition());
    }
    
    // ==============================
    // SCREEN POINTS
    // ==============================

    // getting screen border points in world coordinates
    void DefineScreenCoords()
    {
        // getting screen border coords
        Camera camera = Camera.main;

        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
    }

    // GETTING SCREEN POINTS
    public float GetScreenTop() => topRight.y;
    public float GetScreenBottom() => bottomLeft.y;
    public float GetScreenLeft() => bottomLeft.x;
    public float GetScreenRight() => topRight.x;

    // ==============================

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
            GameOver();
            return;
        }

        SetInvincible(invincibleSecondsAfterHit);
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

    // Save score
    PlayerPrefs.SetInt("FinalScore", score);

    Time.timeScale = 1f;
    SceneManager.LoadScene(gameOverSceneName);
}

    public void GameFinished()
    {
        // text saying you have won the game and displaying current score

        // temporary solution
        waveManager.waveText.gameObject.SetActive(true);
        waveManager.waveText.text = "Congratz! you won!";
    }

    void RefreshUI()
    {
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>(true);

        if (uiManager != null)
            uiManager.Refresh(score, hp);
    }
}