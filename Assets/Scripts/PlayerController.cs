using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    public float rotationSpeed = 5f;
    private CharacterController controller;
    private PlayerInput playerInput;

    private Transform cameraTransform;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputAction moveAction;
    //private InputAction lookAction;
    private InputAction jumpAction;

    private void Start()
    {
        controller =GetComponent<CharacterController>();
        playerInput=GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction= playerInput.actions["Move"];
        jumpAction= playerInput.actions["Jump"];

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();

        Vector3 move = new Vector3(input.x, 0, input.y);

        move=move.x*cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y=0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //totates toward camera directions

        Quaternion targetRotation = Quaternion.Euler(0,cameraTransform.eulerAngles.y,0);
        transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation , rotationSpeed * Time.deltaTime);
    }
}