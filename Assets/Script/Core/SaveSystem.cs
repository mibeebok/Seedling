using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "farm_save.json");

    // Для случаев, когда объекты ещё не созданы при загрузке
    private static Vector2? pendingPlayerPos = null;
    private static Vector2? pendingGoatPos = null;

    public static void SaveGame()
    {
        var saveFile = new SaveFile();

        // 1️⃣ Сохраняем тайлы
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
        Debug.Log($"[SaveSystem] Сохранено тайлов: {saveFile.tiles.Count}");

        // 2️⃣ Сохраняем игрока
        if (Player.Instance != null)
        {
            saveFile.player = new PlayerData
            {
                position = Player.Instance.transform.position
            };
            Debug.Log($"[SaveSystem] Позиция игрока сохранена: {saveFile.player.position}");
        }

        // 3️⃣ Сохраняем козу
        var goat = Object.FindFirstObjectByType<GoatBehavior>();
        if (goat != null)
        {
            saveFile.goat = new GoatData
            {
                position = goat.transform.position
            };
            Debug.Log($"[SaveSystem] Позиция козы сохранена: {saveFile.goat.position}");
        }

        // 4️⃣ Сохраняем в файл
        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, true));
        Debug.Log($"[SaveSystem] Игра сохранена. Путь: {SavePath}");
    }

    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("[SaveSystem] Файл сохранения не найден!");
            return;
        }

        var saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SavePath));
        Debug.Log("[SaveSystem] Сохранение загружено из файла.");

        // 1️⃣ Загружаем тайлы
        int restoredTiles = 0;
        foreach (var data in saveFile.tiles)
        {
            var gridPos = FarmGrid.Instance.WorldToGridPosition(data.position);
            var tile = FarmGrid.Instance.GetTileAt(gridPos);
            tile?.GetComponent<SoilTile>()?.LoadFromSaveData(data);
            restoredTiles++;
        }
        Debug.Log($"[SaveSystem] Восстановлено тайлов: {restoredTiles}");

        // 2️⃣ Загружаем игрока
        if (saveFile.player != null)
        {
            if (Player.Instance != null)
            {
                Player.Instance.transform.position = saveFile.player.position;
                Debug.Log($"[SaveSystem] Позиция игрока загружена: {saveFile.player.position}");
            }
            else
            {
                pendingPlayerPos = saveFile.player.position;
                Debug.Log("[SaveSystem] Игрок ещё не создан, позиция сохранена в pending.");
            }
        }

        // 3️⃣ Загружаем козу
        if (saveFile.goat != null)
        {
            var goat = Object.FindFirstObjectByType<GoatBehavior>();
            if (goat != null)
            {
                goat.transform.position = saveFile.goat.position;
                Debug.Log($"[SaveSystem] Позиция козы загружена: {saveFile.goat.position}");
            }
            else
            {
                pendingGoatPos = saveFile.goat.position;
                Debug.Log("[SaveSystem] Коза ещё не создана, позиция сохранена в pending.");
            }
        }

        Debug.Log("[SaveSystem] Загрузка завершена!");
    }

    // Применяем отложенные позиции, если объекты появились позже
    public static void ApplyPendingPositions()
    {
        if (pendingPlayerPos.HasValue && Player.Instance != null)
        {
            Player.Instance.transform.position = pendingPlayerPos.Value;
            pendingPlayerPos = null;
            Debug.Log($"[SaveSystem] Применена отложенная позиция игрока: {Player.Instance.transform.position}");
        }

        if (pendingGoatPos.HasValue)
        {
            var goat = Object.FindFirstObjectByType<GoatBehavior>();
            if (goat != null)
            {
                goat.transform.position = pendingGoatPos.Value;
                pendingGoatPos = null;
                Debug.Log($"[SaveSystem] Применена отложенная позиция козы: {goat.transform.position}");
            }
        }
    }

    // Старые методы для совместимости
    public static void SaveAllTiles() => SaveGame();
    public static void LoadAllTiles() => LoadGame();

    // =====================
    // Классы данных
    // =====================
    [System.Serializable]
    public class SaveFile
    {
        public List<SaveData> tiles;
        public PlayerData player;
        public GoatData goat;
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
}
