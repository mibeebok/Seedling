using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class FarmGrid : Sounds
{
    public static FarmGrid Instance { get; private set; }

    [Header("Grid Settings")]
    public int gridSizeX = 50;
    public int gridSizeY = 50;
    public float cellSize = 2f;
    public GameObject tilePrefab;
    
    [Header("Border Settings")]
    public GameObject borderPrefab; // Префаб границы (горы/камни)
    public int borderThickness = 3; // Толщина границы в ячейках
    public bool generateBorders = true;

    private GameObject[,] grid;
    private GameObject borderContainer; // Контейнер для границ
    public bool isGridGenerated = false;

    private AudioSource musicSource;
    private int lastTrackIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = false;
        
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.3f);
        musicSource.volume = savedVolume;

        if (sounds != null && sounds.Length > 0)
        {
            StartCoroutine(PlayMusicLoop());
        }
        else
        {
            Debug.LogWarning("Массив звуков не инициализирован или пуст!");
        }

        GenerateGrid(() => SaveSystem.LoadGame());
    }

    private IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            int index;
            do
            {
                index = Random.Range(0, sounds.Length);
            } while (index == lastTrackIndex && sounds.Length > 1);

            lastTrackIndex = index;
            AudioClip clip = sounds[index];

            musicSource.clip = clip;
            musicSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }

    public void SetMusicVolume(float volume)
    {
        AudioSource src = GetComponent<AudioSource>();
        if (src != null)
        {
            src.volume = volume;
        }
    }
        
    private void GenerateGrid(System.Action onComplete = null)
    {
        if (tilePrefab == null)
        {
            Debug.LogError("Tile Prefab не назначен!");
            return;
        }

        grid = new GameObject[gridSizeX, gridSizeY];
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );

        StartCoroutine(GenerateGridCoroutine(gridCenter, onComplete));
    }

    public Vector3 GridToWorldPosition(Vector2Int gridPosition)
    {
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );
        
        float worldX = gridPosition.x * cellSize - gridCenter.x;
        float worldY = gridPosition.y * cellSize - gridCenter.y;
        
        return new Vector3(worldX, worldY, 0);
    }

    private IEnumerator GenerateGridCoroutine(Vector3 center, System.Action onComplete)
    {
        // Создаем контейнер для границ
        if (generateBorders && borderPrefab != null)
        {
            borderContainer = new GameObject("Borders");
            borderContainer.transform.SetParent(transform);
            GenerateBorders(center);
        }

        // Генерируем основную сетку
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GameObject tile = Instantiate(tilePrefab, transform);
                tile.transform.position = new Vector3(
                    x * cellSize - center.x,
                    y * cellSize - center.y,
                    0
                );
                tile.name = $"Tile_{x}_{y}";
                grid[x, y] = tile;

                if ((x * gridSizeY + y) % 100 == 0)
                    yield return null;
            }
        }

        isGridGenerated = true;
        onComplete?.Invoke();
    }

    private void GenerateBorders(Vector3 center)
    {
        // Вычисляем границы мира
        float worldMinX = -center.x;
        float worldMaxX = (gridSizeX - 1) * cellSize - center.x;
        float worldMinY = -center.y;
        float worldMaxY = (gridSizeY - 1) * cellSize - center.y;

        // Расширяем границы на толщину бордера
        int startX = -borderThickness;
        int endX = gridSizeX + borderThickness;
        int startY = -borderThickness;
        int endY = gridSizeY + borderThickness;

        for (int x = startX; x < endX; x++)
        {
            for (int y = startY; y < endY; y++)
            {
                // Проверяем, находится ли позиция ЗА пределами игровой сетки
                bool isOutsideGrid = x < 0 || x >= gridSizeX || y < 0 || y >= gridSizeY;
                
                if (isOutsideGrid)
                {
                    GameObject borderTile = Instantiate(borderPrefab, borderContainer.transform);
                    borderTile.transform.position = new Vector3(
                        x * cellSize - center.x,
                        y * cellSize - center.y,
                        0
                    );
                    borderTile.name = $"Border_{x}_{y}";
                    
                    // Добавляем коллайдер если его нет
                    if (borderTile.GetComponent<Collider2D>() == null)
                    {
                        BoxCollider2D collider = borderTile.AddComponent<BoxCollider2D>();
                        collider.size = new Vector2(cellSize, cellSize);
                    }
                }
            }
        }
    }

    // Проверка, находится ли позиция за пределами игрового мира
    public bool IsPositionOutsideWorld(Vector3 worldPosition)
    {
        Vector2Int gridPos = WorldToGridPosition(worldPosition);
        return gridPos.x < 0 || gridPos.x >= gridSizeX || 
               gridPos.y < 0 || gridPos.y >= gridSizeY;
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );
        
        int gridX = Mathf.FloorToInt((worldPosition.x + gridCenter.x) / cellSize);
        int gridY = Mathf.FloorToInt((worldPosition.y + gridCenter.y) / cellSize);
                
        return new Vector2Int(gridX, gridY);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        
        // Рисуем границы игрового мира
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );
        
        Vector3 worldMin = new Vector3(-gridCenter.x, -gridCenter.y, 0);
        Vector3 worldMax = new Vector3(
            (gridSizeX - 1) * cellSize - gridCenter.x,
            (gridSizeY - 1) * cellSize - gridCenter.y,
            0
        );
        
        // Красная рамка - граница игрового мира
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            (worldMin + worldMax) * 0.5f,
            new Vector3(worldMax.x - worldMin.x, worldMax.y - worldMin.y, 0.1f)
        );
        
        // Синяя рамка - внешняя граница с бордерами
        if (generateBorders)
        {
            Gizmos.color = Color.blue;
            Vector3 borderMin = worldMin - new Vector3(borderThickness * cellSize, borderThickness * cellSize, 0);
            Vector3 borderMax = worldMax + new Vector3(borderThickness * cellSize, borderThickness * cellSize, 0);
            Gizmos.DrawWireCube(
                (borderMin + borderMax) * 0.5f,
                new Vector3(borderMax.x - borderMin.x, borderMax.y - borderMin.y, 0.1f)
            );
        }
        
        // Рисуем сетку если нужно
        if (grid != null)
        {
            Gizmos.color = Color.cyan;
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPos = GridToWorldPosition(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(worldPos, new Vector3(cellSize, cellSize, 0.1f));
                    
                    #if UNITY_EDITOR
                    UnityEditor.Handles.Label(worldPos + Vector3.up * 0.2f, $"({x},{y})");
                    #endif
                }
            }
        }
    }

    public GameObject GetTileAt(Vector2Int gridPos)
    {
        if (gridPos.x >= 0 && gridPos.x < gridSizeX && 
            gridPos.y >= 0 && gridPos.y < gridSizeY)
        {
            return grid[gridPos.x, gridPos.y];
        }
        return null;
    }

    public Vector2 GetGridBounds()
    {
        float halfWidth = (gridSizeX * cellSize) / 2f;
        float halfHeight = (gridSizeY * cellSize) / 2f;
        return new Vector2(halfWidth, halfHeight);
    }
}