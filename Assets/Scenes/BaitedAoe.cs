using UnityEngine;
using System.Collections;

public class BaitedAoe : MonoBehaviour
{
    public GameObject baitedAoeIndicator; // Reference to the AoE indicator prefab
    public float indicatorDuration = 1f; // Duration each AoE indicator is active
    public float castTime = 3f; // Cast time before AoEs start appearing
    public float aoeRadius = 5f; // Radius of each AoE circle
    public int aoeDamage = 30; // Damage dealt to the player
    public int numberOfAoes = 3; // Number of sequential AoEs to spawn
    public float delayBetweenAoes = 0.5f; // Delay between each AoE spawning

    private int currentAoeCount = 0; // Tracks the number of AoEs spawned
    public Transform player; // Reference to the player
    public BossUIController bossUIController; // Reference to the Boss UI

    public void StartBaitedAoE()
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting("Chain Lightning", castTime); // Display cast bar and name
        }

        // Start AoE spawning after the cast time completes
        Invoke(nameof(BeginAoeSequence), castTime);
    }

    private void BeginAoeSequence()
    {
        currentAoeCount = 0; // Reset AoE count
        TriggerNextAoe(); // Start spawning AoEs
    }

    private void TriggerNextAoe()
    {
        if (currentAoeCount < numberOfAoes)
        {
            ShowAoEIndicator();
            currentAoeCount++;
            Invoke(nameof(TriggerNextAoe), delayBetweenAoes); // Schedule the next AoE
        }
        else if (bossUIController != null)
        {
            // Clear cast bar UI after all AoEs are spawned
            bossUIController.UpdateCastBar(0);
            bossUIController.HideCastUI();
        }
    }

    private void ShowAoEIndicator()
    {
        if (baitedAoeIndicator != null && player != null)
        {
            // Spawn the AoE indicator at the player's current position
            Vector3 spawnPosition = new Vector3(player.position.x, 0.1f, player.position.z);
            GameObject aoeIndicator = Instantiate(baitedAoeIndicator, spawnPosition, Quaternion.identity);
            aoeIndicator.transform.localScale = new Vector3(aoeRadius * 2, 0.2f, aoeRadius * 2); // Scale to AoE size

            // Start the coroutine to check and deal damage right before the AoE disappears
            StartCoroutine(DelayedDamageCheck(aoeIndicator.transform.position, indicatorDuration - 0.05f));

            // Destroy the AoE indicator after its duration
            Destroy(aoeIndicator, indicatorDuration);
        }
    }

    private IEnumerator DelayedDamageCheck(Vector3 aoePosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        CheckAndDealDamage(aoePosition);
    }

    private void CheckAndDealDamage(Vector3 aoePosition)
    {
        if (player != null)
        {
            // Check if the player is within this specific AoE instance's radius
            if (Vector3.Distance(player.position, aoePosition) <= aoeRadius)
            {
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(aoeDamage);
                    Debug.Log($"Player took {aoeDamage} damage from Baited AoE at position {aoePosition}!");
                }
            }
        }
    }
}