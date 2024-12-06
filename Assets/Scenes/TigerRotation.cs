using UnityEngine;

public class TigerRotation : MonoBehaviour
{
    public PointBlankAoe pointBlankAttack;
    public DonutAoeAttack donutAttack;
    public LeftRightCleave leftRightAttack;
    public BaitedAoe baitedAoe;
    public FusionAttacks fusionAttack;
    public float attackOrderCooldown = 5f;

    private float lastAttackTime = 0f;
    private int currentAttackIndex = 0;
    private bool hasStartedAttacking = false;

    public Collider aggroRangeCollider; // Reference to the child's collider

    void Start()
    {
        // Find the aggro range collider (child object) if it's not set in the inspector
        if (aggroRangeCollider == null)
        {
            aggroRangeCollider = GetComponentInChildren<Collider>(); // This will find the first collider in the child objects
        }
    }

    void Update()
    {
        if (hasStartedAttacking && Time.time >= lastAttackTime + attackOrderCooldown)
        {
            ExecuteNextAttack();
            lastAttackTime = Time.time;
            currentAttackIndex = (currentAttackIndex + 1) % 9; // Loop back to the first attack after the last
        }
    }

    private void ExecuteNextAttack()
    {
        switch (currentAttackIndex)
        {
            case 0:
                pointBlankAttack.ExecuteAttack();
                break;
            case 1:
                donutAttack.ExecuteAttack();
                break;
            case 2:
                leftRightAttack.CleaveLeft();
                break;
            case 3:
                leftRightAttack.CleaveRight();
                break;
            case 4:
                baitedAoe.StartBaitedAoE();
                break;
            case 5:
                fusionAttack.ExecutePointBlankWithLeftCleave();
                break;
            case 6:
                fusionAttack.ExecutePointBlankWithRightCleave();
                break;
            case 7:
                fusionAttack.ExecuteDonutWithLeftCleave();
                break;
            case 8:
                fusionAttack.ExecuteDonutWithRightCleave();
                break;
        }
    }

    // Trigger detection for the player entering the aggro range collider
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered aggro range!");
            // Start the attacking sequence
            hasStartedAttacking = true;
        }
    }
}