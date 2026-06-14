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

        allDialogues["TioliDialogueQuest4"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет, Тиоли. Как поживаешь?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Привет, Гриша! Здорово, что ты пришёл. У меня всё хорошо, а как у тебя продвижения? Уже привык, или пока ещё нет?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "В целом неплохо. Постепенно узнаю что-то новое. И кстати говоря, об этом. Не хочу лезть не в своё дело, но мне показалось, что тебя что-то беспокоит. Ты можешь поделиться, возможно, тебе станет легче.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "...(неуверенно осмотрелась по сторонам, размышляя, что ей делать).",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Ну... это довольно непонятная ситуация. Насчёт... 2017 года. Ты ведь не знаешь, что там произошло, правда? (голос Тиоли слегка дрогнул, словно она пыталась сдержать внутри себя бурю эмоций).",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
             new DialogueLine
            {
                text = "Нет, я не знаю. Те события как-то связаны с тобой?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Да, это... напрямую связано со мной. Точнее, с моими родными. Они... кто-то из жителей поджёг дом, где была моя семья. Я в это время ночевала в гостях у подруги, как вдруг мы услышали шум, а далее очень сильно запахло дымом...",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
              new DialogueLine
            {
                text = "Мы сразу выбежали на улицу, и в ужасе заметили, что дым... исходит со стороны моего дома. Когда мы прибежали, там уже была толпа из жителей. А дом уже догорал. С моей спящей внутри семьёй.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
             new DialogueLine
            {
                text = "Боже мой... кому вообще в голову пришла такая жестокость?... Вы их наказали?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "А вот здесь уже проблема. Никто точно не знает всю историю. На месте происшествия первым оказался Ихвильнихт, и как-то жители решили между собой, что это было его рук дело.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "А на следующее утро Финник подтвердил, что видел, как волк поджигал дом. Ихвильнихта изгнали с деревни.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
             new DialogueLine
            {
                text = "То есть как, просто взяли и изгнали? Без доказательств? У вас в деревне разве нет чего-то, типа... полиции? Суда? Такие вопросы нельзя оставлять нерешёнными.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
             new DialogueLine
            {
                text = "Нет, у нас немного по-другому всё работает. У нас есть хранитель леса, точнее, был. Терентий временно в отставке, а на его место метит Финник.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Кстати говоря, о Финнике, мне кажется, он совсем не тот, за кого себя выдаёт.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Почему такие мысли?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Финник раньше был лучшим другом Ихвильнихта. Они вместе росли, можно сказать. Не разлей вода были.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "А потом Финник связался с компанией каких-то лисов, и я полагаю, на него оказали плохое влияние. Именно тогда между этими двумя начались конфликты.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "То есть, ты хочешь сказать… Финник мог подставить Ихвильнихта? Но зачем ему это делать?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "А вот здесь мои догадки оканчиваются. Я в тупике. Уже столько лет пытаюсь разгадать, что на самом деле произошло.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Я пыталась говорить с Ихвильнихтом, но он молчит, как партизан. Даже не знаю, как вытянуть из него хотя-бы слово.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "А что, если мне попробовать? В прошлый раз, вроде-бы, мы с ним смогли поговорить.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Я уверен, что к каждому можно найти свой подход. К тому же, если его совесть чиста, это в его же интересах решить конфликт.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Думаю, ты прав. Мы хотя-бы попытаемся, верно? Ладно, тогда в путь.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["IhvilnichtDialogueQuest5"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Привет, дружище! Мы пришли… в общем, у нас есть к тебе пару вопросов. Сможешь ответить нам?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Дружище?.. (волк нахмурился, слегка отходя назад, соблюдая дистанцию). Вопросы?.. Что вам от меня нужно?",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "(слегка отступая назад, чтобы дать волку пространство). Извини, мы не пришли ругаться с тобой. Мы просто хотим… узнать правду. Вот и всё.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Правду? Ты думаешь, после стольких лет это поменяет хоть что-то? (Ихвильнихт огрызается, обнажив клыки).",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Слушай, я понимаю, что тебе наверняка больно, если ты невиновен. Так может быть, самое время раскрыть правду? Мы пришли не с целью вредить тебе.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Я… действительно не поджигал дом. Я просто прибежал раньше всех. Пытался что-то предпринять, но было слишком поздно, внутрь было даже не проникнуть. Вот только мне никто не поверил (в голосе звучала боль).",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Знаешь… ты звучишь убедительно. Но… к сожалению, нам нужны доказательства.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "(горько усмехаясь), А, ну да, как иначе. На словах уже никто и ни во что не верит…",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Слушай, я понимаю твою боль, но мы ведь должны как-то доказать всем, что ты невиновен.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Я очень хочу тебе поверить, но Гриша прав, доказательства… они могут играть ключевую роль.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Ну тогда удачи вам хоть что-то найти. Уверен, спустя столько лет вы уже ничего не обнаружите.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            }
        };

        allDialogues["DialogueQuest7.1"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Мы не будем находиться здесь слишком долго. Мы справимся, слышишь? Я уверен, что мы найдём хоть что-то.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Надеюсь ты прав…",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };
        allDialogues["DialogueQuest7.2"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Это… это же Финник и Ихвильнихт, только маленькие… откуда это тут?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Ну… слушай, теперь мы знаем, что в пожаре виновен кто-то из них. У нас, так называемый, прогресс. Ну что, следопыт, раскроем это дело, а?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Да! Я думаю, имеет смысл заглянуть к Терентию. Возможно, он что-то знает об этом.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
        };

        allDialogues["DialogueQuest8"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Доброе утро, молодёжь. Кажется, ты пришёл не семена рассматривать, я прав?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Мы тут с Тиоли случайно наткнулись на вот этот старый рисунок. Это… он лежал в завалах, где раньше был дом Тиоли.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Хм… ты точно готов это услышать?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Да. Мне нужно знать правду.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "2017 год. Финник тогда уже вовсю врал, но… это был несчастный случай. Финник с другими лисами хотели просто напугать семью Тиоли – бросить горящую ветку в окно. Но ветер изменил своё направление…",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Ихвильнихт действительно был первым на месте. Он пытался спасти их… но было поздно. Жители решили просто обвинить его, не разбираясь.",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Это… глупо. Разве Ихвильнихт чем-то заслужил к себе такое отношение? Почему подумали на него?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "К сожалению, здесь даже я не смог ничем помочь, жители наотрез отказываются верить во что-то иное. Тем более без доказательств. Именно поэтому я решил на время уйти в отставку.",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Отставку?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Я был хранителем леса. Но эта история навсегда изменила нашу деревню. Сейчас на моё место метит Финник. Слушай… не допусти этого, ладно?",
                speakerName = "Терентий",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Я постараюсь поступить правильно.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
        };

        allDialogues["DialogueQuest9.1"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Боже… что здесь было?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Привет! Ой... эм, а что тут произошло?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            }
        };
        allDialogues["DialogueQuest9.2"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Я… не уверен. Я думал, что это Финник, но… посмотри, здесь волчьи следы и шерсть.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Терентий наврал? Я теперь ничего не понимаю.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Думаю, пока рано делать выводы. Думаю, имеет смысл сходить к Ихвильнихту и к Финнику, поговорить и посмотреть, как они будут себя вести.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            new DialogueLine
            {
                text = "Думаю, ты прав. Ну что, тогда не будем терять времени?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
        };
        allDialogues["DialogueQuest10"] = new List<DialogueLine>
        {
            //0
            new DialogueLine
            {
                text = "(с подозрением). Опять вы. На этот раз что?",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            //1
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Мы нашли волчью шерсть у моего дома. Это твоё?", nextLineIndex = 2}, // плохой путь. + балл тима фокса
                    new DialogueChoice { buttonText = "Кто-то подстроил это, чтобы снова обвинить тебя.", nextLineIndex = 10}, // хороший путь. + балл за тиму волка
                    new DialogueChoice { buttonText = "(молча показать клочок шерсти)", nextLineIndex = 13} //нейтральная позиция
                }
            },
            // вариант плохого пути.
            // 2
            new DialogueLine
            {
                text = "Ты действительно думаешь, что я стал бы пачкать лапы в огороде? (показывает когти – они чистые, без земли).",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 3 - выбор Гриши после этого
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Лапы можно и помыть. Это ещё совсем ничего не значит.", nextLineIndex = 4}, // плохой путь. + балл тима фокса
                    new DialogueChoice { buttonText = "Но как тогда можно это объяснить? Почему там твоя шерсть?", nextLineIndex = 6}, // хороший путь. + балл за тиму волка
                }
            },
            // 4 - про помыть лапы
            new DialogueLine
            {
                text = "(в его голосе прозвучал холод). Хорошо. Пусть так. Я не заставляю тебя поверить мне. Переживу как-нибудь. Мне уже столько лет никто не верил.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 5 - конец диалога
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[] 
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
            // 6 - (но как тогда можно это объяснить?)
            new DialogueLine
            {
                text = "Я честно не знаю. Но я правда не был в самой деревне в ближайший месяц точно. Меня изгнали, вход запрещён. Если кто-то из жителей меня там увидит – у меня будут жуткие неприятности.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 7
            new DialogueLine
            {
                text = "Ладно… мы тебя услышали. Я постараюсь добраться до правды.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 8 
            new DialogueLine
            {
                text = "Я тебе верю.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 9 - конец диалога
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
            // хвариант хорошего пути
            // 10
            new DialogueLine
            {
                text = "Наконец-то кто-то шевелит мозгами… Да, это подстава.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 11
            new DialogueLine
            {
                text = "Хорошо, значит, ситуация гораздо запутаннее. Мне нужно обдумать и возможно найти ещё зацепок. Думаю, мы близки к разгадке.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 12 - конец
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
            // вариант нейтрального пути
            // 13
            new DialogueLine
            {
                text = "Что за… откуда она там? Я не выхожу из леса уже очень много времени. Меня хотят подставить.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 14
            new DialogueLine
            {
                text = "(с подозрением) Знаешь… мне очень тяжело поверить в твои слова после этой находки. Но я пока не готов делать никаких выводов.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 15
            new DialogueLine
            {
                text = "(с грустью) Я тебя услышал. Надеюсь, ты сделаешь правильный выбор.",
                speakerName = "Ихвильнихт",
                speakerFace = null,
                isPlayer = false
            },
            // 16 - конец
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
        };
        allDialogues["TioliDialogueQuest11"] = new List<DialogueLine>
        {
            new DialogueLine
            {
                text = "Если честно, я немного волнуюсь. Ощущение, что все пытаются спрятать от нас правду. Как мы поймём, кто из них точно врёт, а кто нет?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            new DialogueLine
            {
                text = "Я не знаю, кому из них стоит поверить. Но что я могу сказать точно - к разгадке мы близки. Держись рядом со мной.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            }
        };

        allDialogues["DialogueQuest11"] = new List<DialogueLine>
        {
            // 0 
            new DialogueLine
            {
                text = "О-о, это же наш новенький! И кролик. Привет, Тиоли. Давно с тобой не виделись. (странно взглянул на Тиоли).",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 1
            new DialogueLine
            {
                text = "(завёл Тиоли слегка за свою спину). Мы не веселиться сюда пришли. Разговор есть.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 2
            new DialogueLine
            {
                text = "(делает удивлённое выражение лица). Разговор? Ко мне? Ну, выкладывай. Не уверен, что знаю все сплетни в этой деревне, но раз уж ты так сильно хочешь…",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 3 - выбираем путь
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Мы знаем, что ты подбросил шерсть Ихвильнихта.", nextLineIndex = 4}, // хороший путь и поинт к тиме волка
                    new DialogueChoice { buttonText = "Мне нужна твоя правда. Не их - твоя.", nextLineIndex = 12}, // плохой путь и поинт к тиме фокса
                    new DialogueChoice { buttonText = "(молча положил перед ним шерсть волка).", nextLineIndex = 18} // нейтральный путь и поинт к тиме волка
                }
            },
            // вариант хорошего пути
            // 4
            new DialogueLine
            {
                text = "Доказательства есть? Нет? Ну тогда не трать моё время. Вы просто не понимаете, куда ввязались. Лучше бы вам прекратить расследование прямо сейчас. (угрожающе посмотрел в глаза Грише).",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 5
            new DialogueLine
            {
                text = "Выбрал тактику угроз? Знаешь, ты мог бы придумать что-то получше. Наверняка тебе как-то хватило ума придумать весь этот конфликт.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 6
            new DialogueLine
            {
                text = "А кто тебе вообще поверит, новенький? Я живу в этой деревне всю свою жизнь, а ты только приехал. Тебе никто не поверит.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 7
            new DialogueLine
            {
                text = "Ты нас недооцениваешь.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            // 8
            new DialogueLine
            {
                text = "Сейчас бы пугаться слов маленького кролика. Вы мне не угроза. Вы всё равно никого не убедите.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 9
            new DialogueLine
            {
                text = "Посмотрим, кто будет смеяться последним. Готовь чемоданы. Уверен, в ближайшее время ты будешь искать новое жильё.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 10 - конец диалога
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
            // вариант плохого пути
            // 12
            new DialogueLine
            {
                text = "Моя правда? Моя правда в том… что я здесь абсолютно невиновен. Ихвильнихт… у него были причины так сделать. Он не очень ладил со старшим братом Тиоли. (Финник странно смотрел на Тиоли, словно ожидая чего-то).",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 13
            new DialogueLine
            {
                text = "О чём ты говоришь? Они хорошо ладили. Особенно в последние годы… жизни моего брата. Моей семьи.",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            // 14
            new DialogueLine
            {
                text = "Ты маленький наивный кролик. Прямо, как и твой брат. Поэтому он умер.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 15
            new DialogueLine
            {
                text = "Ты… ты хоть понимаешь, что ты говоришь и кому ты это говоришь?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 16
            new DialogueLine
            {
                text = "Ты хотел правды? Я тебе её дал. (лис странно избегал зрительного контакта).",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 17
            new DialogueLine
            {
                text = "Ты явно не всё рассказал. Если хоть что-то в твоих словах было правдой.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 18
            new DialogueLine
            {
                text = "Как знаешь.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 19
            new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
            },
            // вариант нейтрального пути
            // 20
            new DialogueLine
            {
                text = "И зачем ты принёс мне шерсть? (презрительно взглянул на клочок, но стал нервознее).",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 21
            new DialogueLine
            {
                text = "Ничего не хочешь мне сказать? А точнее, сказать Тиоли?",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
            // 22
            new DialogueLine
            {
                text = "Не понимаю, к чему ты клонишь. Я, если ты вдруг не заметил, лис, а не волк. И у меня рыжая шерсть, а не тёмная. Так что это уж точно не моё.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
            // 23
            new DialogueLine
            {
                text = "Мы нашли это у дома Гриши, там словно кто-то копался. Скажи честно, это твоих рук дело?",
                speakerName = "Тиоли",
                speakerFace = null,
                isPlayer = false
            },
            // 24
             new DialogueLine
            {
                text = "Я? Какой смысл мне таким заниматься? Я кандидат на следующего хранителя этого леса. Мне… мне просто это не нужно.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
             // 25
             new DialogueLine
            {
                text = "Ты ведёшь себя очень подозрительно. Если верить уликам – всё ведёт к Ихвильнихту, но исходя из диалога, доверия к тебе у нас ещё меньше.",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true
            },
             // 26
             new DialogueLine
            {
                text = "Я уверен, что ты пожалеешь о своих словах. Просто запомни.",
                speakerName = "Финник",
                speakerFace = null,
                isPlayer = false
            },
             // 27
             new DialogueLine
            {
                text = "",
                speakerName = "Гриша",
                speakerFace = null,
                isPlayer = true,
                choices = new DialogueChoice[]
                {
                    new DialogueChoice { buttonText = "Закончить диалог", nextLineIndex = -2}
                }
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
