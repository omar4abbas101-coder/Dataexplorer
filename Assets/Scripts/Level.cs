using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance;
    Vector3 bottomLeft;
    Vector3 topRight;

    private void Awake()
    {
        instance = this;

        DefineScreenCoords();
    }

    void DefineScreenCoords()
    {
        Camera camera = Camera.main;

        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane));
    }

    public float GetScreenTop() => topRight.y;
    public float GetScreenBottom() => bottomLeft.y;
    public float GetScreenLeft() => bottomLeft.x;
    public float GetScreenRight() => topRight.x;
}
