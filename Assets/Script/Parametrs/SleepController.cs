using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepController : Sounds
{
    [Header("Основные настройки")]
    public Slider sleepSlider;
    public float maxSleep = 100f;
    public float depletionRate = 1f;
    public float depletionAmount = 0.5f;
    
    [Header("Цвета состояния")]
    public Color wellRestedColor = Color.blue;
    public Color tiredColor = new Color(1f, 0.5f, 0f);
    public Color exhaustedColor = Color.red;

    [Header("Затемнение")]
    public CanvasGroup darknessPanel;

    [Header("Истощение")]
    public GameObject warningPanel;
    public Text warningText;
    public Text countdownText;
    public float warningTime = 10f;

    private float currentSleep;
    private float countdownTimer;
    private bool warningActive = false;
    private bool isDestroying = false;

    [Header("Звуки")]
    public AudioClip tickSound;
    public AudioClip alarmSound;
    private bool alarmPlayed = false;
    private Coroutine tickCoroutine;
    public bool isSleeping = false;

    private void Start()
    {
        currentSleep = maxSleep;
        UpdateSleepUI();
        
        if (darknessPanel != null)
        {
            darknessPanel.alpha = 0;
            darknessPanel.blocksRaycasts = false;
        }
        if (warningPanel != null) 
            warningPanel.SetActive(false);
    }

    private void Update()
    {
        if (isDestroying) return;
        
        currentSleep -= depletionAmount * depletionRate * Time.deltaTime;
        currentSleep = Mathf.Clamp(currentSleep, 0f, maxSleep);
        UpdateSleepUI();
        
        if (darknessPanel != null && !isSleeping)
        {
            float sleepPercentage = currentSleep / maxSleep * 100f;
            darknessPanel.alpha = sleepPercentage < 30f ? 0.7f * (1f - sleepPercentage / 30f) : 0f;
        }

        if (currentSleep <= 0 && !warningActive && !isDestroying)
        {
            StartExhaustionWarning();
        }
        
        if (warningActive)
        {
            countdownTimer -= Time.deltaTime;
            countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
            
            if (countdownTimer <= 3f && !alarmPlayed)
            {
                alarmPlayed = true;
                PlaySound(alarmSound);
            }
            
            if (countdownTimer <= 0)
            {
                if (tickCoroutine != null)
                    StopCoroutine(tickCoroutine);
                
                warningActive = false;
                isDestroying = true;
                StartCoroutine(DestroyAllCropsCoroutine());
            }
        }
    }

    public bool HasEnoughEnergy(float amount)
    {
        return currentSleep >= amount;
    }

    public void ConsumeEnergy(float amount)
    {
        currentSleep = Mathf.Clamp(currentSleep - amount, 0f, maxSleep);
        UpdateSleepUI();
    }

    public void SetDepletionRate(float rate)
    {
        depletionRate = rate;
    }

    public void RestoreSleep(float amount)
    {
        currentSleep = Mathf.Clamp(currentSleep + amount, 0f, maxSleep);
        UpdateSleepUI();
        
        if (warningActive)
        {
            warningActive = false;
            
            if (tickCoroutine != null)
                StopCoroutine(tickCoroutine);
            
            if (warningPanel != null)
                warningPanel.SetActive(false);
            
            if (FarmGrid.Instance != null)
                FarmGrid.Instance.ResumeMusic();
        }
    }

    private void UpdateSleepUI()
    {
        if (sleepSlider == null) return;
        sleepSlider.value = currentSleep;
        
        float sleepPercentage = currentSleep / maxSleep * 100f;
        
        if (sleepPercentage >= 50f)
            sleepSlider.fillRect.GetComponent<Image>().color = wellRestedColor;
        else if (sleepPercentage >= 30f)
            sleepSlider.fillRect.GetComponent<Image>().color = tiredColor;
        else
            sleepSlider.fillRect.GetComponent<Image>().color = exhaustedColor;
    }

    private void StartExhaustionWarning()
    {
        warningActive = true;
        countdownTimer = warningTime;
        alarmPlayed = false;
        
        if (warningPanel != null)
        {
            warningPanel.SetActive(true);
            if (warningText != null)
                warningText.text = "БЕГОМ В КРОВАТЬ!\nВсе растения погибнут!";
        }
        
        tickCoroutine = StartCoroutine(TickCoroutine());

        if (FarmGrid.Instance != null)
            FarmGrid.Instance.PauseMusic();
    }

    private IEnumerator DestroyAllCropsCoroutine()
    {
        if (darknessPanel != null)
        {
            float elapsed = 0f;
            float duration = 1.5f;
            while (elapsed < duration)
            {
                darknessPanel.alpha = Mathf.Lerp(darknessPanel.alpha, 1f, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            darknessPanel.alpha = 1f;
        }
        
        yield return new WaitForSeconds(2f);
        
        if (CropsManager.Instance != null)
        {
            var cropKeys = new List<Vector2Int>(CropsManager.Instance.allCrops.Keys);
            foreach (var pos in cropKeys)
            {
                var crop = CropsManager.Instance.allCrops[pos];
                if (crop != null)
                    Destroy(crop.gameObject);
            }
            CropsManager.Instance.allCrops.Clear();

            EcologyController ecoController = FindObjectOfType<EcologyController>();
            if (ecoController != null)
                ecoController.ReduceEco(cropKeys.Count * 3f); // -3 за каждое растение
            
            SoilTile[] allTiles = FindObjectsOfType<SoilTile>();
            foreach (var tile in allTiles)
            {
                if (tile.isPlanted)
                    tile.MarkHarvested();
            }
        }
        
        currentSleep = maxSleep;
        UpdateSleepUI();
        
        if (warningPanel != null)
            warningPanel.SetActive(false);
        
        if (darknessPanel != null)
        {
            float elapsed = 0f;
            float duration = 1.5f;
            while (elapsed < duration)
            {
                darknessPanel.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            darknessPanel.alpha = 0f;
        }
        
        isDestroying = false;

        if (FarmGrid.Instance != null)
            FarmGrid.Instance.ResumeMusic();
    }

    private IEnumerator TickCoroutine()
    {
        int ticks = Mathf.CeilToInt(warningTime); // 10 тиков для 10 секунд
        for (int i = 0; i < ticks; i++)
        {
            if (!warningActive) break;
            PlaySound(tickSound);
            yield return new WaitForSeconds(1f);
        }
    }
}