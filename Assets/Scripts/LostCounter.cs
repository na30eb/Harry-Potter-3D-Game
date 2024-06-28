using UnityEngine;
using TMPro;

public class LostCounter : MonoBehaviour
{
    private static int lostCount = 0; // Initialize lost count
    public TextMeshProUGUI lostCountText; // Reference to the TextMeshPro component

    // Method to increment lost count and update the TextMeshPro text
    public void IncrementLostCount()
    {
        lostCount++;
        Debug.Log("Lost Count: " + lostCount); // Print lost count for debugging

        // Update the TextMeshPro text to display the current lost count
        if (lostCountText != null)
        {
            lostCountText.text = "Horcruxes: " + lostCount+" /4";
        }
    }
}


