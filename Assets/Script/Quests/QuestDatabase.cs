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

        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
            new NPCDialogueChange { npcName = "Терентий", newDialogueKey = "TerentyNeutral"},
            new NPCDialogueChange { npcName = "Финник", newDialogueKey = "FinnickNeutral"},
            new NPCDialogueChange { npcName = "Ихвильнихт", newDialogueKey = "IhvilnichtNeutral"},
            new NPCDialogueChange { npcName = "Тиоли", newDialogueKey = "TioliNeutral"}
        };

        return quest;
    }

    public static Quest SecondQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 2. Первые шаги";
        quest.description = "Поговорите с Тиоли и найдите свой дом.";
        quest.completionNotes = "«А вот я и дома… Пока что ничего не понимаю, но надеюсь по ходу действий разберусь. Тиоли что-то говорила про огород… звучит как отличная идея. Где там магазин?».";

        quest.tasks = new List<QuestTask>
        {
            new QuestTask { description = "Поговорить с Тиоли", isCompleted = false},
            new QuestTask { description = "Найти свой дом", isCompleted = false}
        };
        quest.rewardMoney = 20;
        quest.rewardCropType = CropType.Carrot;
        quest.rewardSeedCount = 1;

        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
               new NPCDialogueChange { npcName = "Тиоли", newDialogueKey = "TioliNeutral"}
        };

        return quest;
    }

    public static Quest ThirdQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 3. Я-огородник";
        quest.description = "Новый день – новые возможности. Пора познавать глубины огорода. Я уже вижу, как становлюсь самым талантливым в этой сфере! За дело.";
        quest.completionNotes = "«Терентий, вроде, не такой угрюмый, как показался мне вначале. Правда Финник… он ведёт себя как-то странно. Буду за ним наблюдать.».";

        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Посетить лавку Терентия", isCompleted = false },
        new QuestTask { description = "Вскопать и полить грядку", isCompleted = false },
        new QuestTask { description = "Вырастить 3 морковки", isCompleted = false }
    };
        quest.rewardMoney = 10;
        quest.rewardCropType = CropType.Potato;
        quest.rewardSeedCount = 1;

        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
             new NPCDialogueChange { npcName = "Финник", newDialogueKey = "FinnickNeutral"}
        };

        return quest;
    }

    public static Quest FourthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 4. Первый друг";
        quest.description = "Кажется, в этой деревне не так уж и скучно. По крайней мере, выглядит так, словно у меня появился друг – кролик Тиоли. Правда, я чувствую, что она чего-то словно не договаривает. Меня это слегка напрягает, но я уверен, что разберусь со всем.";
        quest.completionNotes = "«Если честно, не ожидал, что эта деревня скрывает в себе такую трагедию. Бедняжка Тиоли… надеюсь, мы с ней сможем добраться до истины.».";

        quest.tasks = new List<QuestTask>
        {
            new QuestTask { description = "Навестить Тиоли", isCompleted = false},
            new QuestTask { description = "Попытаться выяснить, что её тревожит", isCompleted = false}
        };
        quest.rewardMoney = 15;
        quest.rewardCropType = CropType.Rastberry;
        quest.rewardSeedCount = 1;

        quest.npcDialogueChanges = new List<NPCDialogueChange>
       {
           new NPCDialogueChange { npcName = "Тиоли", newDialogueKey = "TioliNeutral"}
       };
        return quest;
    }
    public static Quest FifthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 5. Начало положено";
        quest.description = "Чувствую, диалог выдастся тяжёлым, но мы хотя-бы начнём что-то делать. Не порядок, что такие вещи в деревне остаются под завесой тайны. Хотя бы для успокоения Тиоли мы обязаны попытаться. Надеюсь, Ихвильнихт не будет сильно упираться. А может, его задобрить? Что же может понравиться волку…";
        quest.completionNotes = "«Ну, разговор состоялся. Ихвильнихт, похоже, не враг. Но доказательств у нас пока нет… нужно искать дальше.».";

        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Поговорить с Ихвильнихтом", isCompleted = false }
    };
        quest.rewardMoney = 15;
        quest.rewardCropType = CropType.Beetroot;
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
             new NPCDialogueChange { npcName = "Ихвильнихт", newDialogueKey = "IhvilnichtNeutral"}
        };

        return quest;
    }
}
