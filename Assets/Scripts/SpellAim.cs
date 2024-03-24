using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAim : MonoBehaviour
{
   public GameObject spellPrefab; // Reference to the spell projectile prefab
    public Transform wandTip; // Reference to the position where the spell starts

    void Update()
    {
        if (Input.GetButtonDown("Fire1")) // Change "Fire1" to whatever input you use for shooting
        {
            // Get the mouse position in world space
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0f; // Ensure the target position is on the same z-axis as the wandTip

            // Instantiate the spell projectile at the wandTip position
            GameObject spell = Instantiate(spellPrefab, wandTip.position, Quaternion.identity);

            // Calculate the direction towards the target position
            Vector3 direction = (targetPosition - wandTip.position).normalized;

            // Apply initial velocity or force to the spell projectile
            Rigidbody spellRigidbody = spell.GetComponent<Rigidbody>();
            spellRigidbody.velocity = direction * 10; // Adjust spellSpeed as needed
        }
    }
}
