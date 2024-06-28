using UnityEngine;

public class LostObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LostCounter lostCounter = FindObjectOfType<LostCounter>();
            if (lostCounter != null)
            {
                lostCounter.IncrementLostCount();
            }
            Destroy(gameObject); // Destroy the object with the Lost tag
        }
    }
}
