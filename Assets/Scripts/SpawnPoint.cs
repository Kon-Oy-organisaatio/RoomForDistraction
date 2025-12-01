using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Tooltip("Size of the gizmo square")]
    public float gizmoSize = 0.2f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Position the gizmo so its bottom edge sits flush at the spawn point
        Vector3 pos = transform.position + Vector3.up * (gizmoSize * 0.5f);

        // Draw a wire cube (square gizmo) with its bottom edge at the surface
        Gizmos.DrawWireCube(pos, new Vector3(gizmoSize, gizmoSize, gizmoSize));
    }
}
