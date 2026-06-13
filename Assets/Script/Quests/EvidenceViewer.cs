using UnityEngine;
using UnityEngine.UI;

public class EvidenceViewer : MonoBehaviour
{
    public Image evidenceImage;
    public Sprite side1;
    public Sprite side2;
    public Button nextButton;
    public Button closeButton;
    public GameObject nextCutsceneStarter;

    private bool isSide1 = true;
    void Start()
    {
        evidenceImage.sprite = side1;
        if (nextButton != null)
            nextButton.onClick.AddListener(() => Flip());
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }
    void Flip()
    {
        isSide1 = !isSide1;
        evidenceImage.sprite = isSide1 ? side1 : side2;
        if (nextButton != null)
            nextButton.gameObject.SetActive(false);
    }

    void Close()
    {
        gameObject.SetActive(false);
        if (nextCutsceneStarter != null)
            nextCutsceneStarter.SetActive(true);
    }
}
