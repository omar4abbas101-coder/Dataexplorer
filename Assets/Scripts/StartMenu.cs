using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    [Header("start text")]
    [SerializeField] TextMeshProUGUI startText;
    [SerializeField] float textFadingSpeed;
    [SerializeField] float pauseWhenFull;
    public AudioManager audioManager;

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneLoader.instance.LoadScene("Mainmenu");
    }


    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void OnEnable()
    {
        StartCoroutine(TextAnimation());
    }

    IEnumerator TextAnimation()
    {
        Color color = startText.color;
        Color transparentColor = new Color(color.r, color.g, color.b, 0f);
        float t = 0;

        while (gameObject.activeSelf)
        {
            t += Time.deltaTime * textFadingSpeed;
            if (t > 1) { t = 1; textFadingSpeed *= -1; }
            if (t < 0) { t = 0; textFadingSpeed *= -1; yield return new WaitForSeconds(pauseWhenFull); }

            startText.color = Color.Lerp(color, transparentColor, t);
            yield return null;
        }
    }

private void Update()
{
    if (Input.GetMouseButtonUp(0))
    {
        AudioManager.instance.PlaySFX("startinggamesound");
        StartGame();
    }
}
}
