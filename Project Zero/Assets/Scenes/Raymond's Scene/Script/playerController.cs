using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] public float freezeTime;

    // Amata's Addition
    [SerializeField] GameObject O2WarningScreen1; // O2WarningScreen2;

    float shootTimer;

    [Header("=====Guns=====")]
    [SerializeField] List<Gunstats> gunList = new List<Gunstats>();
    [SerializeField] GameObject gunModel;
    [SerializeField] Transform Laser, RedSphere, BlueSphere;

    [Header("Audio Settings")]
    [SerializeField] AudioSource aud;
    [Range(0, 1)][SerializeField] AudioClip[] audSteps;
    [Range(0, 1)][SerializeField] float audStepsVol;
    [Range(0, 1)][SerializeField] AudioClip[] audHurt;
    [Range(0, 1)][SerializeField] float audHurtVol;
    [Range(0, 1)][SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;


    [Header("---- UI ----")]
    [SerializeField] private TextMeshProUGUI partsCounterTMP;
    [SerializeField] private TextMeshProUGUI remainingPartsTMP;

    private int requiredParts = 5;


    bool isPlayerSteps;

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

    [Header("---- Slide Settings ----")]
    [SerializeField] private float slideSpeed = 0f;  // Initial slide boost
    [SerializeField] private float slideDuration = 0f; // Time before slowing down
    [SerializeField] private float slideFriction = 0f;  // How fast the slide slows
    [SerializeField] private float slideCooldownTime = 2f;

    private bool canSlide = true;
    private bool isSliding = false;

    private int originalSpeed;
    private bool isCrouching = false;
    private float originalHeight;
    private Vector3 originalCenter;
    private int jumpCount = 0;
    private Vector3 moveDir;
    private Vector3 playerVel;
    private bool isSprinting;
    private int collectedParts;
    //Delvin's Additions
    public GameObject playerDamageScreen;
    public bool isHiding = false;
    private Transform hideSpotInside; // Position inside the hiding place
    private Transform hideSpotOutside; // Position outside the hiding place
    private bool canHide = false; // Player is near a hiding spot
                                  //End of Delvin's Additions
    [SerializeField] GameObject hidePrompt; // UI Prompt for hiding
    [SerializeField] GameObject exitPrompt;
    [SerializeField] GameObject Cam;// UI Prompt for camera
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

        //Hemants addition
        GameManager.instance.WeaponsDisplay.SetActive(false);
        GameManager.instance.RedDisplay.SetActive(false);
        GameManager.instance.BlueDisplay.SetActive(false);
        //End


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
   
        //Interact();
        slide();
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
        if (isHiding)
        {
            moveDir = Vector3.zero; // Prevent movement
            playerVel = Vector3.zero; // Prevent any velocity changes
            return;
        }

        if (Controller.isGrounded)
        {
            if (moveDir.magnitude > 0.3f && !isPlayerSteps)
            {
                StartCoroutine(playSteps());
            }

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
        if (gunList.Count != 0)
        {
            if (Input.GetButton("Fire1") && shootTimer >= shootRate)
            {
                shoot();
            }
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

    //Hemant's Addition
    IEnumerator playSteps()
    {
        isPlayerSteps = true;
        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

        if (!isSprinting)
        {
            yield return new WaitForSeconds(.5f);

        }
        else
            yield return new WaitForSeconds(0.3f);
        isPlayerSteps = false;
    }
    //End

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);

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



                //Crouch speed
                speed = (int)(originalSpeed * crouchSpeedMod);

                isCrouching = true;

            }
            else
            {
                Controller.height = 2f;

                //Restore original speed 
                speed = (int)originalSpeed;

                isCrouching = false;
                Debug.Log(originalHeight);


            }
        }
    }

    void slide()
    {
        if (Input.GetButtonDown("Slide") && isSprinting && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
    }

    IEnumerator SlideRoutine()
    {
        if (!canSlide)
            yield break;

        isSliding = true;
        isCrouching = true;
        canSlide = false; // Prevents sliding again until cooldown is over

        // Temporarily lower player height
        Controller.height = 1f;

        // Slide movement
        Vector3 slideDirection = transform.forward * slideSpeed;
        float slideTime = slideDuration;

        while (slideTime > 0f)
        {
            Controller.Move(slideDirection * Time.deltaTime);
            slideTime -= Time.deltaTime;
            yield return null;
        }

        // Reset states
        isSliding = false;

        if (!Input.GetButton("Crouch"))
        {
            Controller.height = 2f;
            isCrouching = false;
        }

        // Start cooldown before allowing another slide
        yield return new WaitForSeconds(slideCooldownTime);
        canSlide = true; // Re-enables sliding
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

    // Dylan's Edits

    //void Interact()
    //{
    //    if (Input.GetButtonDown("Interact"))
    //    {
    //        RaycastHit hit;
    //        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactRange, interactableLayer))
    //        {
    //            if (hit.collider.CompareTag("Parts"))
    //            {
    //                CollectPart(hit.collider.gameObject);
    //            }
    //            else if (hit.collider.CompareTag("SpacePod"))
    //            {
    //                InsertPart(hit.collider.GetComponent<SpacePod>());
    //            }
    //        }
    //    }
    //}

    public void getParts(GameObject parts)
    {

        if (parts.CompareTag("Parts"))
            collectedParts++;

       GameManager.instance.updateGameGoal(collectedParts);

        //Destroy(part);
        //Debug.Log($"Parts collected: {collectedParts}");
    }

    public void InsertPart(SpacePod pod)
    {
        if (pod == null) return;

        if (collectedParts > 0 && !pod.IsFixed())
        {
            pod.InsertPart();
            collectedParts--;
            //Debug.Log($"Inserted a part. Remaining: {collectedParts}");
        }
    }
    // End of Dylan's Edits

    // Hemant's Adittion

    void shoot()
    {
        shootTimer = 0;
        //if (gunList.Count > 0)
        //{
        //    gunList[gunListPos].AmmoCur--;
        //}
        aud.PlayOneShot(gunList[gunListPos].shootSound, gunList[gunListPos].shootVol);

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
                dmg.TakeDamage(shootDamage, freezeTime, 0);
            }

            // Instantiate the hit effect at the point of impact
            ParticleSystem hiteffect = Instantiate(gunList[gunListPos].HitEffect, hit.point, Quaternion.identity);
            Destroy(hiteffect.gameObject, 0.05F);

            // Instantiate the laser effect from muzzle to hit point
            GameObject laserBeam = Instantiate(gunList[gunListPos].ShootEffect, muzzlePos, Quaternion.identity);

            // Make the laser point toward the hit
            laserBeam.transform.LookAt(hit.point);
            float distance = Vector3.Distance(muzzlePos, hit.point);

            StartCoroutine(DisableMuzzleFlash(gunList[gunListPos].RedSphere));
            laserBeam.transform.localScale = new Vector3(1, 1, distance);

            // Destroy the laser after a short delay
            Destroy(laserBeam, 0.05f);

        }


    }

    public IEnumerator ShootEffect()
    {
        if (gunList[gunListPos] && gunList[gunListPos].shootSound != null)
        {
            aud.PlayOneShot(gunList[gunListPos].shootSound, gunList[gunListPos].shootVol);
        }
        yield return null;
    }

    // Coroutine to disable muzzle flash after 0.05 seconds
    IEnumerator DisableMuzzleFlash(bool _Sphere)
    {
        if (!_Sphere)
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
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);


        if (HP <= 0 || Oxygen <= 0)
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
        //gunListPos = gunList.Count - 1;
        ChangeGunDisplay();
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
            ChangeGunDisplay();

            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
        {
            gunListPos--;
            ChangeGunDisplay();

            changeGun();
        }
    }

    void ChangeGunDisplay()
    {
        if (gunList.Count > 0)
        {
            GameManager.instance.WeaponsDisplay.SetActive(true);

            if (!gunList[gunListPos].RedSphere)
            {
                GameManager.instance.RedDisplay.SetActive(false);
                GameManager.instance.BlueDisplay.SetActive(true);
            }
            else
            {
                GameManager.instance.RedDisplay.SetActive(true);
                GameManager.instance.BlueDisplay.SetActive(false);
            }
        }
        else
        {
            GameManager.instance.WeaponsDisplay.SetActive(false);
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
        //Amata's Addition
        if (other.CompareTag("LowO2"))
        {
            O2WarningScreen1.SetActive(true);
        }
        //End of Amata's Addition

        if (other.CompareTag("HidingSpot"))
        {
            GameManager.instance.retical.SetActive(false);
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot"); // Get outside position
            hidePrompt.SetActive(true);
        }

        else if (other.CompareTag("HidingSpot2"))
        {
            GameManager.instance.retical.SetActive(false);
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot2"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot2"); // Get outside position
            hidePrompt.SetActive(true);
        }
        else if (other.CompareTag("HidingSpot3"))
        {
            GameManager.instance.retical.SetActive(false);
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot3"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot3"); // Get outside position
            hidePrompt.SetActive(true);
        }
        else if (other.CompareTag("HidingSpot1"))
        {
            GameManager.instance.retical.SetActive(false);
            canHide = true;
            hideSpotInside = other.transform.Find("InsideSpot1"); // Get inside position
            hideSpotOutside = other.transform.Find("OutsideSpot1"); // Get outside position
            hidePrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {

        //Amata's Addition
        if (other.CompareTag("LowO2"))
        {
            O2WarningScreen1.SetActive(false);
        }
        //End of Amata's Addition

        if (other.CompareTag("HidingSpot"))
        {
            GameManager.instance.retical.SetActive(true);
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
        else if (other.CompareTag("HidingSpot1"))
        {
            GameManager.instance.retical.SetActive(true);
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
        else if (other.CompareTag("HidingSpot2"))
        {
            GameManager.instance.retical.SetActive(true);
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
        else if (other.CompareTag("HidingSpot3"))
        {
            GameManager.instance.retical.SetActive(true);
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
    }
    //End od Delvin's Additions

}
