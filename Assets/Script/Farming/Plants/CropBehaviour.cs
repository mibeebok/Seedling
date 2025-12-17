using UnityEngine;
using UnityEngine.Tilemaps;

public class CropBehaviour : MonoBehaviour
{
    [Header("Crop Data")]
    public Crop cropData; // ScriptableObject с данными о растении

    private int currentStage = 0;
    private SpriteRenderer spriteRenderer;
    private bool isRotten = false;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Check camera
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

        if (Input.GetKeyDown(KeyCode.I)) // Info button
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

//следующие 2 метода логирование
    private void OnDrawGizmosSelected()
    {
        // Рисуем красный куб вокруг растения в редакторе
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
        
        // Рисуем зеленую линию вверх
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.3f);
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.2f);
        }
    }

    private void UpdateSorting()
    {
        spriteRenderer.sortingLayerName = "Plants";
        spriteRenderer.sortingOrder = 5;
    }

    public void Grow()
    {
        Debug.Log($"Метод Grow вызван для {gameObject.name}");
        Debug.Log($"CropData: {cropData?.name}");
        Debug.Log($"CurrentStage: {currentStage}");
        if (isRotten) 
        {
            Debug.Log("Растение испорчено, не может расти");
            return;
        }
        
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

        Debug.Log($"Текущая стадия: {currentStage}, Всего стадий: {cropData.growthStages.Length}");
        
        if (currentStage < cropData.growthStages.Length - 1)
        {
            currentStage++;
            UpdateVisual();
            Debug.Log($"✓ Растение выросло до стадии {currentStage + 1}/{cropData.growthStages.Length}");
        }
        else
        {
            Debug.Log("Растение достигло максимальной стадии роста и готово к сбору");
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
                
                // ДОБАВЬТЕ ЭТО ДЛЯ ОТЛАДКИ:
                Debug.Log($"=== ОТЛАДКА РАСТЕНИЯ ===");
                Debug.Log($"Позиция: {transform.position}");
                Debug.Log($"Масштаб: {transform.localScale}");
                Debug.Log($"Слой сортировки: {spriteRenderer.sortingLayerName}");
                Debug.Log($"Порядок сортировки: {spriteRenderer.sortingOrder}");
                Debug.Log($"Спрайт установлен: {tile.sprite?.name}");
                Debug.Log($"Размер спрайта: {tile.sprite?.bounds.size}");
                Debug.Log($"Видимость Renderer: {spriteRenderer.enabled}");
            }
            return;
        }

        Debug.LogWarning("CropBehaviour: growthStages[currentStage] не является Tile с sprite.");
    }
    public Item Harvest()
    {
        return cropData?.harvestItem;
    }
    public void SetRotten()
    {
        if (cropData == null || cropData.growthStages.Length == 0) return;

        isRotten = true;
        currentStage = cropData.growthStages.Length - 1;
        UpdateVisual();
    }
    
}
