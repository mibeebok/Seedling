using UnityEngine;
using System.Collections.Generic;

public class GoatBehavior : Sounds
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float soundPlayRadius = 3f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Sound Settings")]
    [SerializeField] private float playInterval = 6f;
    [SerializeField] private float minVolume = 0.1f;
    [SerializeField] private float maxVolume = 0.3f;

    [Header("Animation Settings")]
    [SerializeField] private Animator goatAnimator;
    [SerializeField] private string scaredParameter = "IsScared";
    [SerializeField] private string movingParameter = "IsMoving"; // если есть анимация ходьбы

    [Header("Respawn Settings")]
    [SerializeField] private Vector2 spawnAreaMin;
    [SerializeField] private Vector2 spawnAreaMax;
    [SerializeField] private float minDistanceFromPlayer = 10f;

    [Header("Eating Settings")]
    [SerializeField] private float eatRadius = 1.5f;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float playerAbsenceTimeToTarget = 300f; // 5 минут в секундах
    [SerializeField] private float targetReachedDistance = 0.3f;
    [SerializeField] private float retargetCooldown = 10f; // как часто пересматривать цель

    private Transform player;
    private bool isPlayerNear;
    private float soundTimer = 0f;

    // Для движения к цели
    private Vector2? targetPosition = null;
    private Vector2Int? targetCropGridPos = null;
    private float timeSinceLastTargetCheck = 0f;
    private Dictionary<Vector2Int, float> cropPlayerAbsenceTimers = new Dictionary<Vector2Int, float>();
    private GoatEscape escapeScript;
    private void OnNewDayHandler()
    {
        // Коза должна быть активна, даже если убежала за экран
        gameObject.SetActive(true);
        
        // Сначала пробуем съесть растение
        TryEatNearbyCrop();
        
        // Затем респавним в новое место
        RespawnGoat();
    }    private void Start()
    {
        SaveSystem.ApplyPendingPositions();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (goatAnimator == null)
        {
            goatAnimator = GetComponent<Animator>();
            if (goatAnimator == null)
                Debug.LogError("У козы не установлена анимация!");
        }

        escapeScript = GetComponent<GoatEscape>();
        if (escapeScript == null)
            Debug.LogWarning("GoatEscape не найден на козе! Убегание не будет работать.");

        HouseController.OnNewDay += OnNewDayHandler;
    }

    private void OnDestroy()
    {
        HouseController.OnNewDay -= OnNewDayHandler;
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isPlayerNear = distanceToPlayer <= detectionRadius;

        // Анимация испуга
        goatAnimator.SetBool(scaredParameter, isPlayerNear);

        // Звуки
        soundTimer += Time.deltaTime;
        if (soundTimer >= playInterval)
        {
            if (distanceToPlayer <= soundPlayRadius)
            {
                float volume = Mathf.Lerp(maxVolume, minVolume, distanceToPlayer / soundPlayRadius);
                PlaySound(sounds[0], volume: volume, p1: 0.9f, p2: 1.3f);
            }
            soundTimer = 0;
        }

        // Если игрок близко — убегаем через GoatEscape, сбрасываем цель
        if (isPlayerNear)
        {
            targetPosition = null;
            targetCropGridPos = null;
            goatAnimator.SetBool(movingParameter, false);
            if (escapeScript != null)
                escapeScript.enabled = true; // GoatEscape сам управляет движением
            return;
        }
        else
        {
            if (escapeScript != null)
                escapeScript.enabled = false; // отключаем убегание, коза спокойна
        }

        // Обновляем таймеры отсутствия игрока у растений
        UpdateCropAbsenceTimers();

        // Периодически выбираем цель
        timeSinceLastTargetCheck += Time.deltaTime;
        if (timeSinceLastTargetCheck >= retargetCooldown || targetPosition == null)
        {
            timeSinceLastTargetCheck = 0f;
            ChooseTargetCrop();
        }

        // Двигаемся к цели, если она есть
        if (targetPosition.HasValue)
        {
            MoveToTarget();
        }
        else
        {
            goatAnimator.SetBool(movingParameter, false);
        }
    }

    private void UpdateCropAbsenceTimers()
    {
        if (CropsManager.Instance == null || FarmGrid.Instance == null) return;

        foreach (var kvp in CropsManager.Instance.allCrops)
        {
            Vector2Int gridPos = kvp.Key;
            Vector3 cropWorldPos = FarmGrid.Instance.GridToWorldPosition(gridPos);
            float distToPlayer = Vector2.Distance(cropWorldPos, player.position);

            if (!cropPlayerAbsenceTimers.ContainsKey(gridPos))
                cropPlayerAbsenceTimers[gridPos] = 0f;

            if (distToPlayer <= detectionRadius)
            {
                cropPlayerAbsenceTimers[gridPos] = 0f; // игрок рядом — сброс
            }
            else
            {
                cropPlayerAbsenceTimers[gridPos] += Time.deltaTime;
            }
        }
    }

    private void ChooseTargetCrop()
    {
        targetPosition = null;
        targetCropGridPos = null;

        if (CropsManager.Instance == null || CropsManager.Instance.allCrops.Count == 0)
            return;

        List<Vector2Int> eligibleCrops = new List<Vector2Int>();
        foreach (var kvp in cropPlayerAbsenceTimers)
        {
            if (kvp.Value >= playerAbsenceTimeToTarget && CropsManager.Instance.allCrops.ContainsKey(kvp.Key))
                eligibleCrops.Add(kvp.Key);
        }

        if (eligibleCrops.Count == 0)
            return;

        // Выбираем ближайшее к козе
        Vector2 goatPos = transform.position;
        Vector2Int bestCrop = eligibleCrops[0];
        float minDist = Vector2.Distance(goatPos, FarmGrid.Instance.GridToWorldPosition(bestCrop));

        foreach (var cropPos in eligibleCrops)
        {
            float dist = Vector2.Distance(goatPos, FarmGrid.Instance.GridToWorldPosition(cropPos));
            if (dist < minDist)
            {
                minDist = dist;
                bestCrop = cropPos;
            }
        }

        targetCropGridPos = bestCrop;
        Vector3 cropWorld = FarmGrid.Instance.GridToWorldPosition(bestCrop);
        targetPosition = new Vector2(cropWorld.x, cropWorld.y);

        Debug.Log($"Коза выбрала цель: растение на {bestCrop}, расстояние {minDist:F2}");
    }

    private void MoveToTarget()
    {
        if (!targetPosition.HasValue) return;

        Vector2 currentPos = transform.position;
        Vector2 direction = (targetPosition.Value - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, targetPosition.Value);

        if (distance <= targetReachedDistance)
        {
            // Достигли цели — останавливаемся
            transform.position = targetPosition.Value;
            targetPosition = null;
            goatAnimator.SetBool(movingParameter, false);
            return;
        }

        // Двигаемся
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
        goatAnimator.SetBool(movingParameter, true);

        // Поворот спрайта (если нужно)
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(direction.x) * Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private void RespawnGoat()
    {
        Vector2 newPosition = Vector2.zero;
        bool placedNearCrop = false;

        // 1. Пробуем найти растение
        if (CropsManager.Instance != null && CropsManager.Instance.allCrops.Count > 0)
        {
            // Получаем список всех позиций растений
            var cropPositions = new List<Vector2Int>(CropsManager.Instance.allCrops.Keys);
            
            // Выбираем случайное растение
            Vector2Int randomCropGridPos = cropPositions[Random.Range(0, cropPositions.Count)];
            
            // Конвертируем позицию сетки в мировые координаты
            Vector3 cropWorldPos = FarmGrid.Instance.GridToWorldPosition(randomCropGridPos);
            
            // Случайное смещение в пределах одной клетки, чтобы не стоять прямо на растении
            float offsetX = Random.Range(-0.4f, 0.4f);
            float offsetY = Random.Range(-0.4f, 0.4f);
            
            newPosition = new Vector2(cropWorldPos.x + offsetX, cropWorldPos.y + offsetY);
            placedNearCrop = true;
            
            Debug.Log($"Коза появилась рядом с растением на {randomCropGridPos}");
        }

        // 2. Если растений нет — используем старую зону
        if (!placedNearCrop)
        {
            int attempts = 0;
            const int maxAttempts = 30;
            
            do
            {
                newPosition = new Vector2(
                    Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                    Random.Range(spawnAreaMin.y, spawnAreaMax.y)
                );
                attempts++;
                
                if (attempts >= maxAttempts || 
                    (player != null && Vector2.Distance(newPosition, player.position) >= minDistanceFromPlayer))
                    break;
                    
            } while (true);
            
            Debug.Log("Растений нет, коза появилась в случайной зоне");
        }

        // Применяем позицию
        transform.position = newPosition;
        
        // Убеждаемся, что объект активен (на случай, если был деактивирован при побеге за экран)
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, soundPlayRadius);
        
        // Визуализация зоны спавна
        Gizmos.color = new Color(1, 0.5f, 0, 0.3f);
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector3 size = spawnAreaMax - spawnAreaMin;
        Gizmos.DrawCube(center, size);
    }

    public void GoatScared()
    {
        PlaySound(sounds[1], volume: 0.5f);
    }
    
    public void GoatIdle()
    {
        PlaySound(sounds[0], volume: 0.2f, p1: 0.9f, p2: 1.1f);
    }
    private void TryEatNearbyCrop()
    {
        if (CropsManager.Instance == null) return;
        if (CropsManager.Instance.allCrops.Count == 0) return;

        Vector2 goatPos = transform.position;
        Vector2Int? cropToRemove = null;
        float minDistance = float.MaxValue;

        // Ищем ближайшее растение в радиусе поедания
        foreach (var kvp in CropsManager.Instance.allCrops)
        {
            Vector3 cropWorldPos = FarmGrid.Instance.GridToWorldPosition(kvp.Key);
            float distance = Vector2.Distance(goatPos, cropWorldPos);

            if (distance <= eatRadius && distance < minDistance)
            {
                minDistance = distance;
                cropToRemove = kvp.Key;
            }
        }

        if (cropToRemove.HasValue)
        {
            // Удаляем растение
            if (CropsManager.Instance.allCrops.TryGetValue(cropToRemove.Value, out CropBehaviour crop))
            {
                Destroy(crop.gameObject);
                CropsManager.Instance.allCrops.Remove(cropToRemove.Value);
                
                // Опционально: очищаем SoilTile
                GameObject tileObj = FarmGrid.Instance.GetTileAt(cropToRemove.Value);
                if (tileObj != null)
                {
                    SoilTile soil = tileObj.GetComponent<SoilTile>();
                    if (soil != null)
                    {
                        soil.ClearPlanted(); // вам нужно будет добавить такой метод, если ещё нет
                    }
                }
                
                Debug.Log($"🐐 Коза съела растение на позиции {cropToRemove.Value}");
            }
        }
    }
}