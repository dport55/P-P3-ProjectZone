//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerController2 : MonoBehaviour, IDamage
//{
//    [Header("-----Components-----")]
//    [SerializeField] CharacterController controller;
//    [SerializeField] LayerMask ignoreLayer;
//    [SerializeField] AudioSource aud;

//    [Header("-----Stats-----")]
//    [Range(1, 10)] public float HP;
//    [Range(3, 10)][SerializeField] int speed;
//    [Range(2, 5)][SerializeField] int sprintMod;
//    [Range(5, 20)][SerializeField] int jumpSpeed;
//    [Range(1, 3)][SerializeField] int jumpMax;
//    [Range(15, 45)][SerializeField] int gravity;
//    public GameObject playerDamageScreen;
//    public bool isHiding = false;
//    //[Header("-----Weapons-----")]
//    //[SerializeField] List<GunStats> gunList = new List<GunStats>();
//    //[SerializeField] GameObject gunModel;
//    //[SerializeField] Transform muzzleFlash;
//    //int shootDamage;
//    //float shootRate;
//    //int shootDistance;

//    //[Header("-----Audio-----")]
//    //[Range(0, 1)][SerializeField] AudioClip[] audSteps;
//    //[Range(0, 1)][SerializeField] float audStepsVol;
//    //[Range(0, 1)][SerializeField] AudioClip[] audHurt;
//    //[Range(0, 1)][SerializeField] float audHurtVol;
//    //[Range(0, 1)][SerializeField] AudioClip[] audJump;
//    //[Range(0, 1)][SerializeField] float audJumpVol;


//    int jumpCount;
//    float HPOrig;
//    int gunListPos;


//    float shootTimer;

//    Vector3 movedDir;
//    Vector3 playerVel;

//    bool isSprinting;
//    bool isPlayerSteps;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        HPOrig = HP;

//        //spawnPlayer();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

//        //if (!GameManager.instance.isPaused)
//        //{
//            Movement();
//        //    Sprint();
//        //}


//    }


//    void Movement()
//    {
//        if (controller.isGrounded)
//        {
         
//            jumpCount = 0;
//            playerVel = Vector3.zero;
//        }
//        movedDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//        transform.position += movedDir * speed * Time.deltaTime;
//        movedDir = (Input.GetAxis("Horizontal") * transform.right) +
//            (Input.GetAxis("Vertical") * transform.forward); ;
//        controller.Move(movedDir * speed * Time.deltaTime);

//        Jump();

//        controller.Move(playerVel * Time.deltaTime);
//        playerVel.y -= gravity * Time.deltaTime;

//        shootTimer += Time.deltaTime;

//        //if (Input.GetButtonDown("Fire1") && gunList.Count > 0 && gunList[gunListPos].ammoCurrent > 0 && shootTimer >= shootRate)
//        //{
//        //    Shoot();
//        ////}

//        //SelectGun();
//        //GunReload();

//    }

//    void Sprint()
//    {
//        if (Input.GetButtonDown("Sprint"))
//        {
//            speed += sprintMod;
//            isPlayerSteps = true;
//        }
//        else if (Input.GetButtonUp("Sprint"))
//        {
//            speed /= sprintMod;
//            isPlayerSteps = false;
//        }
//    }


//    //IEnumerator playSteps()
//    //{
//    //    isPlayerSteps = true;
//    //    aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], audStepsVol);

//    //    if (!isSprinting)
//    //    {
//    //        yield return new WaitForSeconds(.5f);

//    //    }
//    //    else
//    //        yield return new WaitForSeconds(0.5f);
//    //    isPlayerSteps = false;
//    //}

//    void Jump()
//    {
//        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
//        {
//            jumpCount++;
//            playerVel.y = jumpSpeed;
//            //aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);

//        }
//    }

//    //void Shoot()
//    //{
//    //    shootTimer = 0;
//    //    gunList[gunListPos].ammoCurrent--;
//    //    aud.PlayOneShot(gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Length)], gunList[gunListPos].shootVol);

//    //    StartCoroutine(flashMuzzle());

//    //    RaycastHit hit;
//    //    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
//    //    {
//    //        //Debug.Log(hit.collider.name);

//    //        Instantiate(gunList[gunListPos].hitEffect, hit.point, Quaternion.identity);

//    //        IDamage dmg = hit.collider.GetComponent<IDamage>();

//    //        if (dmg != null)
//    //        {
//    //            dmg.TakeDamage(shootDamage);
//    //        }
//    //    }
//    //}

//    public void TakeDamage(float amount)
//    {
//        HP -= amount;
//        StartCoroutine(flashDamageScreen());
//        //UpdatePlayerUI();
//        //aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);


//        //if (HP <= 0)
//        //{
//        //    GameManager.instance.youLose();

//        //}
//    }
//    IEnumerator flashDamageScreen()
//    {
//        playerDamageScreen.SetActive(true);
//        yield return new WaitForSeconds(0.1f);
//        playerDamageScreen.SetActive(false);
//    }

