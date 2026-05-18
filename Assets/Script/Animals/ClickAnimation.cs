using UnityEngine;
using UnityEngine.EventSystems; 

public class ClickAnimation : MonoBehaviour, IPointerClickHandler
{
    private Animator _animator;
    private AudioSource _audioSource;

    [SerializeField] private AudioClip clickSound;
    private const string CLICK_TRIGGER = "Click";

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_animator != null)
        {
            _animator.SetTrigger(CLICK_TRIGGER);
        }

        if (_audioSource != null && clickSound != null)
        {
            _audioSource.PlayOneShot(clickSound);
        }
    }
}