using UnityEngine;
using UnityEngine.UI;

public class BookUI : MonoBehaviour
{
    public GameObject bookWindow;
    public GameObject profileContent;
    public GameObject residentsContent;
    public GameObject rulesContent;
    public GameObject questsContent;

    public Button profileButton;
    public Button residentsButton;
    public Button rulesButton;
    public Button questButton;

    public float textOffsetX_Active = 0f;
    public float textOffSetX_Inactive = 15f;

    private MattockController mattock;
    private WateringCanController wateringCan;

    private bool isOpen = false;
    void Start()
    {
        mattock = FindObjectOfType<MattockController>();
        wateringCan = FindObjectOfType<WateringCanController>();

        ApplyButtonStyle(profileButton, true);
        ApplyButtonStyle(residentsButton, false);
        ApplyButtonStyle(rulesButton, false);
        ApplyButtonStyle(questButton, false);

        SetProfileActive(true);
        SetResidentsActive(false);
        SetRulesActive(false);
        SetQuestsActive(false);

        bookWindow.SetActive(false);
    }

    public void ToggleWindow()
    {
        isOpen = !isOpen;
        bookWindow.SetActive(isOpen);

        if (mattock != null)
            mattock.enabled = !isOpen;
        if (wateringCan != null)
            wateringCan.enabled = !isOpen;

        if (isOpen)
        {
            ShowProfile();
        }
    }

    public void ShowProfile()
    {
        profileContent.SetActive(true);
        residentsContent.SetActive(false);
        rulesContent.SetActive(false);
        questsContent.SetActive(false);

        ApplyButtonStyle(profileButton, true);
        ApplyButtonStyle(residentsButton, false);
        ApplyButtonStyle(rulesButton, false);
        ApplyButtonStyle(questButton, false);
    }

    public void ShowResidents()
    {
        profileContent.SetActive(false);
        residentsContent.SetActive(true);
        rulesContent.SetActive(false);
        questsContent.SetActive(false);

        ApplyButtonStyle(profileButton, false);
        ApplyButtonStyle(residentsButton, true);
        ApplyButtonStyle(rulesButton, false);
        ApplyButtonStyle(questButton, false);
    }

    public void ShowRules()
    {
        profileContent.SetActive(false);
        residentsContent.SetActive(false);
        rulesContent.SetActive(true);
        questsContent.SetActive(false);

        ApplyButtonStyle(profileButton, false);
        ApplyButtonStyle(residentsButton, false);
        ApplyButtonStyle(rulesButton, true);
        ApplyButtonStyle(questButton, false);
    }

    public void ShowQuests()
    {
        profileContent.SetActive(false);
        residentsContent.SetActive(false);
        rulesContent.SetActive(false);
        questsContent.SetActive(true);

        ApplyButtonStyle(profileButton, false);
        ApplyButtonStyle(residentsButton, false);
        ApplyButtonStyle(rulesButton, false);
        ApplyButtonStyle(questButton, true);
    }


    private void ApplyButtonStyle(Button btn, bool isActive)
    {
        if (btn == null) return;

        Text btnText = btn.GetComponentInChildren<Text>();

        if (btnText != null)
        {
            RectTransform textRect = btnText.GetComponent<RectTransform>();
            if (textRect != null)
            {
                float offSetX = isActive ? textOffsetX_Active : textOffSetX_Inactive;
                Vector2 pos = textRect.anchoredPosition;
                pos.x = offSetX;
                textRect.anchoredPosition = pos;
            }
        }
    }

    private void SetProfileActive(bool active)
    {
        if (profileContent != null) profileContent.SetActive(active);
    }

    private void SetResidentsActive(bool active)
    {
        if (residentsContent != null) residentsContent.SetActive(active);
    }

    private void SetRulesActive(bool active)
    {
        if (rulesContent != null) rulesContent.SetActive(active);
    }

    private void SetQuestsActive(bool active)
    {
        if (questsContent != null) questsContent.SetActive(active);
    }
}
