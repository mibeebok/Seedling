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

    public void Show(Quest quest)
    {
        questInfoText.text = $"Поздравляю, вы успешно завершили задание \"{quest.questName}\".\nВот ваша награда:";

        rewardMoneyText.text = $"+{quest.rewardMoney} листеньев";

        if (quest.rewardCropType != CropType.None && quest.rewardSeedCount > 0)
        {
            Item seedItem = DataBase.Instance.GetItemByCropType(quest.rewardCropType, true);

            if (seedItem != null)
            {
                rewardItemIcon.sprite = seedItem.img;
                rewardItemNameText.text = $"{seedItem.name} x {quest.rewardSeedCount}";
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
}
