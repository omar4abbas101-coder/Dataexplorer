using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public void OpenTutorial()
    {
        SceneLoader.instance.LoadScene("Tutorial");
    }
}