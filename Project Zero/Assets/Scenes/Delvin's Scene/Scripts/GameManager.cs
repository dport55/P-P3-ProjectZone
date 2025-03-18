using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public GameObject menuActive, menuPause, menuWin, menuLose, menuTutorial, retical, PlayButton;
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
    public bool isPaused;
    

    int goalCount;
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
        if(SceneManager.GetActiveScene().name == "Tutorial")
        {
            PlayButton.SetActive(true);
        }
        else
        {
            PlayButton.SetActive(false);
        }
    }

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


}