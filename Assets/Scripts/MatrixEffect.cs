using System.Collections.Generic;
using UnityEngine;

public class MatrixEffect : MonoBehaviour
{
    [Header("prefabs")]
    [SerializeField] List<Sprite> symbolTypes = new List<Sprite>();
    [SerializeField] GameObject symbolPrefab;

    [Header("settings")]
    [SerializeField] Vector2 speed = new Vector2(0.02f, 0.04f);
    [SerializeField] Vector2 intervals = new Vector2(0.05f, 0.2f);
    [SerializeField] float[] alphaVariations = { 0.3f, 0.5f, 0.75f };
    [SerializeField] float transparencyChance = 0.4f;
    [SerializeField] float symbolScale = 2f;
    [SerializeField] Color symbolColor = Color.green;

    [HideInInspector] public float speedUpMod = 1f;

    float nextInterval = 0;
    float t = 0;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        SpawnTimer();
    }

    void SpawnTimer()
    {
        t += Time.deltaTime * speedUpMod;

        if (t > nextInterval)
        {
            SpawnSymbol();
        }
    }

    void SpawnSymbol()
    {
        if (mainCam == null)
        {
            Debug.LogError("No Main Camera found. Make sure your camera has the tag MainCamera.");
            return;
        }

        if (symbolPrefab == null)
        {
            Debug.LogError("Symbol Prefab is missing!");
            return;
        }

        if (symbolTypes == null || symbolTypes.Count == 0)
        {
            Debug.LogError("No symbol sprites assigned!");
            return;
        }

        nextInterval = Random.Range(intervals.x, intervals.y);
        t = 0;

        float symbolSpeed = Random.Range(speed.x, speed.y) * speedUpMod;

        float camDistance = -mainCam.transform.position.z;

        Vector3 leftBottom = mainCam.ViewportToWorldPoint(new Vector3(0, 0, camDistance));
        Vector3 rightTop = mainCam.ViewportToWorldPoint(new Vector3(1, 1, camDistance));

        float symbolX = Random.Range(leftBottom.x, rightTop.x);
        float symbolY = rightTop.y + 1f;

        Vector2 symbolPos = new Vector2(symbolX, symbolY);

        float symbolAlpha = 1f;

        if (alphaVariations.Length > 0 && Random.value <= transparencyChance)
        {
            symbolAlpha = alphaVariations[Random.Range(0, alphaVariations.Length)];
        }

        Color finalColor = new Color(
            symbolColor.r,
            symbolColor.g,
            symbolColor.b,
            symbolAlpha
        );

        Sprite symbolSprite = symbolTypes[Random.Range(0, symbolTypes.Count)];

        GameObject newSymbol = Instantiate(symbolPrefab, symbolPos, Quaternion.identity);

        MatrixSymbol matrixSymbol = newSymbol.GetComponent<MatrixSymbol>();

        if (matrixSymbol == null)
        {
            Debug.LogError("MatrixSymbol component missing on symbol prefab!");
            Destroy(newSymbol);
            return;
        }

        matrixSymbol.InitSymbol(symbolSprite, finalColor, symbolScale, symbolSpeed);
    }
}