using UnityEngine;

public class BookHotkey : MonoBehaviour
{

    public BookUI bookUI;

    void Update()
    {
        if (ShopUI.IsShopOpen) return;

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (bookUI != null)
                bookUI.ToggleWindow();
        }
    }
}
