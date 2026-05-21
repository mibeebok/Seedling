using UnityEngine;

public class GoatEscape : MonoBehaviour
{
    [Header("Настройки побега")]
    [SerializeField] private float escapeSpeed = 3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float screenMargin = 0.1f;
    [SerializeField] private float directionJitter = 30f;

    [Header("Обход препятствий")]
    [SerializeField] private float obstacleCheckRadius = 0.5f;   // радиус проверки препятствий
    [SerializeField] private float obstacleCheckDistance = 0.8f; // дистанция вперёд
    [SerializeField] private LayerMask obstacleLayer;            // слой препятствий (или можно оставить тег)
    [SerializeField] private float avoidAngleStep = 45f;        // угол поворота при обходе

    private Transform player;
    private Vector2 escapeDirection;
    private bool isEscaping;
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        player = GameObject.FindWithTag("Player")?.transform;

        if (obstacleLayer == 0)
            obstacleLayer = LayerMask.GetMask("Water");
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

            escapeDirection = AvoidObstacles(escapeDirection);
            MoveEscape();
        }
        else
        {
            isEscaping = false;
        }
    }

    private void CalculateEscapePath()
    {
        Vector2 awayFromPlayer = ((Vector2)transform.position - (Vector2)player.position).normalized;
        float randomAngle = Random.Range(-directionJitter, directionJitter);
        escapeDirection = Quaternion.Euler(0, 0, randomAngle) * awayFromPlayer;
    }

    private Vector2 AvoidObstacles(Vector2 desiredDirection)
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position,
            obstacleCheckRadius,
            desiredDirection,
            obstacleCheckDistance,
            obstacleLayer
        );

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            for (float angle = avoidAngleStep; angle <= 180f; angle += avoidAngleStep)
            {
                // Пробуем повернуть влево
                Vector2 leftDir = Quaternion.Euler(0, 0, angle) * desiredDirection;
                if (!Physics2D.CircleCast(transform.position, obstacleCheckRadius, leftDir, obstacleCheckDistance, obstacleLayer))
                {
                    return leftDir;
                }

                // Пробуем вправо
                Vector2 rightDir = Quaternion.Euler(0, 0, -angle) * desiredDirection;
                if (!Physics2D.CircleCast(transform.position, obstacleCheckRadius, rightDir, obstacleCheckDistance, obstacleLayer))
                {
                    return rightDir;
                }
            }

            return -desiredDirection;
        }

        return desiredDirection;
    }

    private void MoveEscape()
    {
        transform.Translate(escapeDirection * escapeSpeed * Time.deltaTime, Space.World);

        Vector3 viewportPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewportPos.x < -screenMargin ||
            viewportPos.x > 1 + screenMargin ||
            viewportPos.y < -screenMargin ||
            viewportPos.y > 1 + screenMargin)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Показываем луч проверки препятствий
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)escapeDirection * obstacleCheckDistance, obstacleCheckRadius);
    }

    private void OnEnable()
    {
        isEscaping = false;
    }
}