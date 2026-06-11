using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
    public void GoToCredits()
    {
       SceneLoader.instance.LoadScene("Credit");

    }
}