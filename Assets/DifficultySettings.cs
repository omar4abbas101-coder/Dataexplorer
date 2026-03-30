using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

[System.Serializable]
public class DifficultyTuning
{
    [Header("Spawning")]
    public float hazardSpawnInterval = 1.5f;
    public float pickupSpawnInterval = 3f;

    [Header("Hazards")]
    public float hazardSpeedMultiplier = 1f;
    public int hazardScoreValue = 10;
}

public class DifficultySettings : MonoBehaviour
{
    private static DifficultySettings _instance;
    public static DifficultySettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DifficultySettings>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("DifficultySettings");
                    _instance = go.AddComponent<DifficultySettings>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private Difficulty currentDifficulty = Difficulty.Normal;

    [Header("Tunings")]
    public DifficultyTuning easy = new DifficultyTuning
    {
        hazardSpawnInterval = 2.0f,
        pickupSpawnInterval = 2.5f,
        hazardSpeedMultiplier = 0.8f,
        hazardScoreValue = 5
    };

    public DifficultyTuning normal = new DifficultyTuning
    {
        hazardSpawnInterval = 1.5f,
        pickupSpawnInterval = 3.0f,
        hazardSpeedMultiplier = 1.0f,
        hazardScoreValue = 10
    };

    public DifficultyTuning hard = new DifficultyTuning
    {
        hazardSpawnInterval = 1.0f,
        pickupSpawnInterval = 4.0f,
        hazardSpeedMultiplier = 1.25f,
        hazardScoreValue = 15
    };

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
    }

    public DifficultyTuning GetTuning()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy: return easy;
            case Difficulty.Hard: return hard;
            default: return normal;
        }
    }
}
