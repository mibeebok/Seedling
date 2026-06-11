using UnityEngine;

public class TutorialPanelController : MonoBehaviour
{
    public RectTransform panelRect;
    public GameObject bookButtonObject;
    public Camera uiCamera;

    public Vector2 offset = new Vector2(-380, -70);

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPanel()
    {
        if (transform.parent != null && !transform.parent.gameObject.activeSelf)
            transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        PositionRelativeToBookButton();
    }

    private void PositionRelativeToBookButton()
    {
        if (bookButtonObject == null || uiCamera == null) return;
      
        Vector3 worldPos = bookButtonObject.transform.position;
        Vector2 screenPos = uiCamera.WorldToScreenPoint(worldPos);

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, null, out localPoint);
            panelRect.anchoredPosition = localPoint + offset;
        }
        else
        {
            panelRect.anchoredPosition = screenPos + offset;
        }
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
