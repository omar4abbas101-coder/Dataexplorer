using UnityEngine;

public class LaserBlink : MonoBehaviour
{
    public GameObject laserColor1;
    public GameObject laserColor2;

    public float blinkInterval = 0.5f;

    private float timer;

    void Start()
    {
        laserColor1.SetActive(true);
        laserColor2.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            timer = 0f;

            bool isActive = laserColor1.activeSelf;

            laserColor1.SetActive(!isActive);
            laserColor2.SetActive(isActive);
        }
    }
}