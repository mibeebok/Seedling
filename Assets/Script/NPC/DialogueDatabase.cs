using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public static class DialogueDatabase
{
    private static Dictionary<string, List<DialogueLine>> allDialogues = new Dictionary<string, List<DialogueLine>>();

    static DialogueDatabase()
 {
        allDialogues["Intro"] = new List<DialogueLine>
    {
            new DialogueLine
        {
            text = "Привет, я вижу, ты здесь новенький? Давай я тебе обо всём расскажу. Для начала нам нужно познакомиться. Меня зовут Тиоли, а как будет твоё имя?",
            speakerName = "Тиоли",
            speakerFace = null,
            isPlayer = false
        },
        new DialogueLine
        {
            text = "Привет, а меня зовут Гриша.",
            speakerName = "Гриша",
            speakerFace = null,
            isPlayer = true
        },
        new DialogueLine
        {
            text = "Отлично, Гриша! Наша деревня росток очень тихая, но уютная. Правда, экологические условия… ну, с этим у нас тут не очень.",
            speakerName = "Тиоли",
            speakerFace = null,
            isPlayer = false
        },
        new DialogueLine
        {
            text = "Странно, а в старых записях про деревню всё было очень даже позитивно. Кстати, а почему записи о деревне резко прекратились с 2017 года?",
            speakerName = "Гриша",
            speakerFace = null,
            isPlayer = true
        },
        new DialogueLine
        {
            text = "Это… это долгая история. Давай я лучше всё тебе покажу здесь.",
            speakerName = "Тиоли",
            speakerFace = null,
            isPlayer = false
        }

     };

        allDialogues["TerentyDialogueQuest1"] = new List<DialogueLine>
        {
             new DialogueLine
            {
                text = "Привет, чего желаешь?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },

            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Диалог", nextLineIndex = 2},
                    new DialogueChoice { buttonText = "Магазин", nextLineIndex = -1}
                }
            },

            new DialogueLine
            {
                text = "...",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false,
            },
            new DialogueLine
            {
                text = "А... привет? Я новый житель деревни. Моё имя Гриша.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Здравствуй. Моё имя Терентий. Если тебя интересуют саженцы, обращайся в мой магазин. Также ты можешь продавать мне свой урожай.",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["FinnickDialogueQuest1"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет, ты тот самый новый житель? Я - Финник, самый главный в этой деревне. Нам с тобой лучше не ссориться.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Э... ладно, как скажешь. Моё имя - Гриша. Что ты можешь рассказать мне о деревне? Я почти ничего не знаю.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Самое важное, что тебе пока-что нужно знать - не дружи с Ихвильнихтом. Скажу по секрету, но вся проблема в экологии из-за него.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["IhvilnichtDialogueQuest1"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет, я недавно сюда переехал, меня зовут Гриша.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Новый житель? ... Меня зовут Ихвильнихт",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["FinnickNeutral"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет. Гриша, да?",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["IhvilnichtNeutral"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "...",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["TerentyNeutral"] = new List<DialogueLine>
        {
               new DialogueLine
            {
                text = "Привет, чего желаешь?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },

            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Диалог", nextLineIndex = 2},
                    new DialogueChoice { buttonText = "Магазин", nextLineIndex = -1}
                }
            },
            new DialogueLine
            {
                text = "Хочешь что-то купить?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["TioliNeutral"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет, Гриша! Как дела?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["TioliDialogueQuest2"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "О, ты уже вернулся. Довольно быстро. Всех нашёл?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Да, всех. Слушай, а почему все такие странные? Немногословные, отстранённые, словно что-то скрывают.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Финник так вообще сказал мне не дружить с Ихвильнихтом. В чём дело?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Они... ты не слушай их особо. Все немного странные, я с тобой полностью согласна. Но Финник не соврал - не советую сближаться с Ихвильнихтом.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "На самом деле, с Финником тоже не стоит. У них там какая-то своя война. Не знаю, в чём их проблема, но я не хочу в этом разбираться.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Кажется, в вашей деревне действительно много тайн.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Ладно, пора показать тебе, где находится твой дом.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };
        allDialogues["TioliDialogueQuest2.2"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Ну вот мы и пришли! Это твой новый дом... пока выглядит грустновато, но зато есть куда расти!",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Видишь эти пустые участки земли? Только представь, какие овощи ты можешь тут посадить. Перед этим, конечно, не забудь вскопать и полить землю.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "А где мне взять семена? Терентий что-то говорил мне про свой магазин. Это мне к нему обращаться?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Да, ты совершенно прав! У Терентия в магазине обширный выбор: от морковки до малины. Самое главное - не забывай поливать свои посадки.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Раз в день будет достаточно. Если будешь делать всё правильно, твой урожай будет самым вкусным.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["FinnickDialogueQuest3"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "О-о-о, да это же наш новенький! Гриша, да? Ну что, как тебе наша деревня? Вижу, у тебя семена в руке. Огород – это дело хорошее…",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
             new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "А ты зачем у моего дома стоишь?" ,nextLineIndex = 2 },
                    new DialogueChoice { buttonText = "Промолчать, осмотреться", nextLineIndex = 4 }
                }
            },
            new DialogueLine
            {
                text = "Да так, прогуливался! У нас тут, если ты не в курсе, все друг за другом присматривают... забота она такая. Мы ведь тут все как одна большая семья.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Сомнительно… но ладно.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Ну ладно, не буду задерживать тебя, потом ещё как-нибудь к тебе заскочу. Пока, огородник!",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            }
        };
    }

    public static List<DialogueLine> GetDialogue(string key)
    {
        if (allDialogues.ContainsKey(key))
            return allDialogues[key];
        else
        {
            Debug.LogError($"Диалог с ключом '{key}' не найден в базе!");
            return null;
        }
    }

}
