using UnityEngine;
using TMPro;

public class KillCounter : MonoBehaviour
{
    private int killCount = 0; // Initialize kill count
    public TextMeshProUGUI killCountText; // Reference to the TextMeshPro component

    // Method to increment kill count and update the TextMeshPro text
    public void IncrementKillCount()
    {
        killCount++;
        Debug.Log("Kill Count: " + killCount); // Print kill count for debugging

        // Update the TextMeshPro text to display the current kill count
        if (killCountText != null)
        {
            killCountText.text = "Kills: " + killCount + "/100";
        }
    }
}
