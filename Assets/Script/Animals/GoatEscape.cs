using UnityEngine;

public class GoatEscape : MonoBehaviour
{
    [Header("Настройки побега")]
    [SerializeField] private float escapeSpeed = 3f;       // скорость побега
    [SerializeField] private float detectionRange = 5f;    // радиус реакции на игрока
    [SerializeField] private float screenMargin = 0.1f;    // запас от границ экрана
    [SerializeField] private float directionJitter = 30f;  // разброс направления в градусах

    private Transform player;
    private Vector2 escapeDirection;
    private bool isEscaping;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            if (!isEscaping)
            {
                CalculateEscapePath();
                isEscaping = true;
            }

            MoveEscape();
        }
        else
        {
            isEscaping = false;
        }
    }

    /// <summary>
    /// Рассчитывает направление убегания в сторону, противоположную игроку,
    /// с небольшим случайным отклонением (чтобы движение выглядело естественно)
    /// </summary>
    private void CalculateEscapePath()
    {
        // Направление от игрока к козе
        Vector2 awayFromPlayer = (transform.position - player.position).normalized;

        // Добавляем "рандомный угол" к направлению, чтобы она не бежала идеально по прямой
        float randomAngle = Random.Range(-directionJitter, directionJitter);
        escapeDirection = Quaternion.Euler(0, 0, randomAngle) * awayFromPlayer;
    }

    /// <summary>
    /// Перемещает козу в рассчитанном направлении и деактивирует при выходе за границы
    /// </summary>
    private void MoveEscape()
    {
        transform.Translate(escapeDirection * escapeSpeed * Time.deltaTime, Space.World);

        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -screenMargin ||
            viewportPos.x > 1 + screenMargin ||
            viewportPos.y < -screenMargin ||
            viewportPos.y > 1 + screenMargin)
        {
            // Коза выбежала за экран — можно выключить или вернуть
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
