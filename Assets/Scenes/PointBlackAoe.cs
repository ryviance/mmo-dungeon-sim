using UnityEngine;

public class PointBlankAoe : MonoBehaviour
{
    public GameObject pointBlankAoeIndicator; // Reference to the AoE indicator prefab

    [Header("AoE Settings")]
    public float indicatorDuration = 1f; // Duration the indicator is visible before the attack
    public float castTime = 3f; // Default cast time of the attack
    public float pointBlankAoeRadius = 5f; // Radius of the AoE circle
    public int pointBlankDamage = 30; // Default damage dealt to the player

    private GameObject currentIndicator; // Instance of the AoE indicator
    public BossUIController bossUIController; // Reference to the BossUIController

    private float? customCastTime; // Store custom cast time temporarily for Invoke usage
    private int? customDamageToApply; // Store custom damage temporarily for Invoke usage
    private float? customIndicatorDuration; // Store custom indicator duration temporarily for Invoke usage

    // Method to execute the AoE attack with customizable parameters
    public void ExecuteAttack(string customCastName = "Eye Of The Storm", float? customCastTime = null, int? customDamage = null, float? customIndicatorDuration = null)
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting(customCastName, customCastTime ?? castTime); // Show cast name on UI and set cast time
        }

        // Show the AoE indicator only in the last moment of the cast
        Invoke(nameof(ShowAoEIndicator), (customCastTime ?? castTime) - (customIndicatorDuration ?? indicatorDuration));
        Invoke(nameof(CastPointBlank), customCastTime ?? castTime); // Delay the attack by the total cast time
    }

    private void CastPointBlank()
    {
        // Find all colliders within the AoE radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, pointBlankAoeRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(customDamageToApply ?? pointBlankDamage);
                    Debug.Log("Player took " + (customDamageToApply ?? pointBlankDamage) + " damage from AoE!");
                }
            }
        }

        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        // Reset cast bar after the attack
        if (bossUIController != null)
        {
            bossUIController.UpdateCastBar(0); // Reset cast bar
            bossUIController.HideCastUI(); // Hide UI after cast completion
        }
    }

    private void ShowAoEIndicator()
    {
        if (pointBlankAoeIndicator != null)
        {
            currentIndicator = Instantiate(pointBlankAoeIndicator, transform.position, Quaternion.identity);
            currentIndicator.transform.localScale = new Vector3(pointBlankAoeRadius * 2, 0.2f, pointBlankAoeRadius * 2); // Scale the indicator to match AoE
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pointBlankAoeRadius);
    }
}