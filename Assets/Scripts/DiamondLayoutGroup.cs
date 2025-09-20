using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[AddComponentMenu("Layout/Diamond Layout Group")]
public class DiamondLayoutGroup : LayoutGroup
{
  public Vector2 cellSize = new Vector2(100, 100);
  public Vector2 spacing = Vector2.zero;

  public override void CalculateLayoutInputHorizontal()
  {
    base.CalculateLayoutInputHorizontal();
    ArrangeChildren();
  }

  public override void CalculateLayoutInputVertical()
  {
    ArrangeChildren();
  }

  public override void SetLayoutHorizontal() { /* handled in Calculate */ }
  public override void SetLayoutVertical() { /* handled in Calculate */ }

  protected override void OnTransformChildrenChanged()
  {
    base.OnTransformChildrenChanged();
    SetDirty();
  }

  protected override void OnRectTransformDimensionsChange()
  {
    base.OnRectTransformDimensionsChange();
    SetDirty();
  }

  void OnValidate()
  {
    SetDirty();
  }

  void SetDirty()
  {
#if UNITY_EDITOR
    if (!Application.isPlaying)
      UnityEditor.EditorApplication.delayCall += () => { if (this) LayoutRebuilder.MarkLayoutForRebuild(rectTransform); };
    else
#endif
      LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
  }

  private void ArrangeChildren()
  {
    int count = rectChildren.Count;
    if (count == 0) return;

    List<int> rows = GenerateDiamondRows(count);

    int rowsCount = rows.Count;
    int maxRowSize = rows.Max();

    float shapeWidth = maxRowSize * cellSize.x + (maxRowSize - 1) * spacing.x;
    float shapeHeight = rowsCount * cellSize.y + (rowsCount - 1) * spacing.y;

    float parentWidth = rectTransform.rect.width;
    float parentHeight = rectTransform.rect.height;
    float availableWidth = Mathf.Max(0f, parentWidth - padding.left - padding.right);
    float availableHeight = Mathf.Max(0f, parentHeight - padding.top - padding.bottom);

    Vector2 anchorNorm = GetAnchorNormalized(childAlignment);
    float ax = anchorNorm.x;
    float ay = anchorNorm.y;

    float contentLeft = -parentWidth * 0.5f + padding.left;
    float contentTop = parentHeight * 0.5f - padding.top;

    float shapeLeft = contentLeft + ax * Mathf.Max(0f, (availableWidth - shapeWidth));
    float shapeCenterX = shapeLeft + shapeWidth * 0.5f;

    float shapeTop = contentTop - ay * Mathf.Max(0f, (availableHeight - shapeHeight));
    float shapeCenterY = shapeTop - shapeHeight * 0.5f;

    int childIndex = 0;
    float halfShapeHeight = shapeHeight * 0.5f;
    for (int row = 0; row < rowsCount && childIndex < rectChildren.Count; row++)
    {
      int rowSize = rows[row];
      float rowWidth = rowSize * cellSize.x + (rowSize - 1) * spacing.x;

      float startXLocal = -rowWidth * 0.5f + cellSize.x * 0.5f;
      float yLocal = halfShapeHeight - (cellSize.y * 0.5f) - row * (cellSize.y + spacing.y);

      for (int i = 0; i < rowSize && childIndex < rectChildren.Count; i++, childIndex++)
      {
        RectTransform child = rectChildren[childIndex];

        child.anchorMin = child.anchorMax = new Vector2(0.5f, 0.5f);
        child.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, cellSize.x);
        child.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, cellSize.y);

        float childLocalX = startXLocal + i * (cellSize.x + spacing.x);
        Vector2 finalPos = new Vector2(shapeCenterX + childLocalX, shapeCenterY + yLocal);

        child.anchoredPosition = finalPos;
      }
    }
  }

  private List<int> GenerateDiamondRows(int n)
  {
    int m = Mathf.CeilToInt(Mathf.Sqrt(n));      // peak guess
    int len = 2 * m - 1;
    List<int> counts = new List<int>(len);
    for (int j = 0; j < len; j++)
      counts.Add(j < m ? j + 1 : 2 * m - 1 - j);

    int cap = m * m;
    int excess = cap - n;
    int peak = m - 1;

    while (excess > 0)
    {
      bool changed = false;

      for (int delta = 1; delta <= peak && excess >= 2; delta++)
      {
        int left = peak - delta;
        int right = peak + delta;
        if (left >= 0 && right < counts.Count && counts[left] > 1 && counts[right] > 1)
        {
          counts[left]--;
          counts[right]--;
          excess -= 2;
          changed = true;
          break;
        }
      }
      if (changed) continue;

      if (excess > 0 && counts[peak] > 1)
      {
        int dec = Mathf.Min(counts[peak] - 1, excess);
        counts[peak] -= dec;
        excess -= dec;
        continue;
      }

      for (int delta = 0; delta <= peak && excess > 0; delta++)
      {
        int left = peak - delta;
        int right = peak + delta;
        if (left >= 0 && counts[left] > 1)
        {
          counts[left]--;
          excess--;
          if (excess == 0) break;
        }
        if (right < counts.Count && counts[right] > 1)
        {
          counts[right]--;
          excess--;
          if (excess == 0) break;
        }
      }

      if (!changed && excess > 0 && counts.All(v => v <= 1))
        break;
    }

    counts = counts.Where(v => v > 0).ToList();
    return counts;
  }

  private Vector2 GetAnchorNormalized(TextAnchor a)
  {
    // TODO: Reverse lower center and upper center, and possibly others, to match Unity's actual behavior.
    switch (a)
    {
      case TextAnchor.UpperLeft: return new Vector2(0f, 1f);
      case TextAnchor.UpperCenter: return new Vector2(0.5f, 1f);
      case TextAnchor.UpperRight: return new Vector2(1f, 1f);
      case TextAnchor.MiddleLeft: return new Vector2(0f, 0.5f);
      case TextAnchor.MiddleCenter: return new Vector2(0.5f, 0.5f);
      case TextAnchor.MiddleRight: return new Vector2(1f, 0.5f);
      case TextAnchor.LowerLeft: return new Vector2(0f, 0f);
      case TextAnchor.LowerCenter: return new Vector2(0.5f, 0f);
      case TextAnchor.LowerRight: return new Vector2(1f, 0f);
      default: return new Vector2(0.5f, 0.5f);
    }
  }
}
