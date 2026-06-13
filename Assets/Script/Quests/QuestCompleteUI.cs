using UnityEngine;
using UnityEngine.UI;

public class QuestCompleteUI : MonoBehaviour
{
    public Text questInfoText;
    public Text rewardMoneyText;
    public Image rewardItemIcon;
    public Text rewardItemNameText;
    public Button okButton;
    public GameObject completeWindow;

    void Start()
    {
        if (completeWindow != null)
            completeWindow.SetActive(false);
        if (okButton != null)
            okButton.onClick.AddListener(() => completeWindow.SetActive(false));
    }

    public void Show(Quest quest, bool hasNextQuest = false)
    {
        questInfoText.text = $"Поздравляю, вы успешно завершили задание \"{quest.questName}\".";
        if (hasNextQuest)
            questInfoText.text += "\n\nУ вас доступно следующее задание!";
        questInfoText.text += "\n\nВот ваша награда:";

        rewardMoneyText.text = $"+{quest.rewardMoney} листеньев";

        if (quest.rewardCropType != CropType.None && quest.rewardSeedCount > 0)
        {
            Item seedItem = DataBase.Instance.GetItemByCropType(quest.rewardCropType, true);

            if (seedItem != null)
            {
                rewardItemIcon.sprite = seedItem.img;
                rewardItemNameText.text = $"{GetRussianSeedName(quest.rewardCropType)} x {quest.rewardSeedCount}";
                rewardItemIcon.gameObject.SetActive(true);
                rewardItemNameText.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError($"Не найдены seed item для {quest.rewardCropType}");
                rewardItemIcon.gameObject.SetActive(false);
                rewardItemNameText.gameObject.SetActive(false);
            }
        }
        else
        {
            rewardItemIcon.gameObject.SetActive(false);
            rewardItemNameText.gameObject.SetActive(false);
        }
        if (completeWindow != null)
            completeWindow.SetActive(true);
    }

    private string GetRussianSeedName(CropType cropType)
    {
        switch (cropType)
        {
            case CropType.Potato: return "Картофель";
            case CropType.Carrot: return "Морковь";
            case CropType.Beetroot: return "Свекла";
            case CropType.Rastberry: return "Малина";
            default: return cropType.ToString();
        }
    }
}
