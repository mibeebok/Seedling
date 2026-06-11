using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text targetText;
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(219f / 255f, 177f / 255f, 111f / 255f);

    private void Awake()
    {
        if (targetText == null)
            targetText = GetComponentInChildren<Text>();
        if (targetText != null)
            normalColor = targetText.color;
        else
            Debug.LogError("ButtonHoverColor: не найден текст на кнопке");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null)
            targetText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetText != null)
            targetText.color = normalColor;
    }
}
