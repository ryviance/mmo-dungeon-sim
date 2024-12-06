// Note to self: Bug where these attacks + telegraphs aren't aligned to castbar

using UnityEngine;

public class FusionAttacks : MonoBehaviour
{
    public PointBlankAoe pointBlankAoe; // Reference to the PointBlankAoe component
    public DonutAoeAttack donutAoeAttack; // Reference to the DonutAoeAttack component
    public LeftRightCleave leftRightCleave; // Reference to the LeftRightCleave component
    public BossUIController bossUIController; // Reference to BossUIController

    // Parameters customizable in the Inspector
    [Header("Point Blank Settings")]
    public float pointBlankCastTime = 3f; // Default cast time for Point Blank
    public float pointBlankIndicatorDuration = 1f; // Default indicator duration for Point Blank
    public int pointBlankDamage = 30; // Default damage for Point Blank

    [Header("Donut Settings")]
    public float donutCastTime = 3f; // Default cast time for Donut
    public float donutIndicatorDuration = 1f; // Default indicator duration for Donut
    public int donutDamage = 30; // Default damage for Donut

    // Executes Point Blank AoE with Left Cleave
    public void ExecutePointBlankWithLeftCleave()
    {   
        if (bossUIController != null)
        {
            bossUIController.StartCasting("Sinistral Eye", pointBlankCastTime); // Show cast name on UI
        }
        pointBlankAoe.ExecuteAttack("Sinistral Eye", pointBlankCastTime, pointBlankDamage, pointBlankIndicatorDuration); // Start Point Blank AoE attack
        leftRightCleave.CleaveLeft("Sinistral Eye", pointBlankCastTime, pointBlankDamage, pointBlankIndicatorDuration); // Start Left Cleave attack

    }

    // Executes Point Blank AoE with Right Cleave
    public void ExecutePointBlankWithRightCleave()
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting("Dextral Eye", pointBlankCastTime); // Show cast name on UI
        }
        pointBlankAoe.ExecuteAttack("Dextral Eye", pointBlankCastTime, pointBlankDamage, pointBlankIndicatorDuration); // Start Point Blank AoE attack
        leftRightCleave.CleaveRight("Dextral Eye", pointBlankCastTime, pointBlankDamage, pointBlankIndicatorDuration); // Start Right Cleave attack
    }

    // Executes Donut AoE with Left Cleave
    public void ExecuteDonutWithLeftCleave()
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting("Sinistral Vortex", pointBlankCastTime); // Show cast name on UI
        }
        donutAoeAttack.ExecuteAttack("Sinistral Vortex", donutCastTime, donutDamage, donutIndicatorDuration); // Start Donut AoE attack
        leftRightCleave.CleaveLeft("Sinistral Vortex", donutCastTime, donutDamage, pointBlankIndicatorDuration); // Start Left Cleave attack
    }

    // Executes Donut AoE with Right Cleave
    public void ExecuteDonutWithRightCleave()
    {
        if (bossUIController != null)
        {
            bossUIController.StartCasting("Dextral Vortex", pointBlankCastTime); // Show cast name on UI
        }
        donutAoeAttack.ExecuteAttack("Dextral Vortex", donutCastTime, donutDamage, donutIndicatorDuration); // Start Donut AoE attack
        leftRightCleave.CleaveRight("Dextral Vortex", donutCastTime, donutDamage, pointBlankIndicatorDuration); // Start Right Cleave attack
    }
}
