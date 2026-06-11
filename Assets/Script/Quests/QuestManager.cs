using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        NotifyUI();
    }

    public void CompleteTask(string taskDescription)
    {
        bool changed = false;
        foreach (var quest in activeQuests)
        {
            foreach (var task in quest.tasks)
            {
                if (!task.isCompleted && task.description == taskDescription)
                {
                    task.isCompleted = true;
                    changed = true;
                }
            }

            if (IsQuestCompleted(quest))
            {
                CompleteQuest(quest);
                changed = true;
                NotifyUI();
                return;
            }
        }

        if (changed) NotifyUI();
    }

    private bool IsQuestCompleted(Quest quest)
    {
        foreach (var task in quest.tasks)
            if (!task.isCompleted) return false;
        return true;
    }

    private void CompleteQuest(Quest quest)
    {
        activeQuests.Remove(quest);
        completedQuests.Add(quest);
        GiveReward(quest);
        ShowQuestCompletePanel(quest);
    }

    private void GiveReward(Quest quest)
    {
        if (MoneyDisplay.Instance != null)
            MoneyDisplay.Instance.AddMoney(quest.rewardMoney);
        else
            Debug.LogError("MoneyDisplay.Instance не найден");

        if (quest.rewardCropType != CropType.None && quest.rewardSeedCount > 0)
        {
            if (InventoryManager.Instance != null)
                InventoryManager.Instance.AddSeedToInventory(quest.rewardCropType, quest.rewardSeedCount);
            else
                Debug.LogError("InventoryManager.Instance не найден");
        }
    }

    private void ShowQuestCompletePanel(Quest quest)
    {
        QuestCompleteUI ui = FindFirstObjectByType<QuestCompleteUI>();
        if (ui != null)
            ui.Show(quest);
    }

    private void NotifyUI()
    {
        BookUI bookUI = FindFirstObjectByType<BookUI>();
        if (bookUI != null) bookUI.RefreshQuestsFromManager();
    }

    public void StartIntroQuest()
    {
        Quest introQuest = QuestDatabase.IntroQuest();
        AddQuest(introQuest);

        SetNPCQuestDialogue("Терентий", "TerentyDialogueQuest1");
        SetNPCQuestDialogue("Финник", "FinnickDialogueQuest1");
        SetNPCQuestDialogue("Ихвильнихт", "IhvilnichtDialogueQuest1");
    }

    public void SetNPCQuestDialogue(string npcObjectName, string dialogueKey)
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        foreach (var npc in allNPCs)
        {
            if (npc.gameObject.name == npcObjectName)
            {
                npc.dialogueKey = dialogueKey;
                return;
            }
        }
    }
}
