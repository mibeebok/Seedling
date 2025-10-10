using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "farm_save.json");


    private static Vector2? pendingPlayerPos = null;
    private static Vector2? pendingGoatPos = null;

    public static void SaveGame()
    {
        var saveFile = new SaveFile();

        // 1. Тайлы
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

        // 2. Игрок
        if (Player.Instance != null)
        {
            saveFile.player = new PlayerData
            {
                position = Player.Instance.transform.position
            };
            Debug.Log($"[SaveSystem] Позиция игрока сохранена: {saveFile.player.position}");
        }

        // 3. Коза
        var goat = Object.FindFirstObjectByType<GoatBehavior>();
        if (goat != null)
        {
            saveFile.goat = new GoatData
            {
                position = goat.transform.position
            };
            Debug.Log($"[SaveSystem] Позиция козы сохранена: {saveFile.goat.position}");
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(saveFile, true));
        Debug.Log($"Game saved to {SavePath}");
    }
    

    public static void LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("No save file found!");
            return;
        }

        var saveFile = JsonUtility.FromJson<SaveFile>(File.ReadAllText(SavePath));

        // 1. Загружаем тайлы
        foreach (var data in saveFile.tiles)
        {
            var gridPos = FarmGrid.Instance.WorldToGridPosition(data.position);
            var tile = FarmGrid.Instance.GetTileAt(gridPos);
            tile?.GetComponent<SoilTile>()?.LoadFromSaveData(data);
        }

        // 2. Загружаем игрока
        if (saveFile.player != null && Player.Instance != null)
        {
            saveFile.player = new PlayerData
            {
                position = Player.Instance.transform.position
            };
        }

        // 3. Загружаем козу
        var goat = Object.FindFirstObjectByType<GoatBehavior>();
        if (saveFile.goat != null && goat != null)
        {
            goat.transform.position = saveFile.goat.position;
        }
        if (saveFile.player != null)
        {
            if (Player.Instance != null)
                Player.Instance.transform.position = saveFile.player.position;
            else
                pendingPlayerPos = saveFile.player.position;
        }

        if (saveFile.goat != null)
        {
            if (goat != null)
                goat.transform.position = saveFile.goat.position;
            else
                pendingGoatPos = saveFile.goat.position;
        }

        Debug.Log("Game loaded!");
    }
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
