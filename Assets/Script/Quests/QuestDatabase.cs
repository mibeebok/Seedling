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
    public static Quest SixthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 6. П-Предприниматель";
        quest.description = "Итак, первые шаги сделаны. Урожай – вырос, а значит, пора задуматься о бизнесе. Что там говорил Терентий? Я могу продать ему часть выращенного? Отличная мысль. Ну что, в путь?";
        quest.completionNotes = "«Ну вот и всё, моя первая прибыль с урожая. Малыми шагами я точно достигну успеха. Нужно продолжать в том же духе. Так, малина… нужно пойти и посадить малину.».";

        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Посетить лавку Терентия", isCompleted = false },
        new QuestTask { description = "Продать часть урожая", isCompleted = false },
        new QuestTask { description = "Купить семена малины", isCompleted = false }
    };
        quest.rewardMoney = 10;
        quest.rewardCropType = CropType.Potato; 
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>(); 

        return quest;
    }
    public static Quest SeventhQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 7. Я доберусь до правды";
        quest.description = "Чтобы начать расследование, в любом фильме или сериале начинают с осмотра места преступления. А значит, мы должны сходить и проверить место, где горел дом. Да, прошло много лет, но может быть нам повезёт, и какие-то улики там останутся.";
        quest.completionNotes = "«Ух, тяжёлый выдался денёк… но мы теперь на шаг ближе к истине. Я очень рад, что прогресс сдвинулся с мёртвой точки. Не могу поверить, что столько лет никому не было дела до этой трагедии. В голове не укладывается…».";

        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Найти место, где горел дом", isCompleted = false },
        new QuestTask { description = "Осмотреться на предмет улик", isCompleted = false }
    };
        quest.rewardMoney = 20;
        quest.rewardCropType = CropType.Rastberry;
        quest.rewardSeedCount = 1;

        quest.npcDialogueChanges = new List<NPCDialogueChange>();
        return quest;
    }
    public static Quest EighthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 8. Тайны, тайны, тайны...";
        quest.description = "Сегодня ровно неделя, как я живу в деревне Росток. Я всё ещё пока мало понимаю, что здесь происходит, но я намерен докопаться до правды. Уверен, в этом мне немного поможет Терентий. Ну что, в путь?";
        quest.completionNotes = "«Сегодня я узнал… много нового. Информации так много, и она так шокирует, что я теряюсь в своих мыслях. Но я понял то, что слепо доверять пока никому не стоит. Я должен наблюдать за Финником, чтобы понять, кто прав, а кто нет.».";
        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Попытаться узнать у Терентия о Финнике и Ихвильнихте", isCompleted = false }
    };
        quest.rewardMoney = 10;          
        quest.rewardCropType = CropType.Potato;  
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
             new NPCDialogueChange { npcName = "Терентий", newDialogueKey = "TerentyNeutral"}
        };

        return quest;
    }
    public static Quest NinthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 9. Завелась крыса?";
        quest.description = "Сегодня я проснулся со странным ощущением, словно что-то… конкретно не то. Что бы это могло значить?";
        quest.completionNotes = "«Я был уверен, что почти разгадал тайны этой деревни, но теперь… теперь я ни в чём не уверен. Кому мне верить? Кому вообще можно верить?».";
        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Разобраться, в чём дело", isCompleted = false },
        new QuestTask { description = "Поговорить с Тиоли", isCompleted = false }
    };
        quest.rewardMoney = 15;
        quest.rewardCropType = CropType.Potato;
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>(); 

        return quest;
    }
    public static Quest TenthQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 10. Навестим угрюмого";
        quest.description = "У меня чувство, что кто-то водит нас за нос. Кто-то явно хочет перевернуть всю ситуацию в свою пользу. Финник или Ихвильнихт? Кто прав, а кто нет? Надеюсь, я смогу сделать правильный выбор.";
        quest.completionNotes = "«История всё запутаннее и запутаннее. Кажется, что вот-вот, и мы уже достигнем истины. Самое главное не ошибиться и принять правильное решение. Вероятно, будущее деревни Росток сейчас легло полностью на мои плечи. Мда, не желал я такой ответственности, когда переезжал. Спокойная жизнь, природа, чистый воздух… Да-да, и где оно всё сейчас?».";
        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Поговорить с Ихвильнихтом", isCompleted = false }
    };
        quest.rewardMoney = 20;
        quest.rewardCropType = CropType.Rastberry;
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>(); 
        return quest;
    }
    public static Quest EleventhQuest()
    {
        Quest quest = new Quest();
        quest.questName = "Квест 11. Что скрывает лис?";
        quest.description = "Разговорить лиса и получить от него что-то правдивое – задача практически невозможная. Но я постараюсь изо всех сил. Кто-то ведь должен это закончить, верно?";
        quest.completionNotes = "«В голове столько разных мыслей, но полной картины я пока не могу слепить. Нужен кто-то более мудрый и… нейтральный. Терентий идеально подходит под это описание.».";
        quest.tasks = new List<QuestTask>
    {
        new QuestTask { description = "Поговорить с Тиоли", isCompleted = false },
        new QuestTask { description = "Попытаться вытянуть правду из Финника", isCompleted = false }
    };
        quest.rewardMoney = 10;
        quest.rewardCropType = CropType.Beetroot;
        quest.rewardSeedCount = 1;
        quest.npcDialogueChanges = new List<NPCDialogueChange>
        {
            new NPCDialogueChange { npcName = "Тиоли", newDialogueKey = "TioliNeutral"}
        };

        return quest;
    }

}
