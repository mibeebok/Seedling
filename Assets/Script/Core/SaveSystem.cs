using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "farm_save.json");

    private static Vector2? pendingPlayerPos = null;
    private static Vector2? pendingGoatPos = null;

    private static HashSet<Vector2Int> changedTiles = new HashSet<Vector2Int>();


    public static void MarkTileChanged(Vector2Int pos)
    {
        changedTiles.Add(pos);
    }

    public static void SaveGame()
    {
        var saveFile = new SaveFile();
        Debug.Log($"[SaveGame] CropsManager.Instance exists: {CropsManager.Instance != null}, allCrops count: {CropsManager.Instance?.allCrops.Count ?? 0}");

        // Сохраняем тайлы земли
        saveFile.tiles = new List<SaveData>();
        for (int x = 0; x < FarmGrid.Instance.gridSizeX; x++)
        {
            for (int y = 0; y < FarmGrid.Instance.gridSizeY; y++)
            {
                var tile = FarmGrid.Instance.GetTileAt(new Vector2Int(x, y));
                if (tile != null)
                {
                    var soil = tile.GetComponent<SoilTile>();
                    if (soil != null)
                        saveFile.tiles.Add(soil.GetSaveData());
                }
            }
        }

        // Сохраняем растения
        saveFile.crops = new List<CropSaveData>();
        if (CropsManager.Instance != null)
        {
            Debug.Log($"[SaveGame] CropsManager OK, allCrops count: {CropsManager.Instance.allCrops.Count}");
            foreach (var kvp in CropsManager.Instance.allCrops)
            {
                var crop = kvp.Value;
                Debug.Log($"[SaveGame] Processing crop at {kvp.Key}: crop={crop != null}, cropData={crop?.cropData != null}");
                if (crop != null && crop.cropData != null)
                {
                    var saveData = new CropSaveData
                    {
                        gridX = kvp.Key.x,
                        gridY = kvp.Key.y,
                        cropType = crop.cropData.cropType.ToString(),
                        currentStage = crop.CurrentStage,
                        isRotten = crop.isRotten
                    };
                    saveFile.crops.Add(saveData);
                    Debug.Log($"[SaveGame] Added crop: {saveData.cropType} at ({saveData.gridX},{saveData.gridY})");
                }
                else
                {
                    Debug.LogWarning("[SaveGame] Crop or cropData is NULL, skipping");
                }
            }
            Debug.Log($"[SaveGame] Final crops list count: {saveFile.crops.Count}");
        }

        // Сохраняем игрока
        if (Player.Instance != null)
        {
            saveFile.player = new PlayerData
            {
                position = Player.Instance.transform.position
            };
        }

        // Сохраняем козу
        var goat = Object.FindFirstObjectByType<GoatBehavior>();
        if (goat != null)
        {
            saveFile.goat = new GoatData
            {
                position = goat.transform.position
            };
        }

        // Сохраняем инвентарь
        if (InventoryController.Instance != null && InventoryController.Instance.mainInventory != null)
        {
            var inv = InventoryController.Instance.mainInventory;
            saveFile.inventory = new InventoryData();
            saveFile.inventory.items = new List<ItemSlotData>();

            for (int i = 2; i < inv.items.Count; i++)
            {
                var slot = inv.items[i];
                if (slot.id != 0 && slot.count > 0)
                {
                    Item item = DataBase.Instance.GetItemById(slot.id);
                    if (item != null)
                    {
                        saveFile.inventory.items.Add(new ItemSlotData
                        {
                            itemName = item.name,  // ← просто name
                            count = slot.count,
                            isSeed = item.type == ItemType.Seed
                        });
                    }
                }
            }
        }

        // Сохраняем деньги
        if (MoneyDisplay.Instance != null)
            saveFile.money = MoneyDisplay.Instance.GetMoney();
        else
            Debug.LogWarning("MoneyDisplay.Instance не найден, деньги не сохранены");

        //сохраняем экологию
        var ecoController = Object.FindFirstObjectByType<EcologyController>();
        if (ecoController != null)
            saveFile.ecology = ecoController.CurrentEco;
            
        // Сохраняем интро катсцену
        saveFile.introCutscenePlayed = CutsceneManager.IntroCutscenePlayed;

        string json = JsonUtility.ToJson(saveFile, true);
        Debug.Log($"[SaveGame] JSON length: {json.Length}");

        // Сохраняем квесты
        if (QuestManager.Instance != null)
        {
            saveFile.activeQuests = ConvertQuestsToSaveData(QuestManager.Instance.activeQuests);
            saveFile.completedQuests = ConvertQuestsToSaveData(QuestManager.Instance.completedQuests);
        }

        // Сохраняем диалоговые ключи NPC
        var allNPCs = Object.FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
        saveFile.npcDialogueKey = new List<NPCDialogueSaveData>();
        foreach (var npc in allNPCs)
        {
            saveFile.npcDialogueKey.Add(new NPCDialogueSaveData
            {
                npcName = npc.npcName,
                dialogueKey = npc.dialogueKey,
            });
        }

        // сохраняем баллы для концовки
        saveFile.teamWolf = QuestManager.Instance.TeamWolf;
        saveFile.teamFox = QuestManager.Instance.TeamFox;

        // 5 Записываем в файл
        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, true));
        Debug.Log($"[SaveSystem] Игра сохранена. Путь: {SavePath}");
    }


    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            if (MoneyDisplay.Instance != null)
                MoneyDisplay.Instance.SetMoney(100);
            else
                Debug.LogWarning("MoneyDisplay.Instance не найден, начальные деньги не установлены");
            CutsceneManager.NotifyGameLoaded();
            return;
        }

        var saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SavePath));

        // Загружаем тайлы
        int restoredTiles = 0;
        foreach (var data in saveFile.tiles)
        {
            var gridPos = FarmGrid.Instance.WorldToGridPosition(data.position);
            var tile = FarmGrid.Instance.GetTileAt(gridPos);
            tile?.GetComponent<SoilTile>()?.LoadFromSaveData(data);
            restoredTiles++;
        }

        // Загружаем растения
        if (saveFile.crops != null && CropsManager.Instance != null)
        {
            foreach (var data in saveFile.crops)
            {
                Vector2Int pos = new Vector2Int(data.gridX, data.gridY);
                
                CropType cropType;
                if (!System.Enum.TryParse(data.cropType, out cropType))
                {
                    Debug.LogError($"Неизвестный тип растения: {data.cropType}");
                    continue;
                }
                
                Crop cropData = CropsManager.Instance.GetCropData(cropType);
                if (cropData == null) continue;
                
                CropBehaviour prefab = CropsManager.Instance.GetCropPrefab(cropType);
                if (prefab == null) continue;
                
                Vector3 worldPos = FarmGrid.Instance.GridToWorldPosition(pos);
                CropBehaviour newCrop = Object.Instantiate(prefab, worldPos, Quaternion.identity);
                newCrop.cropData = cropData;
                
                newCrop.isRotten = data.isRotten;
                
                System.Reflection.FieldInfo stageField = typeof(CropBehaviour).GetField("currentStage", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (stageField != null)
                {
                    stageField.SetValue(newCrop, data.currentStage);
                }
                
                newCrop.UpdateVisual();
                
                CropsManager.Instance.allCrops[pos] = newCrop;
                
                GameObject tileObj = FarmGrid.Instance.GetTileAt(pos);
                if (tileObj != null)
                {
                    SoilTile soil = tileObj.GetComponent<SoilTile>();
                    if (soil != null)
                    {
                        soil.MarkPlanted();
                    }
                }
            }
        }

        // Загружаем игрока
        if (saveFile.player != null)
        {
            if (Player.Instance != null)
            {
                Player.Instance.transform.position = saveFile.player.position;
            }
            else
            {
                pendingPlayerPos = saveFile.player.position;
            }
        }

        // Загружаем козу
        if (saveFile.goat != null)
        {
            var goat = Object.FindFirstObjectByType<GoatBehavior>();
            if (goat != null)
            {
                goat.transform.position = saveFile.goat.position;
            }
            else
            {
                pendingGoatPos = saveFile.goat.position;
            }
        }

        // Загружаем инвентарь
        if (saveFile.inventory != null && saveFile.inventory.items != null && 
            InventoryController.Instance != null && InventoryController.Instance.mainInventory != null)
        {
            var inv = InventoryController.Instance.mainInventory;
            
            for (int i = 2; i < inv.items.Count; i++)
            {
                inv.items[i].id = 0;
                inv.items[i].count = 0;
            }
            
            int slotIndex = 2;
            foreach (var itemData in saveFile.inventory.items)
            {
                if (slotIndex >= inv.items.Count) break;
                
                if (string.IsNullOrEmpty(itemData.itemName))
                {
                    Debug.LogWarning("Пропущен предмет с пустым именем");
                    continue;
                }
                
                Item item = DataBase.Instance.GetItemByName(itemData.itemName, itemData.isSeed);
                if (item != null)
                {
                    inv.items[slotIndex].id = item.id;
                    inv.items[slotIndex].count = itemData.count;
                    slotIndex++;
                }
            }
            
            inv.UpdateInventory();
            InventoryController.Instance.UpdateHotbarVisuals(); // Обновляем хотбар
        }

        // Загружаем деньги
            if (MoneyDisplay.Instance != null)
                MoneyDisplay.Instance.SetMoney(saveFile.money);
            else
                Debug.LogWarning("MoneyDisplay.Instance не найден, деньги не восстановлены");
        
        
        //загружаем экологию
        var ecoController = Object.FindFirstObjectByType<EcologyController>();
        if (ecoController != null)
            ecoController.CurrentEco = saveFile.ecology;

        Debug.Log($"[SaveSystem] Игра загружена. Путь: {SavePath}");

        // Загружаем квесты
        if (saveFile.activeQuests != null)
        {
            QuestManager.Instance.LoadQuestsFromSave(saveFile.activeQuests, saveFile.completedQuests);
        }

        // для концовок
        if (QuestManager.Instance != null)
        {
            typeof(QuestManager).GetField("TeamWolf", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(QuestManager.Instance, saveFile.teamWolf);
            typeof(QuestManager).GetField("TeamFox", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(QuestManager.Instance, saveFile.teamFox);
        }

        // Восстанавливаем диалоговые ключи NPC
        if (saveFile.npcDialogueKey != null)
        {
            var allNPCs = Object.FindObjectsByType<NPCInteraction>(FindObjectsSortMode.None);
            foreach (var npcData in saveFile.npcDialogueKey)
            {
                foreach (var npc in allNPCs)
                {
                    if (npc.npcName == npcData.npcName)
                    {
                        npc.dialogueKey = npcData.dialogueKey;
                        break;
                    }
                }
            }
        }

        // Загружаем флаг интро катсцены
        CutsceneManager.IntroCutscenePlayed = saveFile.introCutscenePlayed;
        CutsceneManager.NotifyGameLoaded();
    }

    // Применяем отложенные позиции, если объекты появились позже
    public static void ApplyPendingPositions()
    {
        if (pendingPlayerPos.HasValue && Player.Instance != null)
        {
            Player.Instance.transform.position = pendingPlayerPos.Value;
            pendingPlayerPos = null;
        }

        if (pendingGoatPos.HasValue)
        {
            var goat = Object.FindFirstObjectByType<GoatBehavior>();
            if (goat != null)
            {
                goat.transform.position = pendingGoatPos.Value;
                pendingGoatPos = null;
            }
        }
    }

    private static List<QuestSaveData> ConvertQuestsToSaveData(List<Quest> quests)
    {
        List<QuestSaveData> result = new List<QuestSaveData> ();
        foreach (var quest in quests)
        {
            QuestSaveData qsd = new QuestSaveData();
            qsd.questName = quest.questName;
            qsd.description = quest.description;
            qsd.completionNotes = quest.completionNotes;
            qsd.rewardMoney = quest.rewardMoney;
            qsd.rewardCropType = quest.rewardCropType.ToString();
            qsd.rewardSeedCount = quest.rewardSeedCount;
            qsd.tasks = new List<TaskSaveData>();
            foreach (var task in quest.tasks)
            {
                TaskSaveData tsd = new TaskSaveData();
                tsd.description = task.description;
                tsd.isCompleted = task.isCompleted;
                qsd.tasks.Add(tsd);
            }

            if (quest.npcDialogueChanges != null)
            {
                qsd.npcDialogueChanges = new List<NPCDialogueSaveData>();
                foreach (var change in quest.npcDialogueChanges)
                {
                    qsd.npcDialogueChanges.Add(new NPCDialogueSaveData
                    {
                        npcName = change.npcName,
                        dialogueKey = change.newDialogueKey
                    });
                }
            }
            result.Add(qsd);
        }
        return result;
    }

    public static void SaveAllTiles() => SaveGame();
    public static void LoadAllTiles() => LoadGame();



     [System.Serializable]
    public class SaveFile
    {
        public List<SaveData> tiles;
        public List<CropSaveData> crops;
        public PlayerData player;
        public GoatData goat;
        public InventoryData inventory;
        public int money;
        public float ecology;
        public bool introCutscenePlayed;
        public List<QuestSaveData> activeQuests;
        public List<QuestSaveData> completedQuests;
        public List<NPCDialogueSaveData> npcDialogueKey;
        public int teamWolf;
        public int teamFox;
    }
    [System.Serializable]
    public class CropSaveData
    {
        public int gridX;
        public int gridY;
        public string cropType;
        public int currentStage;
        public bool isRotten;
    }
    [System.Serializable]
    public class PlayerData
    {
        public Vector2 position;
    }

    [System.Serializable]
    public class GoatData
    {
        public Vector2 position;
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<ItemSlotData> items;
    }

    [System.Serializable]
    public class ItemSlotData
    {
        public string itemName;
        public int count;
        public bool isSeed;
    }

    [System.Serializable]
    public class QuestSaveData
    {
        public string questName;
        public string description;
        public string completionNotes;
        public List<TaskSaveData> tasks;
        public int rewardMoney;
        public string rewardCropType;
        public int rewardSeedCount;
        public List<NPCDialogueSaveData> npcDialogueChanges;
    }

    [System.Serializable]
    public class TaskSaveData
    {
        public string description;
        public bool isCompleted;
    }

    [System.Serializable]
    public class NPCDialogueSaveData
    {
        public string npcName;
        public string dialogueKey;
    }
}
