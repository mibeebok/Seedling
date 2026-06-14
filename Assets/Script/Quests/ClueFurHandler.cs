using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ClueFurHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public SpriteRenderer clueSpriteRenderer;   
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    public GameObject secondCutsceneStarter;   
    public TextMesh popupText;

    private bool isCollected = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isCollected && highlightedSprite != null)
            clueSpriteRenderer.sprite = highlightedSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isCollected && normalSprite != null)
            clueSpriteRenderer.sprite = normalSprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCollected) return;
        isCollected = true;

        if (popupText != null)
        {
            popupText.gameObject.SetActive(true);
        }

        QuestManager.Instance.CompleteTask("Разобраться, в чём дело");

        if (secondCutsceneStarter != null)
            secondCutsceneStarter.SetActive(true);
        else
            Debug.LogError("secondCutsceneStarter не назначен");

        if (clueSpriteRenderer != null)
            clueSpriteRenderer.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
