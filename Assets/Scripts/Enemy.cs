using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
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
        // Checking if target is not null before chasing
        if (target != null)
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
            Destroy(gameObject);
        }
    }
}
