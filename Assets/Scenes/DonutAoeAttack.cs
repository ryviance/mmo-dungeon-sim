using UnityEngine;

public class DonutAoeAttack : MonoBehaviour
{
    public GameObject donutAoeIndicator; // Reference to the AoE indicator prefab

    [Header("AoE Settings")]
    public float indicatorDuration = 1f; // Duration the indicator is visible before the attack
    public float castTime = 3f; // Default cast time of the attack
    public float outerRadius = 5f; // Outer radius of the AoE donut
    public float innerRadius = 2f; // Inner radius of the AoE donut
    public int donutDamage = 30; // Default damage dealt to the player

    private GameObject currentIndicator; // Instance of the AoE indicator
    public BossUIController bossUIController; // Reference to the BossUIController

    private float? customCastTime; // Store custom cast time temporarily for Invoke usage
    private int? customDamageToApply; // Store custom damage temporarily for Invoke usage
    private float? customIndicatorDuration; // Store custom indicator duration temporarily for Invoke usage

    // Method to execute the AoE attack with customizable parameters
    public void ExecuteAttack(string customCastName = "Vortex Of The Storm", float? customCastTime = null, int? customDamage = null, float? customIndicatorDuration = null)
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting(customCastName, customCastTime ?? castTime); // Show cast name on UI and set cast time
        }

        // Show the AoE indicator only in the last moment of the cast
        Invoke(nameof(ShowAoEIndicator), (customCastTime ?? castTime) - (customIndicatorDuration ?? indicatorDuration));
        Invoke(nameof(CastDonutAoe), customCastTime ?? castTime); // Delay the attack by the total cast time
    }

    private void CastDonutAoe()
    {
        // Find all colliders within the outer AoE radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, outerRadius);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // Calculate distance from the center of the AoE
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                // Check if the player is within the outer radius but outside the inner radius
                if (distance < outerRadius && distance >= innerRadius)
                {
                    PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(customDamageToApply ?? donutDamage);
                        Debug.Log("Player took " + (customDamageToApply ?? donutDamage) + " damage from Donut AoE!");
                    }
                }
            }
        }

        // Destroy the indicator after the attack
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
        if (donutAoeIndicator != null)
        {
            // Get the Tiger's current position
            Vector3 tigerPosition = transform.position;

            // Create a new position with the Tiger's X and Z, and set Y to 0.1
            Vector3 spawnPosition = new Vector3(tigerPosition.x, 0.1f, tigerPosition.z);

            // Instantiate the donut AoE indicator at the specified position
            currentIndicator = Instantiate(donutAoeIndicator, spawnPosition, Quaternion.identity);

            // Find the InnerCircle and OuterCircle objects within the indicator prefab
            Transform innerCircle = currentIndicator.transform.Find("Inner Circle");
            Transform outerCircle = currentIndicator.transform.Find("Outer Circle");

            // Scale the outer circle to match the outer radius
            if (outerCircle != null)
            {
                outerCircle.localScale = new Vector3(outerRadius * 2, 0.2f, outerRadius * 2);
            }

            // Scale the inner circle to match the inner radius
            if (innerCircle != null)
            {
                innerCircle.localScale = new Vector3(innerRadius * 2, 0.2f, innerRadius * 2);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, outerRadius); // Draw the outer wire sphere for the donut
        Gizmos.DrawWireSphere(transform.position, innerRadius); // Draw the inner wire sphere for the donut
    }
}