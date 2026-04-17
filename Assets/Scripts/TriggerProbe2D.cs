using UnityEngine;

public class TriggerProbe2D : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[PROBE] {name} triggered by {other.name} | tag={other.tag} | layer={LayerMask.LayerToName(other.gameObject.layer)}");
    }
}
