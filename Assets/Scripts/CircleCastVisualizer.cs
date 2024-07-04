using UnityEngine;

public class CircleCastVisualizer : MonoBehaviour {
    public float radius = 1.0f;
    public Vector2 direction = Vector2.right;
    public float distance = 5.0f;
    public Vector2 originOffset = Vector2.zero; // New property to set the origin offset

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)originOffset, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)(originOffset + direction.normalized * distance), radius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + (Vector3)originOffset, transform.position + (Vector3)(originOffset + direction.normalized * distance));
    }
}