//    //void UpdatePlayerUI()
//    //{
//    //    GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
//    //}

//    //public void GetGunStats(GunStats gun)
//    //{
//    //    gunList.Add(gun);
//    //    gunListPos = gunList.Count - 1;
//    //    ChangeGun();

//    //}

//    //void SelectGun()
//    //{
//    //    if (Input.GetAxis("Mouse ScrollWheel") > 0 && gunListPos < gunList.Count - 1)
//    //    {
//    //        gunListPos++;
//    //        ChangeGun();

//    //    }
//    //    else if (Input.GetAxis("Mouse ScrollWheel") < 0 && gunListPos > 0)
//    //    {
//    //        gunListPos--;
//    //        ChangeGun();

//    //    }

//    //}

//    //void ChangeGun()
//    //{
//    //    shootDamage = gunList[gunListPos].shootDamage;
//    //    shootRate = gunList[gunListPos].shootRate;
//    //    shootDistance = gunList[gunListPos].shootDistance;

//    //    gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
//    //    gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
//    //}

//    //void GunReload()
//    //{

//    //    if (Input.GetButtonDown("Reload"))
//    //    {
//    //        gunList[gunListPos].ammoCurrent = gunList[gunListPos].ammoMax;
//    //    }
//    //}

//    //public void spawnPlayer()
//    //{

//    //    //controller.enabled = false;
//    //    controller.transform.position = GameManager.instance.playerSpawnPos.transform.position;
//    //    //controller.enabled = true;

//    //    HP = HPOrig;
//    //    UpdatePlayerUI();
//    //}

//    //IEnumerator flashMuzzle()
//    //{
//    //    muzzleFlash.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
//    //    muzzleFlash.gameObject.SetActive(true);
//    //    yield return new WaitForSeconds(0.05f);
//    //    muzzleFlash.gameObject.SetActive(false);
//    //}
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI prompts

public class PlayerController2 : MonoBehaviour, IDamage
{
    [Header("-----Components-----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    [SerializeField] AudioSource aud;
    [SerializeField] GameObject hidePrompt; // UI Prompt for hiding
    [SerializeField] GameObject exitPrompt;
    [SerializeField] GameObject Camera;// UI Prompt for exiting

    [Header("-----Stats-----")]
    [Range(1, 10)] public float HP;
    [Range(3, 10)][SerializeField] int speed;
    [Range(2, 5)][SerializeField] int sprintMod;
    [Range(5, 20)][SerializeField] int jumpSpeed;
    [Range(1, 3)][SerializeField] int jumpMax;
    [Range(15, 45)][SerializeField] int gravity;
    [SerializeField] int jumpCount;
     public GameObject playerDamageScreen;

    public bool isHiding = false;

    private Transform hideSpotInside; // Position inside the hiding place
    private Transform hideSpotOutside; // Position outside the hiding place
    private bool canHide = false; // Player is near a hiding spot

    float HPOrig;
    Vector3 movedDir;
    Vector3 playerVel;

    void Start()
    {
        isHiding = false;
        HPOrig = HP;
        hidePrompt.SetActive(false);
        exitPrompt.SetActive(false);
    }

    void Update()
    {
        Movement();

        if (canHide && !isHiding && Input.GetKeyDown(KeyCode.E))
        {
            EnterHidingSpot();
        }
        else if (isHiding && Input.GetKeyDown(KeyCode.E))
        {
            ExitHidingSpot();
        }
    }

    void Movement()
    {
        if (isHiding) return; // Prevent movement while hiding

        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVel.y = 0f;
        }

        // Correct movement calculation
        Vector3 move = (Input.GetAxis("Horizontal") * transform.right) +
                       (Input.GetAxis("Vertical") * transform.forward);

        controller.Move(move * speed * Time.deltaTime); // Apply movement

        Jump(); // Handle jumping

        playerVel.y -= gravity * Time.deltaTime; // Apply gravity
        controller.Move(playerVel * Time.deltaTime); // Apply gravity effect
    }
    public void TakeDamage(float amount)
    {
        HP -= amount;
        StartCoroutine(flashDamageScreen());
        //UpdatePlayerUI();
        //aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], audHurtVol);


        //if (HP <= 0)
        //{
        //    GameManager.instance.youLose();

        //}
    }
    IEnumerator flashDamageScreen()
    {
        playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerDamageScreen.SetActive(false);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void EnterHidingSpot()
    {
        transform.position = hideSpotInside.position; // Move player inside
        isHiding = true;
        hidePrompt.SetActive(false);
        exitPrompt.SetActive(true);
        Camera.SetActive(true);
    }

    void ExitHidingSpot()
    {
        if (hideSpotOutside != null)
        {
            controller.enabled = false; // Disable CharacterController
            transform.position = hideSpotOutside.position; // Move player outside
            controller.enabled = true; // Re-enable CharacterController
        }

        isHiding = false;
        exitPrompt.SetActive(false);
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
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HidingSpot"))
        {
            canHide = false;
            hidePrompt.SetActive(false);
            exitPrompt.SetActive(false);
        }
    }
}