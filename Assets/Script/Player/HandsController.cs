using UnityEngine;

public class HandsController : MonoBehaviour
{
    public SpriteRenderer handSpriteRenderer;
    private Animator handAnimator;
    
    void Start()
    {
        handAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        bool isUsingItem = IsUsingItem();
        
        // Управляем аниматором
        if (handAnimator != null)
        {
            handAnimator.SetBool("IsUsing", isUsingItem);
        }
        
        // Включаем/выключаем рендерер в зависимости от состояния анимации
        if (handAnimator != null && handAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            handSpriteRenderer.enabled = false;
        }
        else
        {
            handSpriteRenderer.enabled = true;
        }
    }
    
    bool IsUsingItem()
    {
        return Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.E);
    }
}
