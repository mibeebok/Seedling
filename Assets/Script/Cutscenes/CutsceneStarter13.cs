using UnityEngine;

public class CutsceneStarter13 : MonoBehaviour
{
    public CutsceneQuest13 cutscene;
    void OnEnable()
    {
        cutscene?.StartCutscene();
    }
}
