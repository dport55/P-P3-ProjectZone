using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IDamage, IPickup
{
    [Header("---- Components ----")]
    [SerializeField] CharacterController Controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] Light flashlight;
    private SpacePod currentSpacePod;

    // Hemant's Adittion
    [Header("---- Shooting Settings ----")]
    [SerializeField] float shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] float freezeTime;

    float shootTimer;

    [Header("=====Guns=====")]
    [SerializeField] List<Gunstats> gunList = new List<Gunstats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] Transform Laser, RedSphere,BlueSphere;

    int gunListPos;
    //End

    [Header("---- Stats ----")]
   public float HP = 6;
    
    [SerializeField] float Oxygen;
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
    //Delvin's Additions
    public GameObject playerDamageScreen;
    public bool isHiding = false;
    private Transform hideSpotInside; // Position inside the hiding place
    private Transform hideSpotOutside; // Position outside the hiding place
    private bool canHide = false; // Player is near a hiding spot
                                  //End of Delvin's Additions
    [SerializeField] GameObject hidePrompt; // UI Prompt for hiding
    [SerializeField] GameObject exitPrompt;
    [SerializeField] GameObject Cam;// UI Prompt for exiting
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform playerModel;
    [SerializeField] float crouchCameraOffset = 0.5f;
    //[SerializeField] float crouchScale = 0.7f;

    float HPOrig;
    float O2Orig;

    

    void Start()
    {
        HPOrig = HP;
        O2Orig = Oxygen;
        //store the players og speed
        originalSpeed = speed;
        UpdatePlayerUI();
        //Store original height and center
        originalHeight = Controller.height;
        originalCenter = Controller.center;
        RedSphere.gameObject.SetActive(false);
        BlueSphere.gameObject.SetActive(false);
        isHiding = false;
   
        hidePrompt.SetActive(false);
        exitPrompt.SetActive(false);

    }

    void Update()
    {
        //// Hemant's Adittion
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.yellow);
        ////End

        movement();
        sprint();
        crouch();
        ToggleFlashlight();
        Interact();

        if (canHide && !isHiding && Input.GetKeyDown(KeyCode.E))
        {
            EnterHidingSpot();
          
        }
        else if (isHiding && Input.GetKeyDown(KeyCode.E))
        {
            ExitHidingSpot();
        }
    }

    void movement()
    {
       
        if (Controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = -1f;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                  (Input.GetAxis("Vertical") * transform.forward);
        Controller.Move(moveDir * speed * Time.deltaTime);

        jump();
        Controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
        // Hemant's Adittion

        shootTimer += Time.deltaTime;
        if (Input.GetButton("Fire1")&& shootTimer >= shootRate){
            shoot();
        }
        SelectGun();
        //End
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
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
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
        //Debug.Log($"Parts collected: {collectedParts}");
    }

    void InsertPart(SpacePod pod)
    {
        if (pod == null) return;

        if (collectedParts > 0 && !pod.IsFixed())
        {
            pod.InsertPart();
            collectedParts--;
            //Debug.Log($"Inserted a part. Remaining: {collectedParts}");
        }
    }
    // Hemant's Adittion

    void shoot()
    {
        shootTimer = 0;
        if (gunList.Count > 0)
        {
            gunList[gunListPos].AmmoCur--;
        }

        // Start coroutine to turn off muzzle flash after a short delay
       
        //// Activate the muzzle flash and randomize rotation
        //Laser.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        //Laser.gameObject.SetActive(true);

        // Use the muzzle flash’s world position directly
        Vector3 muzzlePos = Laser.position;

        RaycastHit hit;

        // Raycast from camera to detect hit point
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            // Apply damage if the hit object has an IDamage component
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.TakeDamage(shootDamage,freezeTime,0);
            }

            // Instantiate the hit effect at the point of impact
            ParticleSystem hiteffect = Instantiate(gunList[gunListPos].HitEffect, hit.point, Quaternion.identity);
            Destroy(hiteffect.gameObject,0.05F);

            // Instantiate the laser effect from muzzle to hit point
            GameObject laserBeam = Instantiate(gunList[gunListPos].ShootEffect, muzzlePos, Quaternion.identity);
            
            // Make the laser point toward the hit
            laserBeam.transform.LookAt(hit.point);
            float distance = Vector3.Distance(muzzlePos, hit.point);
           
                StartCoroutine(DisableMuzzleFlash(gunList[gunListPos].RedSphere));
            laserBeam.transform.localScale = new Vector3(1, 1, distance);

            // Destroy the laser after a short delay
            Destroy(laserBeam,0.05f);
            
        }

       
    }

    // Coroutine to disable muzzle flash after 0.05 seconds
    IEnumerator DisableMuzzleFlash(bool _Sphere)
    {
        if(!_Sphere)
        {
            BlueSphere.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            BlueSphere.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            BlueSphere.gameObject.SetActive(false);
        }
        if (_Sphere)
        {
            RedSphere.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            RedSphere.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.05f);
            RedSphere.gameObject.SetActive(false);
        }
        //Laser.gameObject.SetActive(false);
        
    }
    public void TakeDamage(float amount, float Freeze, float O2)
    {
        HP -= amount;
        Oxygen -= O2;
        StartCoroutine(flashDamageScreen());
        UpdatePlayerUI();
        //aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);


        if (HP <= 0)
        {
            GameManager.instance.youLose();

        }
    }

    void UpdatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        GameManager.instance.playerO2Bar.fillAmount = (float)Oxygen / O2Orig;
    }

    public void getgunstats(Gunstats gun)
    {

        gunList.Add(gun);
        changeGun();
    }

    void changeGun()
    {
        shootDamage = gunList[gunListPos].shootDamage;
        shootDist = gunList[gunListPos].shootDist;
        shootRate = gunList[gunListPos].shootRate;
        freezeTime = gunList[gunListPos].freezeTime;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
        //if(gunModel.name == "Freeze Gun")
        //{
        //    gunModel.transform.rotation = new Quaternion(0f, 270f, 5f,0f);
        //}

        //gunModel.transform.position = gunList[gunListPos].position;
        //gunModel.transform.rotation = gunList[gunListPos].rotation;
    }


    void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
        {
            gunListPos++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            changeGun();
        }
    }

    //End

    //Delvin's Additions

    IEnumerator flashDamageScreen()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }

    void EnterHidingSpot()
    {
        transform.position = hideSpotInside.position; // Move player inside
        isHiding = true;
        hidePrompt.SetActive(false);
        exitPrompt.SetActive(true);
        Cam.SetActive(true);
    }

    void ExitHidingSpot()
    {
        if (hideSpotOutside != null)
        {
            Controller.enabled = false; // Disable CharacterController
            transform.position = hideSpotOutside.position; // Move player outside
            Controller.enabled = true; // Re-enable CharacterController
        }

        isHiding = false;
        exitPrompt.SetActive(false);
        Cam.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot"); // Get outside position
            hidePrompt.SetActive(true);
        }

       else if (other.CompareTag("HidingSpot2"))
        {
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpo2t"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot2"); // Get outside position
            hidePrompt.SetActive(true);
        }
        else if (other.CompareTag("HidingSpot3"))
        {
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot3"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot3"); // Get outside position
            hidePrompt.SetActive(true);
        }
       else if (other.CompareTag("HidingSpot1"))
        {
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot1"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot1"); // Get outside position
            hidePrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
        else if (other.CompareTag("HidingSpot1"))
        {
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
       else if (other.CompareTag("HidingSpot2"))
        {
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
        else if (other.CompareTag("HidingSpot3"))
        {
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
    }
    //End od Delvin's Additions
}
