using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    public float speed = 5f;
    Rigidbody rig;

    bool isMoving = true; // Variable to track movement

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

            // Start coroutine to make the enemy disappear after 30 seconds of not moving
            StartCoroutine(DisappearAfterDelay(30f));
        }
    }

    IEnumerator DisappearAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Check if the enemy is still not moving
        if (!isMoving)
        {
            gameObject.SetActive(false); // Deactivate the enemy GameObject
        }
    }
}
