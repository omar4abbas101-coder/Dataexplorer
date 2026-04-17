using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.15f;

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
        offset = Vector2.zero;
    }

    void Update()
    {
        offset.y -= scrollSpeed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}
