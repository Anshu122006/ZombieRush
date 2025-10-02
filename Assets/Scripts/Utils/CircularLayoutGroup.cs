using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[AddComponentMenu("Layout/Circular Layout Group")]
public class CircularLayoutGroup : LayoutGroup {
    [Header("Child Settings")]
    public float width = 100f;
    public float height = 100f;

    [Header("Circle Settings")]
    public float radius = 100f;
    public float startAngle = 0f;
    public bool clockwise = true;
    public bool rotateChildren = false;

    public override void CalculateLayoutInputHorizontal() {
        base.CalculateLayoutInputHorizontal();
        ArrangeChildren();
    }

    public override void CalculateLayoutInputVertical() {
        ArrangeChildren();
    }

    public override void SetLayoutHorizontal() {
        ArrangeChildren();
    }

    public override void SetLayoutVertical() {
        ArrangeChildren();
    }

    private void ArrangeChildren() {
        int count = rectChildren.Count;
        if (count == 0) return;

        float angleStep = 360f / count;

        for (int i = 0; i < count; i++) {
            RectTransform child = rectChildren[i];
            child.sizeDelta = new Vector2(width, height);

            float angle = startAngle + (clockwise ? -i * angleStep : i * angleStep);
            float rad = angle * Mathf.Deg2Rad;

            Vector2 pos = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * radius;
            child.anchoredPosition = pos;

            if (rotateChildren) {
                child.localRotation = Quaternion.Euler(0, 0, angle);
            }
            else {
                child.localRotation = Quaternion.identity;
            }
        }
    }
}
