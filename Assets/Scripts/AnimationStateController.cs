using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust the speed of the character movement
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator=GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
if (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) {
    animator.SetBool("isWalking", true);
} else {
    animator.SetBool("isWalking", false);
}

// animator.SetFloat("moveX", Input.GetAxis("Horizontal"));
// animator.SetFloat("moveY", Input.GetAxis("Vertical"));


        if(Input.GetKey("space")){
            // Debug.Log("space is pressedd");
            animator.SetBool("jumping" , true);
        }else{
            animator.SetBool("jumping" , false);
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            // Debug.Log("ctrl is pressedd");
            animator.SetBool("isShooting" , true);
        }else{
            animator.SetBool("isShooting" , false);
        }

        
    }
}
