using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource audioSrc => GetComponent<AudioSource>();

    public void PlaySound(AudioClip clip, float volume = 1f, bool destroyed = false, float p1 = 1f, float p2 = 1f)
    {
        if (audioSrc == null)
        {
            Debug.LogError($"AudioSource не найден на объекте {gameObject.name}");
            return;
        }
        
        // Учитываем глобальную громкость звуков
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        float finalVolume = volume * sfxVolume;
        
        audioSrc.pitch = Random.Range(p1, p2);
        audioSrc.PlayOneShot(clip, finalVolume);
    }
}