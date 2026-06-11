using UnityEngine;
using UnityEngine.UI;

public class EcologyController : MonoBehaviour
{
    [Header("Основные настройки")]
    [SerializeField] public Slider ecoSlider;
    [SerializeField] public float maxEco = 200f;
    public float deplicationRate = 1f;
    public float deplicationAmount = 0.01f;

    [Header("Настройка цвета")]
    [SerializeField] public Color wellColor = Color.green;
    [SerializeField] public Color sosoColor = new Color(1f, 0.5f, 0f);
    [SerializeField] public Color badColor = Color.red;

    private float currentEco;

    private void Start()
    {
        currentEco = 0;
        UpdateEcoUI();
    }

    void Update()
    {
        currentEco -= deplicationAmount * deplicationRate * Time.deltaTime;
        currentEco = Mathf.Clamp(currentEco, 0f, maxEco);
        UpdateEcoUI();
    }
    
    public float CurrentEco
    {
        get => currentEco;
        set
        {
            currentEco = Mathf.Clamp(value, 0f, maxEco);
            UpdateEcoUI();
        }
    }

    public void RestoreEco(float amount)
    {
        currentEco = Mathf.Clamp(currentEco + amount, 0f, maxEco);
        UpdateEcoUI(); 
    }

    public void ReduceEco(float amount)
    {
        currentEco = Mathf.Clamp(currentEco - amount, 0f, maxEco);
        UpdateEcoUI();
    }

    private void UpdateEcoUI()
    {
        if (ecoSlider == null) return;
        
        ecoSlider.value = currentEco;
        
        float ecoPercentage = currentEco / maxEco * 100f;

        if (ecoPercentage >= 50f)
            ecoSlider.fillRect.GetComponent<Image>().color = wellColor;
        else if (ecoPercentage >= 30f)
            ecoSlider.fillRect.GetComponent<Image>().color = sosoColor;
        else
            ecoSlider.fillRect.GetComponent<Image>().color = badColor;
    }
}