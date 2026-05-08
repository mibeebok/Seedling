using UnityEngine;
using UnityEngine.UI;

public class BookButtonPosition : MonoBehaviour
{
    public Camera uiCamera;
    public Vector2 screenPosition = new Vector2(0.95f, 0.75f); 
    public float pixelOffsetX = -50f;
    public float pixelOffsetY = -50f;

    private BookUI bookUI;

    void Start()
    {
        bookUI = FindObjectOfType<BookUI>();
        if (bookUI == null)
            Debug.LogError("BookUI не найден на сцене!");
    }

    private void Update()
    {
        UpdatePosition();

        if (Input.GetMouseButtonDown(0))
            CheckClick();
    }

    void UpdatePosition()
    {
        if (uiCamera == null) uiCamera = Camera.main;

        Vector3 viewportPos = new Vector3(screenPosition.x, screenPosition.y, 10);
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);

        float ppu = 100f; // пикселей на единицу
        Vector3 offset = new Vector3(pixelOffsetX / ppu, pixelOffsetY / ppu, 0);

        transform.position = worldPos + offset;
    }

    void CheckClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null && collider.OverlapPoint(mousePos))
        {
            if (bookUI != null)
                bookUI.ToggleWindow();
            Debug.Log(" нига кнопки нажата!");
        }
    }
}
