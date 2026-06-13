using UnityEngine;

public class CutsceneStarter9_2 : MonoBehaviour
{
    public CutsceneQuest9_2 cutscene;
    void OnEnable()
    {
        if (cutscene != null)
            cutscene.StartCutscene();
        else
            Debug.LogError("CutsceneQuest9_2 не назначен в CutsceneStarter9_2");
    }
}
