using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public float speed = 5f;
    Rigidbody rig;
    public HealthBar healthBar; // Reference to the HealthBar script

    bool isMoving = true; // Variable to track movement
    private KillCounter killCounter; // Reference to the KillCounter script
    private Collider enemyCollider; // Reference to the Collider component

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody>();

        // Finding the player GameObject using tag "Player"
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        // Checking if playerObject is found
        if (playerObject != null)
        {
            // Setting the target to the player's transform
            target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player not found with tag 'Player'.");
        }

        // Find and store a reference to the HealthBar script
        healthBar = FindObjectOfType<HealthBar>();

        // Find and store a reference to the KillCounter script
        killCounter = FindObjectOfType<KillCounter>();

        // Get the Collider component attached to the enemy
        enemyCollider = GetComponent<Collider>();

        if (enemyCollider == null)
        {
            Debug.LogWarning("Collider component not found on the enemy GameObject.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Checking if target is not null before chasing and character is allowed to move
        if (target != null && isMoving)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rig.MovePosition(pos);
            transform.LookAt(target);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            animator.SetBool("Kill", true);
            isMoving = false; // Stop moving after being hit by a bullet

            // Increment kill count when enemy is killed
            if (killCounter != null)
            {
                killCounter.IncrementKillCount();
            }

            // Decrease player's health
            

            // Disable the collider
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // Start coroutine to make the enemy disappear after 10 seconds of not moving
            StartCoroutine(DisappearAfterDelay(10f));
        }

        if (other.CompareTag("Player"))
        {   
            animator.SetBool("Attack", true);
            animator.SetBool("Run", false); // Stop running when colliding with the player
            if (healthBar != null)
            {
                healthBar.TakeDamage(1f); // Adjust damage as needed
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true); // Resume running when no longer colliding with the player
        }
    }

    IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Check if the enemy is still not moving
        if (!isMoving)
        {
            // Deactivate the enemy GameObject
            gameObject.SetActive(false);
        }
    }
}
