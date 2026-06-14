using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] slides;
    public SceneLoader sceneLoader;

    int currentSlide = 0;

    void Start()
    {
        ShowSlide(currentSlide);
    }

    void ShowSlide(int index)
    {
        for (int i = 0; i < slides.Length; i++)
        {
            slides[i].SetActive(i == index);
        }
    }

    public void NextSlide()
    {
        if (currentSlide < slides.Length - 1)
        {
            currentSlide++;
            ShowSlide(currentSlide);
        }
        else
        {
            SceneLoader.instance.LoadScene("Mainmenu");
        }
    }

  public void PreviousSlide()
{
    if (currentSlide > 0)
    {
        currentSlide--;
        ShowSlide(currentSlide);
    }
    else
    {
        SceneLoader.instance.LoadScene("Mainmenu");
    }
}
}