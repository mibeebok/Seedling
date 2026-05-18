using UnityEngine;

public class ClickAnimation : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip clickSound;

    void Start()
    {
        // Кэшируем компоненты, чтобы не искать их каждый кадр
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayClick();
        }
    }

    void PlayClick()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("Click");
        }

        if (_audioSource != null && clickSound != null)
        {
            _audioSource.PlayOneShot(clickSound); 
        }
    }
}