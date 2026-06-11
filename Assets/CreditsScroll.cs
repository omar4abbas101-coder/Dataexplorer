using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 50f;
    [SerializeField] float endY = 700f;
    [SerializeField] string Startingscene = "Starting screen";

    RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.anchoredPosition +=
            Vector2.up * scrollSpeed * Time.deltaTime;

        if (rectTransform.anchoredPosition.y >= endY)
        {
            SceneManager.LoadScene(Startingscene);
        }
    }
}