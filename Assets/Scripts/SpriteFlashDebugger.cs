using UnityEngine;

public class SpriteFlashDebugger : MonoBehaviour
{
    public KeyCode testKey = KeyCode.T;

    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Debug.Log($"[SpriteFlashDebugger] On '{name}' sr={(sr ? "FOUND" : "MISSING")}  path={GetPath(transform)}");
    }

    void Update()
    {
        if (sr == null) return;

        if (Input.GetKeyDown(testKey))
        {
            sr.color = Color.red;
            Debug.Log($"[SpriteFlashDebugger] Set RED on '{name}'. sr.color now={sr.color}");
        }

        // Log if something changes the color away from red
        if (sr.color != Color.red && Input.GetKey(testKey))
        {
            Debug.Log($"[SpriteFlashDebugger] Color is NOT red anymore. Current={sr.color}. Something overwrote it.");
        }
    }

    static string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}
