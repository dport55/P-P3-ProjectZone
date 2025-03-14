using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("---- Components ----")]
    [SerializeField] CharacterController Controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Light flashlight;
    private SpacePod currentSpacePod;

    [Header("---- Stats ----")]
    [Range(3, 20)][SerializeField] int speed = 6;
    [Range(2, 5)][SerializeField] int sprintMod = 2;
    [Range(5, 20)][SerializeField] float jumpSpeed = 10f;
    [Range(1, 3)][SerializeField] int jumpMax = 2;
    [Range(15, 45)][SerializeField] float gravity = 20f;
    [SerializeField] float crouchHeight = 1f;
    [SerializeField] float crouchSpeedMod = 0.5f;
    [SerializeField] float interactRange = 2f;

   

    private int originalSpeed;
    private bool isCrouching = false;
    private float originalHeight;
    private Vector3 originalCenter;
    private int jumpCount = 0;
    private Vector3 moveDir;
    private Vector3 playerVel;
    private bool isSprinting;
    private int collectedParts = 0;
    
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform playerModel;
    [SerializeField] float crouchCameraOffset = 0.5f;
    //[SerializeField] float crouchScale = 0.7f;


    [Header("Shooting Settings")]
    [SerializeField] float shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    float shootTimer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //store the players og speed
        originalSpeed = speed;

        //Store original height and center
        originalHeight = Controller.height;
        originalCenter = Controller.center;
    }

    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);

        movement();
        sprint();
        crouch();
        ToggleFlashlight();
        Interact();
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

        playerVel.y -= gravity * Time.deltaTime;
        Controller.Move(playerVel * Time.deltaTime);

        shootTimer += Time.deltaTime;
        if (Input.GetButton("Fire1")&& shootTimer >= shootRate){
            shoot();
        }
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
                //Reduce height
                Controller.height = crouchHeight;

                //Prevent ground clipping
                Controller.Move(Vector3.down * 0.1f);

                //Crouch speed
                speed = (int)(originalSpeed * crouchSpeedMod);

                isCrouching = true;

                //Move camera down
                if (playerCamera != null)
                {
                    playerCamera.localPosition -= new Vector3(0, crouchCameraOffset, 0);
                }
            }
            else
            {
                if (CanStandUp()) //Prevent standing if blocked
                {
                    Controller.height = originalHeight;
                    Controller.Move(Vector3.up * 0.1f); //Avoid getting stuck

                    //Restore original speed 
                    speed = (int)originalSpeed;

                    isCrouching = false;

                    //Move camera back up
                    if (playerCamera != null)
                    {
                        playerCamera.localPosition += new Vector3(0, crouchCameraOffset, 0);
                    }
                }
            }
        }
    }


    bool CanStandUp()
    {
        RaycastHit hit;
        float headClearance = originalHeight - crouchHeight; 

        if (Physics.SphereCast(transform.position, Controller.radius, Vector3.up, out hit, headClearance))
        {
            return false; //if Something is blocking the player from standing
        }
        return true;
    }

    void ToggleFlashlight()
    {
        if (Input.GetButtonDown("Flashlight")) 
        {
            if (flashlight != null)
            {
                flashlight.enabled = !flashlight.enabled; //Toggle the flashlight
            }
        }
    }

    void Interact()
    {
        if (Input.GetButtonDown("Interact"))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactRange, interactableLayer))
            {
                if (hit.collider.CompareTag("Parts"))
                {
                    CollectPart(hit.collider.gameObject);
                }
                else if (hit.collider.CompareTag("SpacePod"))
                {
                    InsertPart(hit.collider.GetComponent<SpacePod>());
                }
            }
        }
    }

    void CollectPart(GameObject part)
    {
        collectedParts++;
        Destroy(part);
        Debug.Log($"Parts collected: {collectedParts}");
    }

    void InsertPart(SpacePod pod)
    {
        if (pod == null) return;

        if (collectedParts > 0 && !pod.IsFixed())
        {
            pod.InsertPart();
            collectedParts--;
            Debug.Log($"Inserted a part. Remaining: {collectedParts}");
        }
    }
    void shoot()
    {
        shootTimer = 0;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponentInParent<IDamage>();
            if(dmg != null)
            {
                dmg.takeDamage(shootDamage);    
            }
        }
    }
}
