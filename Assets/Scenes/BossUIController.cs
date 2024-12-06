using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIController : MonoBehaviour
{
    public Text bossNameText;
    public Text bossCastName;
    public Image healthBarFill;
    public Image castBarFill;
    public Image castBarBackground;

    private float bossMaxHealth = 1000f;
    private float currentHealth;
    private float castDuration;
    private float castProgress;

    // Property to store the current cast name
    public string CurrentCastName { get; private set; }

    void Start()
    {
        currentHealth = bossMaxHealth;
        UpdateHealthBar();
        UpdateCastBar(0);
        HideCastUI(); // Initially hide the cast bar and cast name
    }

    public void SetBossName(string name)
    {
        bossNameText.text = name;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, bossMaxHealth);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / bossMaxHealth;
    }

    public void StartCasting(string castName, float duration)
    {
        castProgress = 0;
        castDuration = duration;
        bossCastName.text = castName;
        CurrentCastName = castName; // Set the current cast name

        ShowCastUI(); // Show the cast bar and cast name at the start of a new cast
        StartCoroutine(CastRoutine());
    }

    private System.Collections.IEnumerator CastRoutine()
    {
        while (castProgress < castDuration)
        {
            castProgress += Time.deltaTime;
            UpdateCastBar(castProgress / castDuration);
            yield return null;
        }
        UpdateCastBar(1); // Ensure bar is full at the end of cast
        CurrentCastName = ""; // Reset the cast name once cast completes
        HideCastUI(); // Hide the cast bar and cast name once the cast completes
    }

    public void ShowCastUI()
    {
        castBarFill.gameObject.SetActive(true);
        bossCastName.gameObject.SetActive(true);
        castBarBackground.gameObject.SetActive(true);
    }

    public void HideCastUI()
    {
        castBarFill.gameObject.SetActive(false);
        bossCastName.gameObject.SetActive(false);
        castBarBackground.gameObject.SetActive(false);
    }

    public void UpdateCastBar(float progress)
    {
        castBarFill.fillAmount = progress;
    }
}