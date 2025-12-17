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

    private void OnDestroy()
    {
        HouseController.OnNewDay -= OnNewDay;
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
        Debug.Log($"=== ПОСАДКА СЕМЕНИ ===");
        Debug.Log($"Кликнули на позиции мира: {worldPosition}");
        
        // 2 — Получаем клетку
        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(worldPosition);
        Debug.Log($"Позиция сетки: {gridPos}");
        
        // 3 — Проверяем в словаре, что клетка пустая
        if (allCrops.ContainsKey(gridPos))
        {
            Debug.Log($"На клетке {gridPos} уже есть растение!");
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
        Vector3 spawnPosition = FarmGrid.Instance.GridToWorldPosition(gridPos);
        Debug.Log($"Обратно в мир: {spawnPosition}");
        
        CropBehaviour newCrop = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity
        );
        
        Debug.Log($"Растение создано на позиции: {newCrop.transform.position}");
        Debug.Log($"Разница: {worldPosition} → {gridPos} → {spawnPosition}");
        // 6.5 — назначаем ScriptableObject
        newCrop.cropData = GetCropData(seedItem.cropType);

        // 6.6 — помечаем тайл как посаженный (в SoilTile)
        soilTile.MarkPlanted(); 

        // 7 — Добавляем в словарь
        allCrops.Add(gridPos, newCrop);
    
        // 8 — Немедленно обновляем визуал растения
        newCrop.UpdateVisual();
        
        // 9 — Помечаем как посаженное
        soilTile.MarkPlanted();
        
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
        Debug.Log("=== НАЧАЛСЯ НОВЫЙ ДЕНЬ ===");
        Debug.Log($"Всего растений: {allCrops.Count}");

        
        // Шаг 1: Сначала обрабатываем ВСЕ растения - рост
        var cropKeys = new List<Vector2Int>(allCrops.Keys);
        foreach (var pos in cropKeys)
        {
            Debug.Log($"Обработка растения на позиции {pos}");
            
            if (!allCrops.TryGetValue(pos, out CropBehaviour crop)) 
            {
                Debug.Log($"Растение не найдено в словаре для позиции {pos}");
                continue;
            }

            GameObject tileObj = FarmGrid.Instance.GetTileAt(pos);
            if (tileObj == null)
            {
                Debug.Log($"Тайл не найден для позиции {pos}");
                continue;
            }
            
            SoilTile soil = tileObj.GetComponent<SoilTile>();
            if (soil == null)
            {
                Debug.Log($"SoilTile не найден для позиции {pos}");
                Destroy(crop.gameObject);
                allCrops.Remove(pos);
                continue;
            }

            // Проверяем состояние почвы для растения
            if (soil.isWatered)
            {
                // Если почва полита СЕГОДНЯ - растение растет
                Debug.Log($"Растение на {pos} полито, растет...");
                crop.Grow();
                soil.daysWithoutWater = 0; // Сбрасываем счетчик дней без воды
            }
            else if (soil.wasWateredYesterday)
            {
                // Если почва была полита ВЧЕРА (но не сегодня) - все еще может расти
                Debug.Log($"Растение на {pos} было полито вчера, все еще может расти...");
                crop.Grow();
                soil.daysWithoutWater = 1; // Один день без воды
            }
            else
            {
                // Не поливалось ни сегодня, ни вчера
                soil.daysWithoutWater++;
                Debug.Log($"Растение на {pos} не поливалось. Дней без воды: {soil.daysWithoutWater}");

                if (soil.daysWithoutWater >= 2)
                {
                    crop.SetRotten();
                    Debug.Log($"Растение на {pos} испортилось из-за недостатка воды");
                }
                else
                {
                    // Может все еще расти один день без воды
                    crop.Grow();
                }
            }
        }

        // Шаг 2: Затем обрабатываем высыхание ВСЕЙ почвы
        var allTiles = Object.FindObjectsOfType<SoilTile>();
        Debug.Log($"Обработка высыхания {allTiles.Length} тайлов почвы");
        
        foreach (var tile in allTiles)
        {
            // Сохраняем состояние "полито вчера" перед сбросом
            bool wasJustWatered = tile.isWatered;
            
            // Сбрасываем полив "сегодня" для следующего дня
            tile.ClearDailyWater(); // Этот метод теперь сохраняет wasWateredYesterday
            
            // Если не было полито сегодня, обрабатываем высыхание
            if (!wasJustWatered)
            {
                tile.DryOut();
            }
        }
        
        Debug.Log("=== НОВЫЙ ДЕНЬ ЗАВЕРШЕН ===");
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
