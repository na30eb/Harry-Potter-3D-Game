using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public float speed = 5f;
    public float stopDistance = 2f; // Minimum distance to stop from the player
    public float attackInterval = 1f; // Time interval between attacks
    Rigidbody rig;
    public HealthBar healthBar; // Reference to the HealthBar script

    bool isMoving = true; // Variable to track movement
    private KillCounter killCounter; // Reference to the KillCounter script
    private Collider enemyCollider; // Reference to the Collider component

    private bool isDead = false; // Variable to track if the enemy is dead
    private bool isAttacking = false; // Variable to track if the enemy is attacking

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
        if (target != null && isMoving && !isDead)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > stopDistance)
            {
                Vector3 pos = Vector3.MoveTowards(rig.position, target.position, speed * Time.fixedDeltaTime);
                rig.MovePosition(pos);
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                rig.MoveRotation(lookRotation);
                animator.SetBool("Run", true);
                animator.SetBool("Attack", false);
            }
            else
            {
                // Enemy is within the stop distance
                animator.SetBool("Run", false);
                animator.SetBool("Attack", true);

                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isDead)
        {
            isDead = true; // Mark the enemy as dead
            animator.SetBool("Kill", true);
            isMoving = false; // Stop moving after being hit by a bullet

            // Increment kill count when enemy is killed
            if (killCounter != null)
            {
                killCounter.IncrementKillCount();
            }

            // Disable the collider
            if (enemyCollider != null)
            {
                enemyCollider.enabled = false;
            }

            // Start coroutine to make the enemy disappear after 10 seconds of not moving
            StartCoroutine(DisappearAfterDelay(10f));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Attack", false);
            animator.SetBool("Run", true); // Resume running when no longer colliding with the player
            isMoving = true; // Allow movement again
            isAttacking = false; // Stop the attack coroutine
        }
    }

    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        while (isAttacking)
        {
            if (healthBar != null)
            {
                healthBar.TakeDamage(1f); // Adjust damage as needed
            }
            yield return new WaitForSeconds(attackInterval);
        }
    }

    private IEnumerator DisappearAfterDelay(float delay)
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
