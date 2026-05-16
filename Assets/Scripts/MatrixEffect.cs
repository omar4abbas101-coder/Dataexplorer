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
    
    public float speedUpMod = 1; // for in between wave speed-ups
    float nextInterval = 0;
    float t = 0;

    private void Update()
    {
        SpawnTimer();
    }

    void SpawnTimer()
    {
        // timer
        t += Time.deltaTime * speedUpMod;

        if (t > nextInterval) SpawnSymbol();
    }

    void SpawnSymbol()
    {
        // DECINING ON NEXT INTERVAL
        // ====================================
        nextInterval = Random.Range(intervals.x, intervals.y);
        t = 0;

        // PREPARING VARIABLES FOR SPAWNING
        // ====================================
        // speed
        float symbolSpeed = Random.Range(speed.x, speed.y);
        // position
        float symbolX = Random.Range(GameManager.Instance.GetScreenLeft(), GameManager.Instance.GetScreenRight());
        float symbolY = GameManager.Instance.GetScreenTop() + 1f;
        Vector2 symbolPos = new Vector2(symbolX, symbolY);

        // transparency
        float symbolAlpha = (Random.value > transparencyChance) ? 1f : alphaVariations[Random.Range(0, alphaVariations.Length)];
        // color
        symbolColor = new Color(symbolColor.r, symbolColor.g, symbolColor.b, symbolAlpha);
        // sprite
        Sprite symbolSprite = symbolTypes[Random.Range(0, symbolTypes.Count)];

        // SPAWNING THE SYMBOL
        // ===============================
        GameObject newSymbol = Instantiate(symbolPrefab, symbolPos, Quaternion.identity);
        newSymbol.GetComponent<MatrixSymbol>().InitSymbol(symbolSprite, symbolColor, symbolScale, symbolSpeed);
    }
}
