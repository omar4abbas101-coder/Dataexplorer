using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBonus : MonoBehaviour
{
    [Header("refs")]
    [SerializeField] TextMeshProUGUI scoreText;

    [Header("params")]
    [SerializeField] float duration;
    [SerializeField] float blinkIntervals;
    Color bonusColor;
    

    public void InitScoreBonus(Color colorToBlink, int scoreAmount)
    {
        bonusColor = colorToBlink;
        scoreText.text = "+ " + scoreAmount.ToString();
        scoreText.color = bonusColor;

        StartCoroutine(ScoreAnim());
    }

    IEnumerator ScoreAnim()
    {
        float t = 0;
        float blinkT = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            blinkT += Time.deltaTime;

            if (blinkT > blinkIntervals)
            {
                blinkT = 0;
                scoreText.color = (scoreText.color == Color.white) ? bonusColor : Color.white;
            }

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
