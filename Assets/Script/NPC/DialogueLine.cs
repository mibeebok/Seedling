using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string text;
    public string speakerName;
    public Sprite speakerFace;
    public bool isPlayer;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueChoice
{
    public string buttonText;
    public int nextLineIndex;
}
