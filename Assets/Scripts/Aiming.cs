using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cinemachine;

public class Aiming : MonoBehaviour
{
    Vector3 pos;
    public float speed=1f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
         Vector3 mousePos = Input.mousePosition;
        
        // Set a distance from the camera to move the sphere
        mousePos.z = 10f; // You can adjust this distance as needed
        
        // Convert the screen position to world position
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        
        // Set the position of the sphere to the calculated world position
        transform.position = worldPos;;
    }
}
