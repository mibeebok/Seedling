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

    public Dictionary<Vector2Int, CropBehaviour> allCrops =new Dictionary<Vector2Int, CropBehaviour>();
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
    private void Start()
    {
        HouseController.OnNewDay += OnNewDay;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) // Тестовая кнопка
        {
            Debug.Log("=== ТЕСТОВЫЙ ВЫЗОВ OnNewDay ===");
            OnNewDay();
        }
    }
    public void clearAllCrops()
    {
        foreach (var crop in allCrops.Values)
        {
            if (crop != null) Object.Destroy(crop.gameObject);
        }
        allCrops.Clear();
    }

    private void OnDestroy()
    {
        HouseController.OnNewDay -= OnNewDay;
    }

    public Crop GetCropData(CropType type)
    {
        if (cropByType.TryGetValue(type, out Crop crop))
        {
            return crop;
        }
        Debug.LogError($" в allcropData нет записи для {type}");
        return null;
        // return cropByType[type];
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
        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);

        if (allCrops.ContainsKey(gridPos))
            return false;
        

        GameObject tileObject = FarmGrid.Instance.GetTileAt(gridPos);
        if (tileObject == null)
        {
            return false;
        }

        SoilTile soilTile = tileObject.GetComponent<SoilTile>();
        if (soilTile == null)
        {
            return false;
        }

        if (!soilTile.IsReadyForPlanting())
        {
            return false;
        }
        Crop cropData = GetCropData(seedItem.cropType);
        if (cropData == null) return false;

        CropBehaviour prefab = GetCropPrefab(seedItem.cropType);
        if (prefab == null)
        {
            return false;
        }

        Vector3 spawnPosition = FarmGrid.Instance.GridToWorldPosition(gridPos);
        
        CropBehaviour newCrop = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
        
        newCrop.cropData = GetCropData(seedItem.cropType);

        soilTile.MarkPlanted(); 

        allCrops.Add(gridPos, newCrop);
    
        newCrop.UpdateVisual();
        
        soilTile.MarkPlanted();
        
        SleepController sleepController = FindObjectOfType<SleepController>();
        if (sleepController != null)
        {
            sleepController.ConsumeEnergy(5f);
        }

        EcologyController ecoController = FindObjectOfType<EcologyController>();
        if (ecoController != null)
        {
            ecoController.RestoreEco(5f); // 5 единиц экологии за растение
        }
        
        return true;
    }

    
    public CropBehaviour GetCropPrefab(CropType cropType)
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
        var cropKeys = new List<Vector2Int>(allCrops.Keys);
        foreach (var pos in cropKeys)
        {            
            if (!allCrops.TryGetValue(pos, out CropBehaviour crop)) 
            {
                continue;
            }

            if (crop == null)
            {
                allCrops.Remove(pos);
                continue;
            }
            
            GameObject tileObj = FarmGrid.Instance.GetTileAt(pos);
            if (tileObj == null)
            {
                continue;
            }
            
            SoilTile soil = tileObj.GetComponent<SoilTile>();
            if (soil == null)
            {
                continue;
            }
            
            if (crop.isRotten)
            {
                Destroy(crop.gameObject);
                allCrops.Remove(pos);
                soil.MarkHarvested();
                continue;
            }
            if (crop.CurrentStage >= crop.cropData.growthStages.Length - 1)
            {
                Destroy(crop.gameObject);
                allCrops.Remove(pos);
                soil.MarkHarvested();
                
                EcologyController ecoController = FindObjectOfType<EcologyController>();
                if (ecoController != null)
                    ecoController.ReduceEco(3f);
            }
            
            if (soil.isWatered)
            {
                crop.Grow();
            }
            else if (soil.wasWateredYesterday)
            {
                Debug.Log($"Растение на {pos} не полито 1 день");
            }
            else
            {
                Debug.Log($"Растение на {pos} не полито 2 дня - СГНИЛО!");
                crop.SetRotten();
            }
        }
        
        var allTiles = Object.FindObjectsOfType<SoilTile>();
        foreach (var tile in allTiles)
        {
            bool wasJustWatered = tile.isWatered;
            tile.ClearDailyWater();
            if (!wasJustWatered)
            {
                tile.DryOut();
            }
        }
    }

    public void CollectCrop(Vector2Int gridPosition)
    {
        if (allCrops.TryGetValue(gridPosition, out CropBehaviour crop))
        {
            Item harvestItem = crop.Harvest(out int yield);
            if (harvestItem != null && InventoryController.Instance != null)
            {
                InventoryController.Instance.AddItem(harvestItem, yield);
            }

            Destroy(crop.gameObject);
            allCrops.Remove(gridPosition);

            EcologyController ecoController = FindObjectOfType<EcologyController>();
            if (ecoController != null)
                ecoController.ReduceEco(2f);


            GameObject tileObj = FarmGrid.Instance.GetTileAt(gridPosition);
            if (tileObj != null)
            {
                SoilTile soil = tileObj.GetComponent<SoilTile>();
                if (soil != null)
                {
                    soil.MarkHarvested();
                }
            }
        }
    }

    public CropBehaviour GetPlantAt(Vector2Int gridPos)
    {
        Vector3 world = FarmGrid.Instance.GridToWorldPosition(gridPos);
        float radius = 0.3f;

        LayerMask mask = LayerMask.GetMask("Plant");
        Collider2D col = Physics2D.OverlapCircle(world, radius, mask);
        if(col != null)
        {
            return col.GetComponentInParent<CropBehaviour>() ?? col.GetComponent<CropBehaviour>();

        }
        return null;
    }
    
}
