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

    public static void SaveModifiedTiles()
    {
        if (changedTiles.Count == 0)
        {
            Debug.Log("[SaveSystem] Нет изменённых тайлов для сохранения.");
            return;
        }

        SaveFile saveFile;

        // Если файл уже существует — загружаем, чтобы не потерять старые данные
        if (File.Exists(SavePath))
            saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SavePath));
        else
            saveFile = new SaveFile { tiles = new List<SaveData>() };

        // Словарь для быстрого поиска существующих тайлов
        var tileMap = new Dictionary<Vector2Int, SaveData>();
        foreach (var t in saveFile.tiles)
        {
            var pos = FarmGrid.Instance.WorldToGridPosition(t.position);
            tileMap[pos] = t;
        }

        // Обновляем только изменённые
        foreach (var pos in changedTiles)
        {
            var tile = FarmGrid.Instance.GetTileAt(pos);
            if (tile != null)
            {
                var soil = tile.GetComponent<SoilTile>();
                if (soil != null)
                    tileMap[pos] = soil.GetSaveData();
            }
        }

        // Перезаписываем обновлённый список
        saveFile.tiles = new List<SaveData>(tileMap.Values);

        // Сохраняем остальное (игрок, коза, инвентарь — по желанию)
        if (Player.Instance != null)
            saveFile.player = new PlayerData { position = Player.Instance.transform.position };

        var goat = Object.FindFirstObjectByType<GoatBehavior>();
        if (goat != null)
            saveFile.goat = new GoatData { position = goat.transform.position };

        if (InventoryController.Instance != null && InventoryController.Instance.mainInventory != null)
        {
            var inv = InventoryController.Instance.mainInventory;
            saveFile.inventory = new InventoryData
            {
                items = new List<ItemSlotData>()
            };
            for (int i = 2; i < inv.items.Count; i++)
            {
                var slot = inv.items[i];
                if (slot.id != 0 && slot.count > 0)  // ← Сохраняем только не пустые слоты
                {
                    Item item = DataBase.Instance.GetItemById(slot.id);
                    if (item != null && !string.IsNullOrEmpty(item.name))  // ← Проверяем что имя не пустое
                    {
                        saveFile.inventory.items.Add(new ItemSlotData
                        {
                            itemName = item.name,
                            count = slot.count,
                            isSeed = item.type == ItemType.Seed
                        });
                    }
                }
            }
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, true));

        changedTiles.Clear();
    }

    public static void SaveGame()
    {
        var saveFile = new SaveFile();

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
            foreach (var kvp in CropsManager.Instance.allCrops)
            {
                var crop = kvp.Value;
                if (crop != null && crop.cropData != null)
                {
                    saveFile.crops.Add(new CropSaveData
                    {
                        gridX = kvp.Key.x,
                        gridY = kvp.Key.y,
                        cropType = crop.cropData.cropType.ToString(),
                        currentStage = crop.CurrentStage,
                        isRotten = crop.isRotten
                    });
                }
            }
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

        // 5 Записываем в файл
        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, true));
        Debug.Log($"[SaveSystem] Игра сохранена. Путь: {SavePath}");
    }


    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
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
    }
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
}
