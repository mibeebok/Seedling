using UnityEngine;
using UnityEngine.UI;

public class ShopMoneyDisplay : MonoBehaviour
{
    public Sprite[] digitSprites;
    public Sprite dotSprite;

    public Image currencyIcon;
    public Image[] digitImages;
    public Image dotImage;

    public Vector2 normalDigit2Position;
    public Vector2 normalDotPosition;
    public Vector2 thousandDigit2Position;
    public Vector2 thousandDotPosition;

    private int currentMoney;
    void Start()
    {
        if (digitImages == null || digitImages.Length < 5)
            Debug.LogError("ShopMoneyDisplay: не назначены digitImages");
        if (digitSprites == null || digitSprites.Length < 10)
            Debug.LogError("ShopMoneyDisplay: не назначены digitSprites");
        
        if (dotSprite == null) Debug.LogWarning("ShopMoneyDisplay: dotSprite не назначен");
        if (dotImage != null && dotSprite != null) dotImage.sprite = dotSprite;

    }

    public void UpdateMoneyDisplay(int money)
    {
        currentMoney = money;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (digitImages.Length < 5 || digitImages == null) return;

        for (int i = 0; i < digitImages.Length; i++)
            digitImages[i].gameObject.SetActive(false);

        if(dotImage != null) dotImage.gameObject.SetActive(false);

        if (currentMoney <= 9999)
        {
            string s = currentMoney.ToString("D4");
            for (int i = 0; i < 4; i++)
            {
                int dig = s[i] - '0';
                if (digitSprites != null && dig < digitSprites.Length && digitSprites[dig] != null)
                    digitImages[i].sprite = digitSprites[dig];
                else
                    Debug.LogError($"digitSprites[{dig}] is missing");
                digitImages[i].gameObject.SetActive(true);
            }

            if (digitImages.Length > 2) digitImages[2].rectTransform.anchoredPosition = normalDigit2Position;
            if (dotImage != null)dotImage.rectTransform.anchoredPosition = normalDotPosition;
        }
        else
        {
            int thousands = currentMoney / 1000;
            int tenth = (currentMoney % 1000) / 100;
            int tens = thousands / 10;
            int units = thousands % 10;

            digitImages[0].sprite = digitSprites[tens];
            digitImages[1].sprite = digitSprites[units];
            digitImages[2].sprite = digitSprites[tenth];
            digitImages[0].gameObject.SetActive(true);
            digitImages[1].gameObject.SetActive(true);
            digitImages[2].gameObject.SetActive(true);
            dotImage.gameObject.SetActive(true);

            if (digitImages.Length > 2) digitImages[2].rectTransform.anchoredPosition = thousandDigit2Position;
            dotImage.rectTransform.anchoredPosition = thousandDotPosition;
        }
    }
}
