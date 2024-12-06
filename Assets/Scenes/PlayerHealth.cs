using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Maximum health value
    private float currentHealth;
    public Image healthBarFill; // Reference to the HP fill image

    void Start()
    {
        currentHealth = maxHealth; // Initialize health
        UpdateHealthBar();
    }

    // Function to update health
    public void TakeDamage(float damage)
    {
        Debug.Log("Take Damage called.");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Keep health within bounds
        UpdateHealthBar();

        // Check if health is 0 and trigger respawn
        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void UpdateHealthBar()
    {
        healthBarFill.fillAmount = currentHealth / maxHealth; // Update the fill based on health
    }

    // Respawn the player by reloading the scene
    private void Respawn()
    {
        // Reload the current scene to reset the dungeon
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}