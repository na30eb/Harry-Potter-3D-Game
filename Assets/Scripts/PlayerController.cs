using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController),typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
     [SerializeField]
     private Transform barrelTransform;
     [SerializeField]
     private Transform bulletparent;
     [SerializeField]
     private float bulletHitMissDistance = 25f;

    public float rotationSpeed = 5f;
    private CharacterController controller;
    private PlayerInput playerInput;

    private Transform cameraTransform;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputAction moveAction;
    //private InputAction lookAction;
    private InputAction jumpAction;


    private InputAction shootAction;
    [SerializeField]
    private GameObject bulletPrefab;
    private void Awake()
    {
        controller =GetComponent<CharacterController>();
        playerInput=GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction= playerInput.actions["Move"];
        jumpAction= playerInput.actions["Jump"];
        shootAction=playerInput.actions["Shoot"];
    }
    private void OnEnable() {
        shootAction.performed += _ => shootGun();
    }
    private void OnDisable() {
        shootAction.performed -= _ => shootGun();

    }
    private void shootGun(){
        RaycastHit Hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab,barrelTransform.position,Quaternion.identity,bulletparent);
        BulletController bulletController=bullet.GetComponent<BulletController>();
        if(Physics.Raycast(cameraTransform.position,cameraTransform.forward,out Hit , Mathf.Infinity)){
            bulletController.target=Hit.point;
            bulletController.hit=true;
        }else{

            bulletController.target=cameraTransform.position+cameraTransform.forward*bulletHitMissDistance;
            bulletController.hit=true;
        }
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