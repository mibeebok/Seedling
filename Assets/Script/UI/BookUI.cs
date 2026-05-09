using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

    [System.Serializable]
    public class ResidentData
    {
        public string name;
        public string info;
        public Sprite icon;
    }
    public ResidentData[] residents;
    public GameObject residentCardPrefab;
    public Transform residentsContentContainer;
    public ScrollRect residentsScrollRect;

    [System.Serializable]
    public class QuestData
    {
        public string name;
        public string description;
        public string progress;
    }

    public QuestData[] questsInProgress;
    public QuestData[] questsCompleted;
    public GameObject questCardPrefab;
    public GameObject completedQuestCardPrefab;
    public Transform questsContentContainer;
    public ScrollRect questsScrollRect;
    public Button inProgressButton;
    public Button completedButton;

    public Sprite plusSprite;
    public Sprite minusSprite;

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

        if (inProgressButton != null)
            inProgressButton.onClick.AddListener(OnInProgressClicked);
        if (completedButton != null)
            completedButton.onClick.AddListener(OnCompletedClicked);
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

    public void PopulateResidents()
    {
        if (residentsContentContainer == null || residentCardPrefab == null)
        {
            Debug.LogWarning("Не назначен residentsContentContainer или residentCardPrefab");
            return;
        }

        foreach (Transform child in residentsContentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ResidentData rd in residents)
        {
            GameObject card = Instantiate(residentCardPrefab, residentsContentContainer);

            Transform nameObj = card.transform.Find("TextName");
            if (nameObj != null)
            {
                Text nameText = nameObj.GetComponent<Text>();
                if (nameText != null) nameText.text = rd.name;
            }
            else
            {
                Text anyText = card.GetComponentInChildren<Text>();
                if (anyText != null) anyText.text = rd.name;
            }

            Transform infoObj = card.transform.Find("TextInfo");
            if (infoObj != null)
            {
                Text infoText = infoObj.GetComponent<Text>();
                if (infoText != null) infoText.text = rd.info;
            }

            Transform iconObj = card.transform.Find("ImageIcon");
            if (iconObj != null)
            {
                Image iconImg = iconObj.GetComponent<Image>();
                if (iconImg != null) iconImg.sprite = rd.icon;
            }
        }
    }

    public void PopulateQuests(bool showInProgress)
    {
        if (questsContentContainer == null || questCardPrefab == null)
        {
            Debug.LogWarning("questsContentContainer не назначены");
            return;
        }

        foreach (Transform child in questsContentContainer)
            Destroy(child.gameObject);

        if (showInProgress)
        {
            if (questCardPrefab == null)
            {
                Debug.LogWarning("Нет префаба для активных квестов");
                return;
            }

            foreach (QuestData qd in questsInProgress)
            {
                GameObject card = Instantiate(questCardPrefab, questsContentContainer);
                FillQuestCardForActive(card, qd);
            }
        }
        else 
        {
            if (completedQuestCardPrefab == null)
            {
                Debug.LogWarning("Нет префаба для завершённых квестов");
                return;
            }

            foreach (QuestData qd in questsCompleted)
            {
                GameObject card = Instantiate(completedQuestCardPrefab, questsContentContainer);
                CompletedQuestCardController controller = card.GetComponent<CompletedQuestCardController>();

                if (controller != null)
                {
                    controller.SetData(qd.name, qd.description);
                    controller.plusSprite = plusSprite;
                    controller.minusSprite = minusSprite;
                }
                else 
                {
                    Transform nameObj = card.transform.Find("HeaderPanel/QuestName");
                    if (nameObj != null) nameObj.GetComponent<Text>().text = qd.name;

                    Transform thoughtsObj = card.transform.Find("DetailsPanel/GrishaThoughts");

                    if (thoughtsObj != null) thoughtsObj.GetComponent<Text>().text = qd.description;
                }
            }
        }

        if (questsScrollRect != null)
            questsScrollRect.verticalNormalizedPosition = 1f;
    }

    private void FillQuestCardForActive(GameObject card, QuestData qd)
    {
        Transform nameObj = card.transform.Find("QuestName");
        if (nameObj != null) nameObj.GetComponent<Text>().text = qd.name;

        Transform descObj = card.transform.Find("QuestDescription");
        if (descObj != null) descObj.GetComponent<Text>().text = qd.description;

        Transform progObj = card.transform.Find("QuestProgress");
        if (progObj != null) progObj.GetComponent<Text>().text = qd.progress;
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

        PopulateResidents();

        if (residentsScrollRect != null)
        {
            residentsScrollRect.verticalNormalizedPosition = 1f;
        }
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

        PopulateQuests(true);
    }

    public void OnInProgressClicked()
    {
        PopulateQuests(true);
    }

    public void OnCompletedClicked()
    {
        PopulateQuests(false);
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
