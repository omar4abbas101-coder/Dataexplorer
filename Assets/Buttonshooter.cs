using UnityEngine;

public class Buttonshooter : MonoBehaviour
{
    public PlayerShooter playerShooter;

    public void OnShootButtonPressed()
    {
        if (playerShooter != null)
        {
            playerShooter.Shoot();
        }
    }
}