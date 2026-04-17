using UnityEngine;

public class HazardSpawnPoint : MonoBehaviour
{
    [Header("Gizmo Settings")]
    public float gizmoLength = 2f;

    // Direction the hazard will move (2D)
    public Vector2 Direction
    {
        get { return transform.up.normalized; }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Vector3 start = transform.position;
        Vector3 end = start + (Vector3)Direction * gizmoLength;

        Gizmos.DrawLine(start, end);

        // Draw arrow head
        Vector3 left  = Quaternion.Euler(0, 0, 25) * -Direction;
        Vector3 right = Quaternion.Euler(0, 0, -25) * -Direction;

        Gizmos.DrawLine(end, end + left * 0.3f);
        Gizmos.DrawLine(end, end + right * 0.3f);
    }
}
