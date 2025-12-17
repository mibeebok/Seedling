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

    private GameObject[,] grid;
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
        // Создаем или получаем AudioSource
        musicSource = GetComponent<AudioSource>();
        if (musicSource == null)
            musicSource = gameObject.AddComponent<AudioSource>();

        // Настройка
        musicSource.loop = false; // мы сами управляем циклом
        musicSource.volume = 0.3f;

        // Запуск фоновой музыки
        if (sounds != null && sounds.Length > 0)
        {
            StartCoroutine(PlayMusicLoop());
        }
        else
        {
            Debug.LogWarning("Массив звуков не инициализирован или пуст!");
        }

        // Генерация сетки
        GenerateGrid(() => SaveSystem.LoadGame());
    }
    private IEnumerator PlayMusicLoop()
    {
        while (true)
        {
            // Выбираем случайный трек, чтобы не повторялся подряд
            int index;
            do
            {
                index = Random.Range(0, sounds.Length);
            } while (index == lastTrackIndex && sounds.Length > 1);

            lastTrackIndex = index;
            AudioClip clip = sounds[index];

            // Проигрываем трек
            musicSource.clip = clip;
            musicSource.Play();

            // Ждем окончания
            yield return new WaitForSeconds(clip.length);
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
        // Вычислите центр сетки как в GenerateGridCoroutine
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );
        
        // Используйте ТОЧНУЮ ЖЕ формулу что и при генерации тайлов:
        float worldX = gridPosition.x * cellSize - gridCenter.x;
        float worldY = gridPosition.y * cellSize - gridCenter.y;
        
        Debug.Log($"GridToWorld: grid({gridPosition.x},{gridPosition.y}) → world({worldX},{worldY})");
        
        return new Vector3(worldX, worldY, 0);
    }

    private IEnumerator GenerateGridCoroutine(Vector3 center, System.Action onComplete)
    {
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

                // пропускаем кадр каждые 100 тайлов
                if ((x * gridSizeY + y) % 100 == 0)
                    yield return null;
            }
        }

        isGridGenerated = true;
        onComplete?.Invoke();
    }

    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        Vector3 gridCenter = new Vector3(
            (gridSizeX - 1) * cellSize * 0.5f,
            (gridSizeY - 1) * cellSize * 0.5f,
            0
        );
        
        // Обратная формула:
        int gridX = Mathf.FloorToInt((worldPosition.x + gridCenter.x) / cellSize);
        int gridY = Mathf.FloorToInt((worldPosition.y + gridCenter.y) / cellSize);
        
        Debug.Log($"WorldToGrid: world({worldPosition.x},{worldPosition.y}) → grid({gridX},{gridY})");
        
        return new Vector2Int(gridX, gridY);
    }
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || grid == null) return;
        
        // Нарисовать сетку для отладки
        Gizmos.color = Color.cyan;
        
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPos = GridToWorldPosition(new Vector2Int(x, y));
                Gizmos.DrawWireCube(worldPos, new Vector3(cellSize, cellSize, 0.1f));
                
                // Подпись координат
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(worldPos + Vector3.up * 0.2f, $"({x},{y})");
                #endif
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
        //границы карты
        float halfWidth = (gridSizeX * cellSize) / 2f;
        float halfHeight = (gridSizeY * cellSize) / 2f;

        return new Vector2(halfWidth, halfHeight);
    }
}