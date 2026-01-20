using UnityEngine;

public class CanvasPosition : MonoBehaviour
{
   public Vector2 canvasPosition = Vector2.zero;
    
    void Start()
    {
        RectTransform canvasRect = GetComponent<RectTransform>();
        canvasRect.anchoredPosition = canvasPosition;
        Debug.Log($"Fixed canvas position to: {canvasPosition}");
    }
    
    void Update()
    {
        if (GetComponent<RectTransform>().anchoredPosition != canvasPosition)
        {
            GetComponent<RectTransform>().anchoredPosition = canvasPosition;
        }
    }
}
