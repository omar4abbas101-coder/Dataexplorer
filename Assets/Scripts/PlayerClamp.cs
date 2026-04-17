using UnityEngine;

public class PlayerClamp : MonoBehaviour
{
    [SerializeField] private float padding = 0.5f; // distance from screen edge

    Camera cam;
    float minX, maxX, minY, maxY;

    void Start()
    {
        cam = Camera.main;
        UpdateBounds();
    }

    void LateUpdate()
    {
        ClampPosition();
    }

    void UpdateBounds()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        minX = cam.transform.position.x - camWidth + padding;
        maxX = cam.transform.position.x + camWidth - padding;
        minY = cam.transform.position.y - camHeight + padding;
        maxY = cam.transform.position.y + camHeight - padding;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
