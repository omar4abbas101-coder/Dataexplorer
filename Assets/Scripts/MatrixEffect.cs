using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatrixEffect : MonoBehaviour
{
    [Header("prefabs")]
    [SerializeField] List<Sprite> symbolTypes = new List<Sprite>();
    [SerializeField] GameObject symbolPrefab;

    [Header("settings")]
    [SerializeField] Vector2 speed;
    [SerializeField] Vector2 intervals;
    [SerializeField] float[] alphaVariations;
    [SerializeField] float transparencyChance;
    [SerializeField] float symbolScale;
    [SerializeField] Color symbolColor;

    [HideInInspector] public float speedUpMod = 1f;

    float nextInterval = 0;
    float t = 0;

    private void Update()
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
        nextInterval = Random.Range(intervals.x, intervals.y);
        t = 0;

        // THIS makes the letters fall faster too
        float symbolSpeed = Random.Range(speed.x, speed.y) * speedUpMod;

        float symbolX = Random.Range(
            GameManager.Instance.GetScreenLeft(),
            GameManager.Instance.GetScreenRight()
        );

        float symbolY = GameManager.Instance.GetScreenTop() + 1f;

        Vector2 symbolPos = new Vector2(symbolX, symbolY);

        float symbolAlpha = (Random.value > transparencyChance)
            ? 1f
            : alphaVariations[Random.Range(0, alphaVariations.Length)];

        Color finalColor = new Color(
            symbolColor.r,
            symbolColor.g,
            symbolColor.b,
            symbolAlpha
        );

        Sprite symbolSprite = symbolTypes[Random.Range(0, symbolTypes.Count)];

        GameObject newSymbol = Instantiate(
            symbolPrefab,
            symbolPos,
            Quaternion.identity
        );

        MatrixSymbol matrixSymbol = newSymbol.GetComponent<MatrixSymbol>();

        if (matrixSymbol == null)
        {
            Debug.LogError("MatrixSymbol component missing on symbol prefab!");
            return;
        }

        matrixSymbol.InitSymbol(
            symbolSprite,
            finalColor,
            symbolScale,
            symbolSpeed
        );
    }
}