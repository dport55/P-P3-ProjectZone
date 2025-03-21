using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject menuActive, menuPause, menuWin, menuLose, menuTutorial, retical, PlayButton, O2WarningScreen1, O2WarningScreen2;
    public static GameManager instance;
    public GameObject player;
    public PlayerController playerScript;
    public BossEnemy bossEnemy;
    public CrawlerEnemy crawlerEnemy;
    public ScreamerEnemy screamerEnemy;
    //public CameraSwitcher cameraSwitcher;
    public StaticEnemy staticEnemy;
    public Image playerHPBar;
    public Image playerO2Bar;
    [SerializeField] TMP_Text goalCountText;
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

    public void ShowWinMenu()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

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

    public void updateGameGoal(int parts)
    {
        
        goalCountText.text = parts.ToString("F0") + "/10";

        if (parts == 10)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }
}