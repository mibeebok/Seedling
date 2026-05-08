using UnityEngine;

public class BookHotkey : MonoBehaviour
{

    public BookUI bookUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (bookUI != null)
                bookUI.ToggleWindow();
        }
    }
}
