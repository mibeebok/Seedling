using UnityEngine;
using UnityEngine.Tilemaps;

public class CropBehaviour : MonoBehaviour
{
    [Header("Crop Data")]
    public Crop cropData; // ScriptableObject с данными о растении

    private int currentStage = 0;
    public int CurrentStage => currentStage;
    private SpriteRenderer spriteRenderer;
    public bool isRotten = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

            UpdateSorting();
            UpdateVisual();
    }
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        transform.localScale = new Vector3(2f, 2f, 2f);
        
        Vector3 pos = transform.position;
        pos.z = -0.1f;
        transform.position = pos;

        UpdateSorting();
        UpdateVisual();
        
        currentStage = 0;

        spriteRenderer.sortingLayerName = "Default";
        spriteRenderer.sortingOrder = 0;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
                Debug.Log($"Экранная позиция: {screenPos}");
                
                // Если Z < 0 - растение за камерой
                if (screenPos.z < 0)
                {
                    Debug.LogError("Растение ЗА КАМЕРОЙ! Z = " + screenPos.z);
                    // Переместите растение ближе к камере
                    Vector3 pos = transform.position;
                    pos.z = -5f; // Ближе к камере
                    transform.position = pos;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("=== ИНФОРМАЦИЯ О РАСТЕНИИ ===");
            Debug.Log($"Name: {gameObject.name}");
            Debug.Log($"Position: {transform.position}");
            Debug.Log($"Local Position: {transform.localPosition}");
            Debug.Log($"Scale: {transform.localScale}");
            Debug.Log($"Active: {gameObject.activeInHierarchy}");
            
            if (spriteRenderer != null)
            {
                Debug.Log($"Renderer enabled: {spriteRenderer.enabled}");
                Debug.Log($"Sorting Layer: {spriteRenderer.sortingLayerName} (ID: {spriteRenderer.sortingLayerID})");
                Debug.Log($"Sorting Order: {spriteRenderer.sortingOrder}");
                Debug.Log($"Sprite: {spriteRenderer.sprite?.name ?? "null"}");
                Debug.Log($"Color: {spriteRenderer.color}");
                Debug.Log($"Bounds: {spriteRenderer.bounds}");
            }
        }
    }

    private void UpdateSorting()
    {
        spriteRenderer.sortingLayerName = "Plants";
        spriteRenderer.sortingOrder = 5;
    }
    private void OnMouseDown()
    {
        if (currentStage >= 2 && currentStage <= 3)
        {
            CollectHarvest();
        }
    }
    private string GetCropName(CropType type)
    {
        switch (type)
        {
            case CropType.Potato: return "Картофель";
            case CropType.Carrot: return "Морковь";
            case CropType.Beetroot: return "Свёкла";
            case CropType.Rastberry: return "Малина";
            default: return "Новый вид!";
        }
    }
    private void OnMouseEnter()
    {
        if (CropInfoUI.Instance != null && cropData != null)
        {
            string statusText;
            
            if (currentStage >= 4)
            {
                statusText = "Ты опоздал. Растение испорчено!";
            }
            else if (currentStage >= 2 && currentStage <= 3)
            {
                statusText = "Можно собирать!";
            }
            else
            {
                statusText = "Еще растет...";
            }
            string fullText =$"Это {GetCropName(cropData.cropType)}\n{statusText}";
            
            CropInfoUI.Instance.ShowInfo(transform.position, fullText);
        }
    }
    private void OnMouseExit()
    {
        if (CropInfoUI.Instance != null)
            CropInfoUI.Instance.HideInfo();
    }

    private void CollectHarvest()
    {
        if (CropInfoUI.Instance != null)
        {
            CropInfoUI.Instance.HideInfo();
        }
        Item harvestedItem = Harvest(out int yield);
        if (harvestedItem != null && InventoryController.Instance != null)
        {
            InventoryController.Instance.AddItem(harvestedItem, yield);
        }

        Vector2Int gridPos = FarmGrid.Instance.WorldToGridPosition(transform.position);
        if (CropsManager.Instance.allCrops.ContainsKey(gridPos))
        {
            CropsManager.Instance.allCrops.Remove(gridPos);
        }

        GameObject tileObj = FarmGrid.Instance.GetTileAt(gridPos);
        if (tileObj != null)
        {
            SoilTile soil = tileObj.GetComponent<SoilTile>();
            if (soil != null)
            {
                soil.MarkHarvested();
            }
        }

        Destroy(gameObject);
    }

    public void Grow()
    {
        if (cropData == null)
        {
            Debug.LogError("CropData не назначен!");
            return;
        }
        
        if (cropData.growthStages == null || cropData.growthStages.Length == 0)
        {
            Debug.LogError($"GrowthStages не настроены для {cropData.name}!");
            return;
        }
        
        if (isRotten) 
        {
            return;
        }

        if (currentStage < cropData.growthStages.Length - 1)
        {
            currentStage++;
            UpdateVisual();
        }
    }

   public void UpdateVisual()
    {
        if (cropData == null || cropData.growthStages == null || cropData.growthStages.Length == 0) return;
        
        TileBase tileBase = cropData.growthStages[currentStage];
        if (tileBase == null) return;

        var tile = tileBase as Tile;
        if (tile != null)
        {
            if (spriteRenderer != null) 
            {
                spriteRenderer.sprite = tile.sprite;
            }
            return;
        }
    }
    public Item Harvest(out int yield)
    {
        if (cropData == null || cropData.harvestItem == null)
        {
            yield = 0;
            return null;
        }

        int totalStages = cropData.growthStages.Length;
        float growthProgress = (float)currentStage / (totalStages - 1);
        
        yield = Mathf.RoundToInt(Mathf.Lerp(cropData.baseHarvestYield, cropData.maxHarvestYield, growthProgress));
        
        return cropData.harvestItem;
    }
    
    public void SetRotten()
    {
        if (cropData == null || cropData.growthStages.Length == 0) return;

        isRotten = true;
        currentStage = cropData.growthStages.Length - 1;
        UpdateVisual();
    }

    private void OnDestroy()
    {
        if (CropInfoUI.Instance != null)
        {
            CropInfoUI.Instance.HideInfo();
        }
    }
}
