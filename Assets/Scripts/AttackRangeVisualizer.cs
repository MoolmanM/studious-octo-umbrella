using UnityEngine;

using UnityEngine;

public class AttackRangeVisualizer : MonoBehaviour
{
  public RectTransform enemy;   // assign in inspector
  public float ellipseX = 50f;  // horizontal radius
  public float ellipseY = 25f;  // vertical radius
  public int ellipseSegments = 40; // smoothness of ellipse

  private void OnDrawGizmos()
  {
    if (enemy == null) return;

    Vector3 pos = enemy.position;
    Vector2 size = enemy.rect.size;

    // --- Draw rectangle (matches enemy size) ---
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireCube(pos, size);

    // --- Draw ellipse centered on enemy ---
    Gizmos.color = Color.cyan;
    DrawEllipse(pos, ellipseX, ellipseY, ellipseSegments);
  }

  private void DrawEllipse(Vector3 center, float radiusX, float radiusY, int segments)
  {
    Vector3 prevPoint = center + new Vector3(radiusX, 0, 0);

    for (int i = 1; i <= segments; i++)
    {
      float angle = i * 2 * Mathf.PI / segments;
      Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radiusX, Mathf.Sin(angle) * radiusY, 0f);

      Gizmos.DrawLine(prevPoint, newPoint);
      prevPoint = newPoint;
    }
  }
}
