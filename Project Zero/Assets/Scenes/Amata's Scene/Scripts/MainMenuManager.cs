using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] public GameObject menuActive, menuPause, menuWin, menuLose, menuTutorial, retical, PlayButton;
    public static MainMenuManager instance;
    public bool isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
