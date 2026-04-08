using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSrc => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume=1f, bool destroyed= false, float p1 = 1f, float p2 = 1f){
        // Проверка на случай если AudioSource не найден
        if (audioSrc == null)
        {
            Debug.LogError($"AudioSource не найден на объекте {gameObject.name}");
            return;
        }
        audioSrc.pitch = Random.Range(p1, p2);
        audioSrc.PlayOneShot(clip, volume);
    }
}