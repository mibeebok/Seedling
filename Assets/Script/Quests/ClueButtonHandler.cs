using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClueButtonHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SpriteRenderer clueSpriteRenderer;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public GameObject evidencePanel;
    public EvidenceViewer evidenceViewer;
    public QuestManager questManager;
    public TextMesh progressText;

    private bool isFound = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isFound && highlightedSprite != null)
            clueSpriteRenderer.sprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isFound && normalSprite != null)
            clueSpriteRenderer.sprite = normalSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isFound) return;
        isFound = true;
        if (evidencePanel != null)
            evidencePanel.SetActive(true);
        if (progressText != null)
            progressText.text = "Улика: 1/1";
        clueSpriteRenderer.gameObject.SetActive(false);
        if (questManager != null)
            questManager.SetClueFound();
        gameObject.SetActive(false);
    }
}
