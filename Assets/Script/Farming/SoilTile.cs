using UnityEngine;

public class SoilTile : MonoBehaviour
{
    [Header("State")]
    public bool isPlowed = false;
    public bool isWatered = false; // Полита СЕГОДНЯ
    public bool wasWateredYesterday = false; // Полита ВЧЕРА (добавить это поле)
    public bool isPlanted = false;
    public int daysWithoutWater = 0;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite plowedSprite;
    public Sprite plowedDrySprite; // Сухая вспаханная земля
    public Sprite wateredSprite; // Полита СЕГОДНЯ
    public Sprite wateredYesterdaySprite; // Полита ВЧЕРА (нужен полив сегодня) - добавьте этот спрайт

    [HideInInspector] public SpriteRenderer spriteRenderer;
    private SoilTileWateringCan wateringCan;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wateringCan = GetComponent<SoilTileWateringCan>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = -1;
            spriteRenderer.sortingLayerName = "Default";
        }
        UpdateVisual();
    }

    // -----------------------
    // VISUAL UPDATE
    // -----------------------
    public void UpdateVisual()
    {
        if (isWatered && wateredSprite != null)
        {
            // Полита СЕГОДНЯ (голубой эффект)
            spriteRenderer.sprite = wateredSprite;
        }
        else if (wasWateredYesterday && wateredYesterdaySprite != null)
        {
            // Полита ВЧЕРА (нужен полив сегодня)
            spriteRenderer.sprite = wateredYesterdaySprite;
        }
        else if (isPlowed && !isWatered && daysWithoutWater > 0 && plowedDrySprite != null)
        {
            // Сухая вспаханная земля
            spriteRenderer.sprite = plowedDrySprite;
        }
        else if (isPlowed && plowedSprite != null)
        {
            // Свежевспаханная земля
            spriteRenderer.sprite = plowedSprite;
        }
        else
        {
            spriteRenderer.sprite = normalSprite;
        }
    }

    // -----------------------
    // ACTIONS
    // -----------------------
    public void Plow()
    {
        isPlowed = true;
        wasWateredYesterday = false; // Сбрасываем
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void Water()
    {
        isWatered = true;
        wasWateredYesterday = false; // Если полили сегодня, то "вчерашний" полив сбрасывается
        daysWithoutWater = 0;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public bool IsReadyForPlanting()
    {
        // Можно сажать только если полита СЕГОДНЯ
        return isPlowed && isWatered && !isPlanted;
    }

    public void MarkPlanted()
    {
        isPlanted = true;
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void MarkHarvested()
    {
        isPlanted = false;
        daysWithoutWater = 0;
        isWatered = false;
        wasWateredYesterday = false;
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void ClearDailyWater()
    {
        // При переходе на новый день:
        // 1. Если была полита сегодня -> становится полита вчера
        // 2. Сбрасываем полив сегодня
        if (isWatered)
        {
            wasWateredYesterday = true;
        }
        isWatered = false;
        
        UpdateVisual();
        SaveSystem.MarkTileChanged(GetGridPos());
    }

    public void DryOut()
    {
        if (isPlowed && !isWatered)
        {
            daysWithoutWater++;
            
            // Если земля не поливалась 2 дня
            if (daysWithoutWater >= 2)
            {
                isPlowed = false;
                wasWateredYesterday = false;
                isPlanted = false;
                daysWithoutWater = 0;
                
                // Удаляем растение если есть
                var gridPos = GetGridPos();
                if (CropsManager.Instance != null && CropsManager.Instance.allCrops.ContainsKey(gridPos))
                {
                    var crop = CropsManager.Instance.allCrops[gridPos];
                    if (crop != null)
                    {
                        Destroy(crop.gameObject);
                    }
                    CropsManager.Instance.allCrops.Remove(gridPos);
                }
                
                UpdateVisual();
                SaveSystem.MarkTileChanged(gridPos);
                Debug.Log($"Почва высохла и вернулась в обычное состояние на {gridPos}");
            }
            else if (daysWithoutWater == 1)
            {
                // Просто обновляем визуал для сухой земли
                UpdateVisual();
            }
        }
    }

    // -----------------------
    // SAVE / LOAD
    // -----------------------
    public SaveData GetSaveData()
    {
        return new SaveData
        {
            position = transform.position,
            isPlowed = isPlowed,
            isWatered = isWatered,
            isPlanted = isPlanted,
            daysWithoutWater = daysWithoutWater,
            wasWateredYesterday = wasWateredYesterday // Добавить в SaveData
        };
    }

    public void LoadFromSaveData(SaveData data)
    {
        isPlowed = data.isPlowed;
        isWatered = data.isWatered;
        isPlanted = data.isPlanted;
        daysWithoutWater = data.daysWithoutWater;
        wasWateredYesterday = data.wasWateredYesterday;

        UpdateVisual();
    }

    // -----------------------
    private Vector2Int GetGridPos()
    {
        return FarmGrid.Instance.WorldToGridPosition(transform.position);
    }
}