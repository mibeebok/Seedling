using UnityEngine;

public class QuestChoiceHandler : MonoBehaviour
{
    private void Awake()
    {
        DialogueManager dm = FindFirstObjectByType<DialogueManager>();
        if (dm != null)
            dm.OnChoiceSelectedEvent += HandleChoice;
        else
            Debug.LogError("DialogueManager not found for QuestChoiceHandler");
    }
    private void HandleChoice(string dialogueKey, int currentLineIndex, int chosenNextIndex)
    {
        switch (dialogueKey)
        {
            case "DialogueQuest10":
                HandleQuest10(dialogueKey, currentLineIndex, chosenNextIndex);
                break;
            case "DialogueQuest11":
                HandleQuest11(dialogueKey, currentLineIndex, chosenNextIndex);
                break;
            case "DialogueQuest12":
                HandleQuest12(dialogueKey, currentLineIndex, chosenNextIndex);
                break;
                // здесь дальше добавлять диалоги
        }
    }
    private void HandleQuest10(string dialogueKey, int currentLineIndex, int chosenNextIndex)
    {
        // первый выбор (строка 1)
        if (currentLineIndex == 1)
        {
            if (chosenNextIndex == 2)      // "мы нашли волчью шерсть... (плохо)"
                QuestManager.Instance.AddTeamFox(1);
            else if (chosenNextIndex == 10) // "кто-то подстроил... (хорошо)"
                QuestManager.Instance.AddTeamWolf(1);
            else if (chosenNextIndex == 13) // молча показать (нейтрально)
                QuestManager.Instance.AddTeamWolf(1); // даёт немного хороших
        }
        // второй выбор (после строки 2 – только если пошли по плохой ветке)
        else if (currentLineIndex == 3)
        {
            if (chosenNextIndex == 4)      // "лапы можно и помыть..." (плохо)
                QuestManager.Instance.AddTeamFox(1);
            else if (chosenNextIndex == 6) // "но как тогда можно это..." (неплохо)
                QuestManager.Instance.AddTeamWolf(1);
        }
    }
    private void HandleQuest11(string dialogueKey, int currentLineIndex, int chosenNextIndex)
    {
        // первый выбор (строка 3)
        if (currentLineIndex == 3)
        {
            if (chosenNextIndex == 4)      // "мы знаем, что ты подбросил шерсть..." (хорошо)
                QuestManager.Instance.AddTeamWolf(1);
            else if (chosenNextIndex == 12) // "мне нужна твоя правда..." (плохо)
                QuestManager.Instance.AddTeamFox(1);
            else if (chosenNextIndex == 18) // молча положил шерсть (неплохо)
                QuestManager.Instance.AddTeamWolf(1); // даёт хорошие очки
        }

    }
    private void HandleQuest12(string dialogueKey, int currentLineIndex, int chosenNextIndex)
    {
        // выбор (стркоа 8)
        if (currentLineIndex == 8)
        {
            if (chosenNextIndex == 9)      // первый вариант "Слушай… а это идея."
            {
                QuestManager.Instance.quest12Choice = 1;
                QuestManager.Instance.AddTeamWolf(1);
            }
            else if (chosenNextIndex == 12) // второй вариант "виновен Ихвильнихт"
            {
                QuestManager.Instance.quest12Choice = 2;
                QuestManager.Instance.AddTeamFox(1);
            }
        }
    }
}
