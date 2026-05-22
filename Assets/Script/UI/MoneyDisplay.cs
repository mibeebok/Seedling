using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplay : MonoBehaviour
{
    public static MoneyDisplay Instance { get; private set; }

    public Sprite[] digitSprites;   
    public Sprite dotSprite;

    public Image iconImage;         
    public Image[] digitImages;     
    public Image dotImage;

    public int GetMoney() => currentMoney;

    public Vector2 normalDigit2Position;
    public Vector2 normalDotPosition;

    public Vector2 thousandDigit2Position;
    public Vector2 thosandDotPosition;

    private int currentMoney;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        if (digitImages == null || digitImages.Length < 5)
        {
            Debug.LogError("MoneyDisplay: не назначены digitImages (нужно 5)");
            return;
        }
        if (dotImage == null) Debug.LogError("MoneyDisplay: не назначен dotImage");
        if (iconImage == null) Debug.LogError("MoneyDisplay: не назначена iconImage");
    }

    public void SetMoney(int amount)
    {
        currentMoney = amount;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (digitImages.Length < 5 || dotImage == null) return;

        for (int i = 0; i < digitImages.Length; i++)
            digitImages[i].gameObject.SetActive(false);
        dotImage.gameObject.SetActive(false);

        if (currentMoney <= 9999)
        {

            string s = currentMoney.ToString("D4"); 
            for (int i = 0; i < 4; i++)
            {
                int dig = s[i] - '0';
                digitImages[i].sprite = digitSprites[dig];
                digitImages[i].gameObject.SetActive(true);

            }

            if (digitImages.Length > 2) digitImages[2].rectTransform.anchoredPosition = normalDigit2Position;
            dotImage.rectTransform.anchoredPosition = normalDotPosition;
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
            dotImage.rectTransform.anchoredPosition = thosandDotPosition;
        }
    }

    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        UpdateDisplay();
        SaveSystem.SaveGame();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateDisplay();
        SaveSystem.SaveGame();
    }
}