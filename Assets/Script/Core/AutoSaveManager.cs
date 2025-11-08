using UnityEngine;
using System.Collections;

public class AutoSaveManager : MonoBehaviour
{
    public float saveInterval = 600f; // 10 минут = 600 секунд

    private void Start()
    {
        StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            SaveSystem.SaveModifiedTiles();
        }
    }
}
