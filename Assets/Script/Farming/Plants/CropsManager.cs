using System.Collections.Generic;
using UnityEngine;

public class CropsManager : MonoBehaviour
{
    [Header("Crops Data")]
    public List<Crop> allCropData;

    private Dictionary<CropType, Crop> cropByType = new Dictionary<CropType, Crop>();

    public static CropsManager Instance { get; private set; }

    [Header("Crop Prefabs (prefabs with CropBehaviour)")]
    [SerializeField] private CropBehaviour potatoPrefab;
    [SerializeField] private CropBehaviour carrotPrefab;
    [SerializeField] private CropBehaviour beetrootPrefab;
    [SerializeField] private CropBehaviour rastberryPrefab;

    private Dictionary<Vector2Int, CropBehaviour> allCrops =new Dictionary<Vector2Int, CropBehaviour>();
    private Dictionary<Vector2Int, CropBehaviour> plantedCrops = 
    new Dictionary<Vector2Int, CropBehaviour>();


    private void Awake()
    {
        Instance = this;

        foreach (var crop in allCropData)
        {
            if (!cropByType.ContainsKey(crop.cropType))
                cropByType.Add(crop.cropType, crop);
        }

        if (Instance == null) Instance = this;
        else return;
    }

    public Crop GetCropData(CropType type)
    {
        return cropByType[type];
    }

    public bool CanPlantAt(Vector2Int gridPosition)
    {
        GameObject tileObject = FarmGrid.Instance.GetTileAt(gridPosition);
        if (tileObject == null) return false;

        var soilTile = tileObject.GetComponent<SoilTile>();
        return soilTile != null && soilTile.IsReadyForPlanting() &&
               !allCrops.ContainsKey(gridPosition);
    }

    public bool TryPlantSeed(Item seedItem, Vector2 worldPosition)
    {
        Debug.Log($"Пробуем посадить {seedItem.name}, тип: {seedItem.type}, культура: {seedItem.cropType}");

        // 1 — Проверяем, что это семена
        if (!seedItem.IsSeed())
        {
            Debug.Log("Этот предмет НЕ семена");
            return false;
        }

        // 2 — Получаем клетку
        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);

        // 3 — Проверяем в словаре, что клетка пустая
        if (allCrops.ContainsKey(gridPos))
        {
            Debug.Log("На этой клетке уже растёт растение!");
            return false;
        }

        // 4 — Проверяем тайл почвы
        GameObject tileObject = FarmGrid.Instance.GetTileAt(gridPos);
        if (tileObject == null)
        {
            Debug.Log("Тайл не найден!");
            return false;
        }

        SoilTile soilTile = tileObject.GetComponent<SoilTile>();
        if (soilTile == null)
        {
            Debug.Log("На тайле нет SoilTile!");
            return false;
        }

        if (!soilTile.IsReadyForPlanting())
        {
            Debug.Log("Почва НЕ готова для посадки!");
            return false;
        }

        // 5 — Получаем префаб культуры
        CropBehaviour prefab = GetCropPrefab(seedItem.cropType);
        if (prefab == null)
        {
            Debug.LogError($"❌ Префаб культуры для {seedItem.cropType} НЕ найден!");
            return false;
        }

        // 6 — Создаём объект растения
        CropBehaviour newCrop = Instantiate(
            prefab,
            FarmGrid.Instance.GridToWorldPosition(gridPos),
            Quaternion.identity
        );

        // 6.5 — Должно быть! Устанавливаем ScriptableObject культуры
        // ВАЖНО: присваиваем ScriptableObject культуры
        newCrop.cropData = GetCropData(seedItem.cropType);

        // 7 — Добавляем в словарь
        allCrops.Add(gridPos, newCrop);
        soilTile.ResetAfterPlanting();

        // 8 — Сбрасываем состояние почвы, т.к. растение посажено
        Debug.Log($"✔ Посажено: {seedItem.cropType} на {gridPos}");
        return true;
    }


    private CropBehaviour GetCropPrefab(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Potato: return potatoPrefab;
            case CropType.Carrot: return carrotPrefab;
            case CropType.Beetroot: return beetrootPrefab;
            case CropType.Rastberry: return rastberryPrefab;
            default: return null;
        }
    }

    public void OnNewDay()
    {
        foreach (var crop in allCrops.Values)
        {
            if (crop != null) crop.Grow();
        }

        FindFirstObjectByType<SoilTileWateringCan>()?.ResetAllWateredTiles();

    }

    public void CollectCrop(Vector2Int gridPosition)
    {
        if (allCrops.TryGetValue(gridPosition, out CropBehaviour crop))
        {
            Item harvestItem = crop.Harvest();
            if (harvestItem != null && InventoryController.Instance != null)
            {
                InventoryController.Instance.AddItem(harvestItem, 1);
            }

            Destroy(crop.gameObject);
            allCrops.Remove(gridPosition);
        }
    }
}
