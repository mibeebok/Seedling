using System.Collections.Generic;
using UnityEngine;

public class CropsManager : MonoBehaviour
{
    public static CropsManager Instance { get; private set; }

    [Header("Crop Prefabs (prefabs with CropBehaviour)")]
    [SerializeField] private CropBehaviour potatoPrefab;
    [SerializeField] private CropBehaviour carrotPrefab;
    [SerializeField] private CropBehaviour beetrootPrefab;
    [SerializeField] private CropBehaviour rastberryPrefab;

    private Dictionary<Vector2Int, CropBehaviour> allCrops = new Dictionary<Vector2Int, CropBehaviour>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);
        var tileObject = FarmGrid.Instance.GetTileAt(gridPos);
        var soilTile = tileObject?.GetComponent<SoilTile>();

        Debug.Log($"Плитка {(tileObject == null ? "не найдена" : "найдена")}, готова ли почва: {soilTile?.IsReadyForPlanting()}");

        if (!CanPlantAt(gridPos)) {
            Debug.Log("CanPlantAt вернул false");
            return false;
        }

        CropBehaviour prefab = GetCropPrefab(seedItem.cropType);
        Debug.Log($"Prefab для {seedItem.cropType}: {(prefab == null ? "НЕ найден" : "найден")}");

            return true;
    /*
        if (!seedItem.IsSeed())
        {
            Debug.Log("Предмет не является семенами");
            return false;
        }

        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);

        if (!CanPlantAt(gridPos))
        {
            Debug.Log($"Нельзя посадить здесь. Готова ли почва: {FarmGrid.Instance.GetTileAt(gridPos)?.GetComponent<SoilTile>()?.IsReadyForPlanting()}");
            return false;
        }

        CropBehaviour prefab = GetCropPrefab(seedItem.cropType);
        if (prefab == null)
        {
            Debug.Log($"Не найден префаб для {seedItem.cropType}");
            return false;
        }

        Vector3 worldPos = FarmGrid.Instance.GridToWorldPosition(gridPos);
        CropBehaviour newCrop = Instantiate(prefab, worldPos, Quaternion.identity);

        // Привяжем данные (если в prefab.cropData не задано, можно назначить ScriptableObject по типу)
        // newCrop.cropData = DataBase.Instance.GetCropSO(seedItem.cropType); // опционально

        allCrops[gridPos] = newCrop;

        FarmGrid.Instance.GetTileAt(gridPos)?.GetComponent<SoilTile>()?.ResetAfterPlanting();

        Debug.Log($"Посажено: {seedItem.name} на позиции {gridPos}");
        return true;*/
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
