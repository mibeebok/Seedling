using UnityEngine;

public class SavePosition : MonoBehaviour
{
    public GameObject player;

    public void Save()
    {
        if (player == null)
        {
            Debug.LogError("[SavePosition] Игрок не назначен");
            return;
        }

        // SaveSystem.SaveGame();

        PlayerPrefs.SetFloat("playerX", player.transform.position.x);
        PlayerPrefs.SetFloat("playerY", player.transform.position.y);
       
        PlayerPrefs.Save();

        Debug.Log("[SavePosition] Игра успешно сохарнена");
    }
}
