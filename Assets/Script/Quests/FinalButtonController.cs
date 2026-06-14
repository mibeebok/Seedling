using UnityEngine;
using UnityEngine.UI;

public class FinalButtonController : MonoBehaviour
{
    public Button finalButton;
    public GameObject errorPopup;     
    public GameObject cutsceneStarter14_2; 
    public EcologyController ecologyController;

    private bool canStartFinal = false;
    void Start()
    {
        if (finalButton != null)
            finalButton.onClick.AddListener(OnFinalButtonClick);
        if (errorPopup != null)
            errorPopup.SetActive(false);
    }

    public void EnableFinalButton()
    {
        canStartFinal = true;
        if (finalButton != null) finalButton.gameObject.SetActive(true);
    }

    private void OnFinalButtonClick()
    {
        if (!canStartFinal) return;

        float currentEcoPercent = (ecologyController.CurrentEco / ecologyController.maxEco) * 100f;

        if (currentEcoPercent < 70f)
        {
            if (errorPopup != null) errorPopup.SetActive(true);
            return;
        }

        if (cutsceneStarter14_2 != null)
            cutsceneStarter14_2.SetActive(true);
    }

    public void CloseErrorPopup()
    {
        if (errorPopup != null) errorPopup.SetActive(false);
    }

    public void DisableFinalButton()
    {
        canStartFinal = false;
        if (finalButton != null) finalButton.gameObject.SetActive(false);
    }
}
