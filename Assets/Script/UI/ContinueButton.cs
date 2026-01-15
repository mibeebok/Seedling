using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    public GameObject buttonContinue;

    void Start()
    {
        if (buttonContinue != null)
        {
            UnityEngine.UI.Button btn = buttonContinue.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.onClick.AddListener(ContinueButton);
            }
            else
            {
                Debug.LogError("Компонент Button не найден на buttonContinue!");
            }
        }
    }
}