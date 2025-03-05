using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---- Components ----")]
    [SerializeField] CharacterController Controller;
    [SerializeField] LayerMask ignoreLayer;

    [Header("---- Stats ----")]
    [Range(3, 20)][SerializeField] int speed = 6;
    [Range(2, 5)][SerializeField] int sprintMod = 2;
    [Range(5, 20)][SerializeField] float jumpSpeed = 10f;
    [Range(1, 3)][SerializeField] int jumpMax = 2;
    [Range(15, 45)][SerializeField] float gravity = 20f;
    [SerializeField]float crouchHeight = 1f;  // Height when crouched
    [SerializeField] float crouchSpeedMod = 0.5f; // Speed modifier when crouched

    private bool isCrouching = false;
    private float originalHeight;
    private Vector3 originalCenter;
    private int jumpCount = 0;
    private Vector3 moveDir;
    private Vector3 playerVel;
    private bool isSprinting;

    void Start()
    {
        if (Controller == null)
        {
            Debug.LogError("CharacterController not assigned in PlayerController.");
        }

        // Store original height and center
        originalHeight = Controller.height;
        originalCenter = Controller.center;
    }

    void Update()
    {
        movement();
        sprint();
        crouch();

    }

    void movement()
    {
        if (Controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = -1f;

            if (Input.GetButtonDown("Jump") && !isCrouching)
            {
                jump();
            }
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);
        Controller.Move(moveDir * speed * Time.deltaTime);

        // Apply gravity
        playerVel.y -= gravity * Time.deltaTime;
        Controller.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && !isCrouching)
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void jump()
    {
        if (jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (!isCrouching)
            {
                // Reduce character height and adjust the center
                Controller.height = crouchHeight;
                Controller.center = new Vector3(originalCenter.x, originalCenter.y - (originalHeight - crouchHeight) / 2, originalCenter.z);
                speed = (int)(speed * crouchSpeedMod); // Reduce speed
                isCrouching = true;
            }
            else
            {
                if (CanStandUp()) // Prevent standing up if obstructed
                {
                    Controller.height = originalHeight;
                    Controller.center = originalCenter;
                    speed = (int)(speed / crouchSpeedMod); // Restore speed
                    isCrouching = false;
                }
            }
        }
    }

    bool CanStandUp()
    {
        RaycastHit hit;
        float headClearance = originalHeight - crouchHeight; // Space needed to stand

        if (Physics.SphereCast(transform.position, Controller.radius, Vector3.up, out hit, headClearance))
        {
            return false; // Something is blocking the player from standing
        }
        return true;
    }

}
