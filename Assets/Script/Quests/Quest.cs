using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;
    public List<QuestTask> tasks;
    public string completionNotes;
    public int rewardMoney;
    public CropType rewardCropType;
    public int rewardSeedCount;
    public List<NPCDialogueChange> npcDialogueChanges;
}

[System.Serializable]
public class NPCDialogueChange
{
    public string npcName;
    public string newDialogueKey;
}
