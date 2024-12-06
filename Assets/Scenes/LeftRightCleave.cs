using System.Collections;
using UnityEngine;

public class LeftRightCleave : MonoBehaviour
{
    public GameObject leftHalfCleave; // Left cleave GameObject with a box collider
    public GameObject rightHalfCleave; // Right cleave GameObject with a box collider

    [Header("Cleave Settings")]
    public float cleaveIndicatorDuration = 1.5f; // Duration the indicators are visible before attack
    public float castTime = 2f; // Default cast time before the attack snapshots
    public int cleaveDamage = 40; // Default damage dealt by the cleave

    public BossUIController bossUIController; // Reference to the Boss UI

    private float? customCastTime; // Store custom cast time temporarily for Invoke usage
    private int? customDamageToApply; // Store custom damage temporarily for Invoke usage
    private float? customIndicatorDuration; // Store custom indicator duration temporarily for Invoke usage

    public void CleaveLeft(string customCastName = "Sinistral Swipe", float? customCastTime = null, int? customDamage = null, float? customIndicatorDuration = null)
    {
        StartCasting(customCastName, customCastTime);
        Invoke(nameof(ActivateLeftCleaveIndicator), (customCastTime ?? castTime) - (customIndicatorDuration ?? cleaveIndicatorDuration));

        customDamageToApply = customDamage ?? cleaveDamage;
        StartCoroutine(CallCastCleaveWithCustomDamageAfterDelay(customCastTime ?? castTime, leftHalfCleave));
    }

    public void CleaveRight(string customCastName = "Dextral Swipe", float? customCastTime = null, int? customDamage = null, float? customIndicatorDuration = null)
    {
        StartCasting(customCastName, customCastTime);
        Invoke(nameof(ActivateRightCleaveIndicator), (customCastTime ?? castTime) - (customIndicatorDuration ?? cleaveIndicatorDuration));

        customDamageToApply = customDamage ?? cleaveDamage;
        StartCoroutine(CallCastCleaveWithCustomDamageAfterDelay(customCastTime ?? castTime, rightHalfCleave));
    }

    // Coroutine to call CastCleaveWithCustomDamage with a delay for either left or right cleave
    private IEnumerator CallCastCleaveWithCustomDamageAfterDelay(float delay, GameObject halfCleave)
    {
        yield return new WaitForSeconds(delay);
        CastCleaveWithCustomDamage(halfCleave);

        // Immediately deactivate the cleave indicators after the cast time ends (damage snapshot time)
        DeactivateCleaveIndicators();
    }

    private void StartCasting(string customCastName, float? customCastTime)
    {
        string castName = customCastName;
        float castingTime = customCastTime ?? castTime;

        if (bossUIController != null)
        {
            bossUIController.StartCasting(castName, castingTime);
        }
    }

    private void ActivateLeftCleaveIndicator()
    {
        if (leftHalfCleave != null)
        {
            leftHalfCleave.SetActive(true); // Activate left cleave indicator
        }
    }

    private void ActivateRightCleaveIndicator()
    {
        if (rightHalfCleave != null)
        {
            rightHalfCleave.SetActive(true); // Activate right cleave indicator
        }
    }

    private void CastCleaveWithCustomDamage(GameObject halfCleave)
    {
        // Use the stored custom damage for this cleave
        int damage = customDamageToApply ?? cleaveDamage;

        // Call CastCleave for the correct cleave zone
        DealDamageInCleaveZone(halfCleave, damage);
    }

    private void DealDamageInCleaveZone(GameObject cleaveZone, int damage)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player object not found!");
            return;
        }

        BoxCollider boxCollider = cleaveZone.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            // Check if the player is inside the BoxCollider of the cleave zone
            if (boxCollider.bounds.Contains(player.transform.position))
            {
                // Apply damage to the player if they are inside the collider's bounds
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Debug.Log($"Player took {damage} damage from {cleaveZone.name}!");
                }
            }
            else
            {
                Debug.Log("Player is outside the cleave zone!");
            }
        }
    }

    private void DeactivateCleaveIndicators()
    {
        // Immediately deactivate the indicators after damage is applied
        if (leftHalfCleave != null) leftHalfCleave.SetActive(false);
        if (rightHalfCleave != null) rightHalfCleave.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        // Draw Gizmo for Left Cleave
        if (leftHalfCleave != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); // Red with transparency
            BoxCollider leftCleaveCollider = leftHalfCleave.GetComponent<BoxCollider>();
            if (leftCleaveCollider != null)
            {
                // Set Gizmos matrix to match the left cleave GameObject's transformation
                Gizmos.matrix = leftHalfCleave.transform.localToWorldMatrix;
                Gizmos.DrawCube(leftCleaveCollider.center, leftCleaveCollider.size);
            }
        }

        // Draw Gizmo for Right Cleave
        if (rightHalfCleave != null)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.5f); // Blue with transparency
            BoxCollider rightCleaveCollider = rightHalfCleave.GetComponent<BoxCollider>();
            if (rightCleaveCollider != null)
            {
                // Set Gizmos matrix to match the right cleave GameObject's transformation
                Gizmos.matrix = rightHalfCleave.transform.localToWorldMatrix;
                Gizmos.DrawCube(rightCleaveCollider.center, rightCleaveCollider.size);
            }
        }
    }
}