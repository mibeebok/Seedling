using UnityEngine;
using UnityEngine.UI;

public class CompletedQuestCardController : MonoBehaviour
{

    public GameObject detailsPanel;
    public Button toggleButton;
    public Image buttonImage;

    public Text questNameText;
    public Text grishaThoughtsText;
    public RectTransform headerPanel;

    public Sprite plusSprite;
    public Sprite minusSprite;

    private bool isExpanded = false;
    void Start()
    {
        if (toggleButton != null)
            toggleButton.onClick.AddListener(ToggleDetails);
        if (buttonImage == null && toggleButton != null)
            buttonImage = toggleButton.GetComponent<Image>();
        if (detailsPanel != null)
            detailsPanel.SetActive(false);
        UpdateButtonSprite();

    }

    public void ToggleDetails()
    {
        isExpanded = !isExpanded;
        if (detailsPanel != null)
            detailsPanel.SetActive(isExpanded);
        UpdateButtonSprite();

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
   
    }

    private void UpdateButtonSprite()
    {
        if (buttonImage != null)
            buttonImage.sprite = isExpanded ? minusSprite : plusSprite;
    }

    public void SetData(string questName, string thoughts)
    {
        if (questNameText != null)
            questNameText.text = questName;
        if (grishaThoughtsText != null)
            grishaThoughtsText.text = thoughts;
        if (headerPanel != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(headerPanel);
    }
}
