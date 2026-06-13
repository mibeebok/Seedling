using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public TutorialHomeController tutorialHomeController;
    public static QuestManager Instance { get; private set; }

    public List<Quest> activeQuests = new List<Quest>();
    public List<Quest> completedQuests = new List<Quest>();
    public GameObject finnickTriggerZone;
    private Coroutine plowWaterCoroutine;
    private Coroutine carrotCoroutine;

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

        if (quest.questName == "Квест 3. Я-огородник")
        {
            bool plowWaterDone = false;
            bool carrotDone = false;
            foreach (var task in quest.tasks)
            {
                if (task.description == "Вскопать и полить грядку" && task.isCompleted)
                    plowWaterDone = true;
                if (task.description == "Вырастить 3 морковки" && task.isCompleted)
                    carrotDone = true;
            }
            if (!plowWaterDone) StartCheckPlowAndWater();
            if (!carrotDone) StartCheckCarrots();
        }
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

        if (taskDescription == "Вскопать и полить грядку" && plowWaterCoroutine != null)
        {
            StopCoroutine(plowWaterCoroutine);
            plowWaterCoroutine = null;
        }

        if (taskDescription == "Вырастить 3 морковки" && carrotCoroutine != null)
        {
            StopCoroutine(carrotCoroutine);
            carrotCoroutine = null;
        }

        
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

        if (quest.npcDialogueChanges != null)
        {
            foreach (var change in quest.npcDialogueChanges)
            {
                SetNPCQuestDialogue(change.npcName, change.newDialogueKey);
            }
        }

        bool hasNext = false;
        if (quest.questName == "Квест 1. Знакомство с деревней")
        {
            Quest secondQuest = QuestDatabase.SecondQuest();
            AddQuest(secondQuest);
            SetNPCQuestDialogue("Тиоли", "TioliDialogueQuest2");

            hasNext = true;
        }

        if (quest.questName == "Квест 2. Первые шаги")
        {
            Quest thirdQuest = QuestDatabase.ThirdQuest();
            AddQuest(thirdQuest);

            hasNext = true;
        }

        ShowQuestCompletePanel(quest, hasNext);
        SaveSystem.SaveGame();
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

    private void ShowQuestCompletePanel(Quest quest, bool hasNext = false)
    {
        QuestCompleteUI ui = FindFirstObjectByType<QuestCompleteUI>();
        if (ui != null)
            ui.Show(quest, hasNext);
    }

    private void NotifyUI()
    {
        BookUI bookUI = FindFirstObjectByType<BookUI>();
        if (bookUI != null) bookUI.RefreshQuestsFromManager();
    }

    public void StartIntroQuest()
    {
        Debug.Log("StartIntroQuest called");
        Quest introQuest = QuestDatabase.IntroQuest();
        AddQuest(introQuest);

        SetNPCQuestDialogue("Терентий", "TerentyDialogueQuest1");
        SetNPCQuestDialogue("Финник", "FinnickDialogueQuest1");
        SetNPCQuestDialogue("Ихвильнихт", "IhvilnichtDialogueQuest1");
        SetNPCQuestDialogue("Тиоли", null);
    }

    public void SetNPCQuestDialogue(string npcName, string dialogueKey)
    {
        var allNPCs = FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        bool found = false;
        foreach (var npc in allNPCs)
        {
            if (npc.npcName == npcName)
            {
                npc.dialogueKey = dialogueKey;
                found = true;
            }
        }
        if (!found) Debug.LogWarning($"NPC with name '{npcName}' not found!");
    }

    public void LoadQuestsFromSave(List<SaveSystem.QuestSaveData> activeSave, List<SaveSystem.QuestSaveData> completedSave)
    {
        activeQuests.Clear();
        completedQuests.Clear();

        if (activeSave != null)
        {
            foreach (var qsd in activeSave)
            {
                Quest quest = new Quest();
                quest.questName = qsd.questName;
                quest.description = qsd.description;
                quest.completionNotes = qsd.completionNotes;
                quest.rewardMoney = qsd.rewardMoney;
                quest.rewardCropType = (CropType)System.Enum.Parse(typeof(CropType), qsd.rewardCropType);
                quest.rewardSeedCount = qsd.rewardSeedCount;
                quest.tasks = new List<QuestTask>();
                foreach (var tsd in qsd.tasks)
                {
                    quest.tasks.Add(new QuestTask { description = tsd.description, isCompleted = tsd.isCompleted} );
                }

                if (qsd.npcDialogueChanges != null)
                {
                    quest.npcDialogueChanges = new List<NPCDialogueChange>();
                    foreach (var nd in qsd.npcDialogueChanges)
                    {
                        quest.npcDialogueChanges.Add(new NPCDialogueChange 
                        { 
                            npcName = nd.npcName, 
                            newDialogueKey = nd.dialogueKey 
                        });
                    }
                }
                activeQuests.Add(quest);
            }
        }

        if (completedSave != null)
        {
            foreach (var qsd in completedSave)
            {
                Quest quest = new Quest();
                quest.questName = qsd.questName;
                quest.description = qsd.description;
                quest.completionNotes = qsd.completionNotes;
                quest.rewardMoney = qsd.rewardMoney;
                quest.rewardCropType = (CropType)System.Enum.Parse(typeof(CropType), qsd.rewardCropType);
                quest.rewardSeedCount = qsd.rewardSeedCount;
                quest.tasks = new List<QuestTask>();
                foreach (var tsd in qsd.tasks)
                {
                    quest.tasks.Add(new QuestTask { description = tsd.description, isCompleted = tsd.isCompleted });
                }

                if (qsd.npcDialogueChanges != null)
                {
                    quest.npcDialogueChanges = new List<NPCDialogueChange>();
                    foreach (var nd in qsd.npcDialogueChanges)
                    {
                        quest.npcDialogueChanges.Add(new NPCDialogueChange
                        { 
                            npcName = nd.npcName, 
                            newDialogueKey = nd.dialogueKey 
                        });
                    }
                }

                completedQuests.Add(quest);

            }
        }
        NotifyUI();
    }

    private void StartCheckPlowAndWater()
    {
        if (plowWaterCoroutine != null) StopCoroutine(plowWaterCoroutine);
        plowWaterCoroutine = StartCoroutine(CheckPlowAndWaterTask());
    }

    private IEnumerator CheckPlowAndWaterTask()
    {
        while (true)
        {
            bool found = false;
            for (int x = 0; x < FarmGrid.Instance.gridSizeX; x++)
            {
                for (int y = 0; y < FarmGrid.Instance.gridSizeY; y++)
                {
                    var tile = FarmGrid.Instance.GetTileAt(new Vector2Int(x, y));
                    if (tile != null)
                    {
                        var soil = tile.GetComponent<SoilTile>();
                        if (soil != null && soil.isPlowed && soil.isWatered)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                if (found) break;
            }
            if (found)
            {
                CompleteTask("Вскопать и полить грядку");
                yield break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    private void StartCheckCarrots()
    {
        if (carrotCoroutine != null) StopCoroutine(carrotCoroutine);
        carrotCoroutine = StartCoroutine(CheckCarrotsTask());
    }

    private IEnumerator CheckCarrotsTask()
    {
        while (true)
        {
            int carrotCount = 0;
            var inv = InventoryController.Instance.mainInventory;
            Item carrotItem = DataBase.Instance.GetItemByCropType(CropType.Carrot, false);
            if (carrotItem != null)
            {
                for (int i = InventoryController.TOOL_SLOTS; i < inv.items.Count; i++)
                {
                    if (inv.items[i].id == carrotItem.id)
                        carrotCount += inv.items[i].count;
                }
            }
            if (carrotCount >= 3)
            {
                CompleteTask("Вырастить 3 морковки");
                yield break;
            }
            yield return new WaitForSeconds(5f);
        }
    }

    public bool IsQuestActive(string questName)
    {
        foreach (var q in activeQuests)
            if (q.questName == questName) return true;
        return false;
    }
}
