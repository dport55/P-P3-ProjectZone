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

    //End of Delvin's Changes
    public bool isPaused;


    
    //change

    //Hemant's Addition
    public GameObject WeaponsDisplay, RedDisplay, BlueDisplay;
    //End



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerController>();

    }


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
        //MainCamera.enabled = false; // Disable camera switcher
        WinCam.SetActive(true); // Enable the WinCam
        menuActive = menuWin;
        menuActive.SetActive(true);
  
      
        Explosion1.SetActive(true);
        Explosion2.SetActive(true);
        Explosion3.SetActive(true);
        Explosion4.SetActive(true);
        Explosion5.SetActive(true);
        Explosion6.SetActive(true);
StartCoroutine(DelayPauseAndCredits(3f));
       

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
    private IEnumerator DelayPause()
    {
        yield return new WaitForSeconds(7f); // Wait for specified time


        statePause(); // Show credits  statePause(); // Pause the game
    }

    public void ShowSettings()
    {
        statePause();
        menuPause.SetActive(false);
        menuActive = SettingsMenu;
        menuActive.SetActive(true);
    }
    //End of Delvin's Changes
    public void statePause()
    {
        playButtonShow();
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
    //End of Delvin's Changes
}