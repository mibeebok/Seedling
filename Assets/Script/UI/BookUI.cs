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

    private bool showingInProgress = true;

    public Sprite plusSprite;
    public Sprite minusSprite;

    private MattockController mattock;
    private WateringCanController wateringCan;
    private InventoryController inventoryController;

    private bool isOpen = false;
    void Start()
    {
        mattock = FindFirstObjectByType<MattockController>();
        wateringCan = FindFirstObjectByType<WateringCanController>();
        inventoryController = FindFirstObjectByType<InventoryController>();

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
        if (ShopUI.IsShopOpen) return;

        isOpen = !isOpen;
        bookWindow.SetActive(isOpen);

        var tutorialPanel = FindFirstObjectByType<TutorialPanelController>();
        if (tutorialPanel != null && tutorialPanel.gameObject.activeSelf)
            tutorialPanel.HidePanel();

        if (mattock != null)
            mattock.enabled = !isOpen;
        if (wateringCan != null)
            wateringCan.enabled = !isOpen;

        if (inventoryController != null)
            inventoryController.enabled = !isOpen;

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

        RefreshQuestsFromManager();
    }

    public void OnInProgressClicked()
    {
        showingInProgress = true;
        RefreshQuestsFromManager();
    }

    public void OnCompletedClicked()
    {
        showingInProgress = false;
        RefreshQuestsFromManager();
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

    public void RefreshQuestsFromManager()
    {
        if (QuestManager.Instance == null) return;
        PopulateQuestsFromManager(showingInProgress);
    }

    private void PopulateQuestsFromManager(bool showInProgress)
    {
        if (questsContentContainer == null) return;

        foreach (Transform child in questsContentContainer)
            Destroy(child.gameObject);

        var quests = showInProgress ? QuestManager.Instance.activeQuests : QuestManager.Instance.completedQuests;

        foreach (var quest in quests)
        {
            if (showInProgress)
            {
                GameObject card = Instantiate(questCardPrefab, questsContentContainer);
                Transform nameObj = card.transform.Find("QuestName");
                if (nameObj != null) nameObj.GetComponent<Text>().text = quest.questName;
                Transform descObj = card.transform.Find("QuestDescription");
                if (descObj != null) descObj.GetComponent<Text>().text = quest.description;
                Transform progObj = card.transform.Find("QuestProgress");
                if (progObj != null)
                {
                    string progressText = "";
                    int completed = 0;
                    foreach (var task in quest.tasks)
                        if (task.isCompleted) completed++;
                    progressText = $"{completed}/{quest.tasks.Count}. ";
                    for (int i = 0; i < quest.tasks.Count; i++)
                    {
                        if (i > 0) progressText += "\n";
                        if (quest.tasks[i].isCompleted)
                            progressText += $"<color=green>{quest.tasks[i].description}</color>";
                        else
                            progressText += quest.tasks[i].description;

                    }
                    progObj.GetComponent<Text>().text = progressText;
                }
            }
            else
            {
                GameObject card = Instantiate(completedQuestCardPrefab, questsContentContainer);
                CompletedQuestCardController controller = card.GetComponent<CompletedQuestCardController>();
                if (controller != null)
                {
                    controller.SetData(quest.questName, quest.completionNotes);
                    controller.plusSprite = plusSprite;
                    controller.minusSprite = minusSprite;
                }
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
