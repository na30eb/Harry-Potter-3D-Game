using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 4.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public float rotationSpeed = 5f;

    [SerializeField] private Transform barrelTransform;
    [SerializeField] private Transform bulletparent;
    [SerializeField] private float bulletHitMissDistance = 25f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip soundJump;
    [SerializeField] private AudioClip soundShoot;
    [SerializeField] private AudioClip soundWalk;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Transform cameraTransform;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private AudioSource audioPlayer;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        audioPlayer = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        shootAction.performed += _ => ShootGun();
    }

    private void OnDisable()
    {
        shootAction.performed -= _ => ShootGun();
    }

    private void ShootGun()
    {
        // Play shoot sound
        if (soundShoot != null)
            audioPlayer.PlayOneShot(soundShoot);

        RaycastHit hit;
        GameObject bullet = GameObject.Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, bulletparent);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
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

        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        // Play walk sound
        if (soundWalk != null && move.magnitude > 0 && groundedPlayer && !audioPlayer.isPlaying)
            audioPlayer.PlayOneShot(soundWalk);

        // Changes the height position of the player..
        if (jumpAction.triggered && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            // Play jump sound
            if (soundJump != null)
                audioPlayer.PlayOneShot(soundJump);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        // rotates toward camera directions
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
