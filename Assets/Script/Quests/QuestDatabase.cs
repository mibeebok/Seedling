using UnityEngine;
using System.Collections.Generic;

public static class QuestDatabase
{
    public static Quest IntroQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 1. Знакомство с деревней";
        quest.description = "После получения иконки с квестом у Гриши задача найти остальных трёх жителей и познакомиться с ними. Порядок знакомства с персонажами не важен.";
        quest.completionNotes = "«Ну… жители, конечно, странные, но… здесь хотя-бы спокойно. Думаю, если быть со всеми добрым, у меня не будет проблем. Пока что самая интересная здесь Тиоли. Вот её я пока и буду держаться.».";
        quest.tasks = new List<QuestTask>
        {
            new QuestTask {  description = "Поговорить с Терентием", isCompleted = false},
            new QuestTask { description = "Поговорить с Финником", isCompleted = false},
            new QuestTask { description = "Поговорить с Ихвильнихтом", isCompleted = false}
        };
        quest.rewardMoney = 20;
        quest.rewardCropType = CropType.Potato;
        quest.rewardSeedCount = 1;
        return quest;
    }
}
