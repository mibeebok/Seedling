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

    public GameObject quest7ProgressText;
    public GameObject triggerZoneQ15;
    public GameObject triggerZoneQ14_1;
    public GameObject triggerZoneQ14_2;
    public GameObject triggerZoneQ13;
    public GameObject triggerZoneQ12;
    public GameObject triggerZoneQ11;
    public GameObject triggerZoneQ10;
    public GameObject triggerZoneQ9_1;
    public GameObject triggerZoneQ8;
    public GameObject triggerZoneQ5;
    public GameObject triggerZoneQ7;
    public GameObject quest7Arrows;
    private bool clueFound = false;
    public int quest12Choice = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

        if (quest.questName == "Квест 3. Я-огородник")
        {
            Quest fourthQuest = QuestDatabase.FourthQuest();
            AddQuest(fourthQuest);
            SetNPCQuestDialogue("Тиоли", "TioliDialogueQuest4");
            hasNext = true;
        }

        if (quest.questName == "Квест 4. Первый друг")
        {
            Quest fifthQuest = QuestDatabase.FifthQuest();
            AddQuest(fifthQuest);
            SetNPCQuestDialogue("Ихвильнихт", "IhvilnichtDialogueQuest5");
            if (triggerZoneQ5 != null)
                triggerZoneQ5.SetActive(true);
            hasNext = true;
        }

        if (quest.questName == "Квест 5. Начало положено")
        {
            Quest sixthQuest = QuestDatabase.SixthQuest();
            AddQuest(sixthQuest);
            hasNext = true;
        }

        if (quest.questName == "Квест 6. П-Предприниматель")
        {
            Quest seventhQuest = QuestDatabase.SeventhQuest();
            AddQuest(seventhQuest);
            if (triggerZoneQ7 != null) triggerZoneQ7.SetActive(true);
            if (quest7Arrows != null) quest7Arrows.SetActive(true);
            hasNext = true;
        }

        if (quest.questName == "Квест 7. Я доберусь до правды")
        {
            if (quest7Arrows != null) quest7Arrows.SetActive(false);
            if (quest7ProgressText != null) quest7ProgressText.SetActive(false);

            Quest eighthQuest = QuestDatabase.EighthQuest();
            AddQuest(eighthQuest);
            if (triggerZoneQ8 != null) triggerZoneQ8.SetActive(true);
            hasNext = true;
        }
        if (quest.questName == "Квест 8. Тайны, тайны, тайны...")
        {
            Quest ninthQuest = QuestDatabase.NinthQuest();
            AddQuest(ninthQuest);
            if (triggerZoneQ9_1 != null) triggerZoneQ9_1.SetActive(true);
            hasNext = true;
        }
        if (quest.questName == "Квест 9. Завелась крыса?")
        {
            Quest tenthQuest = QuestDatabase.TenthQuest();
            AddQuest(tenthQuest);
            if (triggerZoneQ10 != null) triggerZoneQ10.SetActive(true);
            hasNext = true;
        }

        if (quest.questName == "Квест 10. Навестим угрюмого")
        {
            Quest eleventhQuest = QuestDatabase.EleventhQuest();
            AddQuest(eleventhQuest);
            SetNPCQuestDialogue("Тиоли", "TioliDialogueQuest11");
            if (triggerZoneQ11 != null) triggerZoneQ11.SetActive(true);
            hasNext = true;
        }
        if (quest.questName == "Квест 11. Что скрывает лис?")
        {
            Quest twelfthQuest = QuestDatabase.TwelfthQuest();
            AddQuest(twelfthQuest);
            if (triggerZoneQ12 != null) triggerZoneQ12.SetActive(true);
            hasNext = true;
        }
        if (quest.questName == "Квест 12. Так близко, но так далеко")
        {
            // заметки гриши в зависимости от выбора
            if (quest12Choice == 1)
            {
                // хороший путь
                quest.completionNotes = "«Разгадка совсем близко… осталось совсем чуть-чуть, я уже это чувствую.».";
                Quest thirteenthQuest = QuestDatabase.ThirteenthQuest();
                AddQuest(thirteenthQuest);
                if (triggerZoneQ13 != null) triggerZoneQ13.SetActive(true);
                hasNext = true;
            }
            else if (quest12Choice == 2)
            {
                // плохой путь
                quest.completionNotes = "«Честно сказать, я не совсем уверен в правильности своего выбора. Но ведь улики… всё ведёт к нему. Надеюсь, я не пожалею о сделанном.».";
                Quest fifteenthQuest = QuestDatabase.FifteenthQuest();
                AddQuest(fifteenthQuest);
                if (triggerZoneQ15 != null) 
                    triggerZoneQ15.SetActive(true);
                hasNext = true;
            }
            quest12Choice = 0;
        }

        if (quest.questName == "Квест 13. Состав... шерсти?")
        {
            Quest fourteenthQuest = QuestDatabase.FourteenthQuest();
            AddQuest(fourteenthQuest);
            if (triggerZoneQ14_1 != null) triggerZoneQ14_1.SetActive(true);
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
        if (quest.questName == "Квест 14. Счастливый финал?" || quest.questName == "Квест 15. Суд")
            return;

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
    public void SetClueFound() => clueFound = true;
    public bool IsClueFound() => clueFound;

    public int TeamWolf { get; private set; } = 0;
    public int TeamFox { get; private set; } = 0;

    public void AddTeamWolf(int amount)
    {
        TeamWolf += amount;
        SaveSystem.SaveGame();
    }

    public void AddTeamFox(int amount)
    {
        TeamFox += amount;
        SaveSystem.SaveGame();
    }
}
