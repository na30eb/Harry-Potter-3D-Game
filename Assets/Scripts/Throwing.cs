using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{public Transform gunEnd; // Reference to the gun end object (muzzle location)
    public GameObject bulletPrefab; // Prefab for the bullet
    public float bulletSpeed = 20f; // Speed of the bullet
    public float fireRate = 0.25f; // Time between shots

    private Camera mainCamera; // Reference to the main camera
    private float nextFire; // Time until the next shot can be fired

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Raycast from the camera to the mouse position
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // Calculate the direction from gunEnd to the hit point
            Vector3 bulletDirection = (hit.point - gunEnd.position).normalized;

            // Instantiate the bullet prefab
            GameObject bullet = Instantiate(bulletPrefab, gunEnd.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            // Set the bullet's velocity
            bulletRb.velocity = bulletDirection * bulletSpeed;

            // Ignore collisions with the player (optional)
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }
}
