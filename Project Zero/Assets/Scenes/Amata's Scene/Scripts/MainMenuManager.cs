using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject MainMenuPanel, CreditsPanel, BackButton;
    public static MainMenuManager instance;
    public GameObject[] thingsToClose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
        
      

        // Make cursor visible and unlocked so player can click Start
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuCreditsPanelShow()
    {
        foreach (GameObject thing in thingsToClose)
        {
            thing.SetActive(false);
        }
        BackButton.SetActive(true);
       
        CreditsPanel.SetActive(true);
    }

    public void MainMenuPanelShow()
    {
     
        CreditsPanel.SetActive(false);
        BackButton.SetActive(false);
    }

    public void MainMenuStart()
    {
        SceneManager.LoadScene(1);
    }
    public void MainMenuBack()
    {
        foreach (GameObject thing in thingsToClose)
        {
            thing.SetActive(true);
        }
        BackButton.SetActive(false);
        
        CreditsPanel.SetActive(false);
    }
}
