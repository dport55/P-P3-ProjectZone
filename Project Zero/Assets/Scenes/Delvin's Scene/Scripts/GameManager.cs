using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    [SerializeField] GameObject menuActive, menuPause, menuWin, menuLose, menuTutorial, MovePrompt;
    public Image playerHPBar;
    public bool isPaused;
    public GameObject player;
    public PlayerController playerScript;

    int HPOrig;
    int goalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
   
    }

    private void Start()
    {
        moveShow();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
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
        isPaused = !isPaused;
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

    public void moveShow()
    {
        menuActive = MovePrompt;
        menuActive.SetActive(true);
        if (Input.GetButtonDown("Horizontal") || (Input.GetButtonDown("Vertical")))
        {
            menuActive.SetActive(false);
        }
    }

    public void pauseShow()
    {
        statePause();
        menuActive.SetActive(false);
        menuActive = menuPause;
        menuActive.SetActive(true);
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
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
