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
