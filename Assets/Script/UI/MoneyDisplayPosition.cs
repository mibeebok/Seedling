using UnityEngine;

public class MoneyDisplayPosition : MonoBehaviour
{

    public Camera uiCamera;
    public Vector2 screenPosition = new Vector2(0.83f, 0.87f);
    public float pixelOffSetX = 0f;
    public float pixelOffSetY = 0f;

    void Start()
    {
        if (uiCamera == null) uiCamera = Camera.main;
        if (uiCamera == null)
            Debug.LogError("MoneyDisplayPosition: камера не назначена и не найдена!");
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (uiCamera == null) return;

        Vector3 viewportPos = new Vector3(screenPosition.x, screenPosition.y, 10);
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);

        float ppu = 100f;

        Vector3 offset = new Vector3(pixelOffSetX / ppu, pixelOffSetY / ppu,0);
        transform.position = worldPos + offset;
    }
}
