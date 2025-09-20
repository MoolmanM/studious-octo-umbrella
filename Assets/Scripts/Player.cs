using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    public RectTransform RectTransform => rectTransform ?? (rectTransform = GetComponent<RectTransform>());
}
