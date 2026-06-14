using UnityEngine;

public class CutsceneStarter14_2 : MonoBehaviour
{
    public CutsceneQuest14_2 cutscene;
    void OnEnable() => cutscene?.StartCutscene();
}
