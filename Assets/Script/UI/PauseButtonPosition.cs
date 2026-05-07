using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonPosition : MonoBehaviour 
{
    [Header("Настройки")]
    public Camera uiCamera;
    public Vector2 screenPosition = new Vector2(0.95f, 0.9f);
    public float pixelOffsetX = -50f;
    public float pixelOffsetY = -50f;
    public GameObject menu;

    private bool isMenuOpen = false;

    void Start()
    {
        if(menu != null){
            menu.SetActive(false);
        }
    }

    void Update()
    {
        UpdatePosition();
        
        if (!isMenuOpen && Input.GetMouseButtonDown(0))
        {
            CheckClick();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void UpdatePosition()
    {
        if (uiCamera == null) uiCamera = Camera.main;
        
        Vector3 viewportPos = new Vector3(screenPosition.x, screenPosition.y, 10);
        Vector3 worldPos = uiCamera.ViewportToWorldPoint(viewportPos);
        
        // Конвертируем пиксельные отступы в мировые единицы
        float ppu = 100f; // Пикселей на единицу
        Vector3 offset = new Vector3(pixelOffsetX/ppu, pixelOffsetY/ppu, 0);
        
        transform.position = worldPos + offset;
    }

    void CheckClick()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = GetComponent<Collider2D>();

        if (collider != null && collider.OverlapPoint(mousePos))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu(){
        if (menu != null) {
            isMenuOpen = !menu.activeSelf;
            bool menuActive = !menu.activeSelf; 
            menu.SetActive(menuActive);

            Time.timeScale = menuActive ? 0f : 1f;

        }
        else{
            Debug.LogWarning("Забыла назначить объект меню!!");
        }
    }

    public void ResumeGame()
    {
        //  Через ToggleMenu (если меню активно)
        if (menu != null && menu.activeSelf)
        {
            ToggleMenu();
            return;
        }

        Time.timeScale = 1f;
        isMenuOpen = false;
    }
    public bool IsMenuOpen(){return isMenuOpen;}
   
}