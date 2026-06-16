using UnityEngine;

public class ArrowPulse : MonoBehaviour
{
    public float amplitude = 5f;
    public float speed = 2f;
    private RectTransform rt;
    private float baseX;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        baseX = rt.anchoredPosition.x;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        rt.anchoredPosition = new Vector2(baseX + offset, rt.anchoredPosition.y);
    }
}