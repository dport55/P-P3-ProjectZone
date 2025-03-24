using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject menuActive, menuPause, menuWin, menuLose, menuTutorial, retical, PlayButton, O2WarningScreen1, O2WarningScreen2, SettingsMenu;

    //Delvin's Changes
    public static GameManager instance;
    public GameObject player;
    public PlayerController playerScript;
    public BossEnemy bossEnemy;
    public CrawlerEnemy crawlerEnemy;
    public ScreamerEnemy screamerEnemy;
    public Camera MainCamera;
    public StaticEnemy staticEnemy;
    public GameObject WinCam;
    public Image playerHPBar;
    public Image playerO2Bar;
    public Animator creditsAnimator;

    [SerializeField] TMP_Text goalCountText;

    public GameObject Explosion1;
    public GameObject Explosion2;
    public GameObject Explosion3;
    public GameObject Explosion4;
    public GameObject Explosion5;
    public GameObject Explosion6;
    public GameObject Credits;
    public GameObject ship;
    public GameObject playerMarker;
    public GameObject[] displayClose;
    public GameObject PartsList;

    //End of Delvin's Changes
    public bool isPaused;



    //change

    //Hemant's Addition
    public GameObject WeaponsDisplay, RedDisplay, BlueDisplay;

    [Header("Audio Settings")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip background_aud;
    [Range(0, 1)][SerializeField] float aud_vol;
    bool GameON;

    //End



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();


        //Hemant's Addition
        PlayBackgroundMusic();
        //End
    }

    //Hemant's Addition
    void PlayBackgroundMusic()
    {
        if (aud != null && background_aud != null)
        {
            aud.clip = background_aud;
            aud.volume = aud_vol;
            aud.loop = true;  // Ensure it loops
            aud.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or AudioClip is missing!");
        }
    }
    //End

    private void Start()
    {
        //TutorialShow();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }
//Delvin's Changes
    public void ShowWinMenu()
    {
        foreach (GameObject close in displayClose)
        {
            close.SetActive(false);
        }
        retical.SetActive(false);
        playerMarker.SetActive(false);
        WinCam.SetActive(true); // Enable the WinCam
        menuActive = menuWin;
        menuActive.SetActive(true);
        StartCoroutine(ShipTakeoff());

       StartCoroutine(DelayPauseAndCredits(6f));
       

        if (creditsAnimator != null)
        {
            creditsAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        // Start coroutine to delay pause and credits
        
    StartCoroutine(DelayPause());// 3 seconds delay
    }

    private IEnumerator DelayPauseAndCredits(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for specified time

      
        Credits.SetActive(true); // Show credits  statePause(); // Pause the game
    }
    private IEnumerator ShipTakeoff()
    {
        ship.SetActive(true); // Activate ship, animation should play automatically
        yield return new WaitForSeconds(5f); // Wait for 5 seconds before triggering explosions

        // Activate explosions after delay
        Explosion1.SetActive(true);
        Explosion2.SetActive(true);
        Explosion3.SetActive(true);
        Explosion4.SetActive(true);
        Explosion5.SetActive(true);
        Explosion6.SetActive(true);

        StartCoroutine(CameraShake(WinCam, 0.5f, 0.3f)); // Shake camera when explosions start
    }

    // Camera Shake Effect
    private IEnumerator CameraShake(GameObject targetCam, float duration, float magnitude)
    {
        if (targetCam == null) yield break; // Prevent errors if WinCam is not assigned

        Transform camTransform = targetCam.transform;
        Vector3 originalPosition = camTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            camTransform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        camTransform.localPosition = originalPosition; // Reset camera position
    }

    private IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(11f); // Wait for specified time


        statePause(); // Show credits  statePause(); // Pause the game
    }

    public void ShowSettings()
    {
        statePause();
        menuPause.SetActive(false);
        menuActive = SettingsMenu;
        menuActive.SetActive(true);
        menuPause.SetActive(false);
    }
    //End of Delvin's Changes
    public void statePause()
    {
      
        isPaused = !isPaused;
        retical.SetActive(false);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void TutorialShow()
    {
        statePause();
        menuActive.SetActive(false);
        menuActive = menuTutorial;
        menuActive.SetActive(true);
    }

    public void pauseShow()
    {
        statePause();
        menuActive.SetActive(false);
        menuActive = menuPause;
        menuActive.SetActive(true);
        SettingsMenu.SetActive(false);
    }

    public void playButtonShow()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }
    }

    //public void showO2Warning()
    //{
    //    if (O2Count < 100)
    //    {
    //        PlayButton.SetActive(true);
    //    }
    //    else
    //    {
    //        PlayButton.SetActive(false);
    //    }
    //}



    //private IEnumerator OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("LowO2"))
    //    {
    //        O2WarningScreen1.SetActive(true);
    //        yield return new WaitForSeconds(2f);
    //        O2WarningScreen1.SetActive(false);

    //        O2WarningScreen2.SetActive(true);
    //        yield return new WaitForSeconds(2f);
    //        O2WarningScreen2.SetActive(false);
    //    }
    //    else
    //    {
    //        O2WarningScreen1.SetActive(false);
    //        O2WarningScreen2.SetActive(false);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("LowO2"))
    //    {
    //        StopAllCoroutines();
    //        O2WarningScreen1.SetActive(false);
    //        O2WarningScreen2.SetActive(false);
    //    }


    //}
    public void stateUnpause()
    {
        isPaused = !isPaused;
        retical.SetActive(true);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }
    //Delvin's Changes
    public void updateGameGoal(int parts)
    {
        goalCountText.text = parts.ToString("F0") + "/10";

    }

    public void MainMenuSettings()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SettingsMenu.SetActive(true);

    }

    public void ShowParts()
    {
        statePause();
        menuPause.SetActive(false);
        menuActive = PartsList;
        menuActive.SetActive(true);
        menuPause.SetActive(false);
        PartsList.SetActive(true);
    }
    //End of Delvin's Changes


}