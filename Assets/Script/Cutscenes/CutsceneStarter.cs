using UnityEngine;

public class CutsceneStarter : MonoBehaviour
{
    public CutsceneQuestSeventhPart2 cutscene;
    void OnEnable()
    {
        if (cutscene != null)
            cutscene.StartCutscene();
        else
            Debug.LogError("CutsceneQuestSeventhPart2 не назначен");
    }
}
