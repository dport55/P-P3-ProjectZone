using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject MainMenuPanel, CreditsPanel;
    public static MainMenuManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenuCreditsPanelShow()
    {
        MainMenuPanel.SetActive(false);
        CreditsPanel.SetActive(true);
    }

    public void MainMenuPanelShow()
    {
        MainMenuPanel.SetActive(true);
        CreditsPanel.SetActive(false);
    }

    public void MainMenuStart()
    {
        SceneManager.LoadScene(1);
    }
}
