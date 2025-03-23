using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] GameObject mainMenuSettings, mainMenuCredits;
    public void resume()
    {
        GameManager.instance.stateUnpause();
    }


    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.stateUnpause();
    }

    public void loadLevel(int level)
  {
        SceneManager.LoadScene(level);
        GameManager.instance.stateUnpause();
  }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void SettingsMenu()
    {
      
        GameManager.instance.MainMenuSettings();
    }

    //public void MainMenuSettings()
    //{
    //    mainMenuSettings.SetActive(true);

    //}

    public void MainMenuCredits()
    {
        mainMenuCredits.SetActive(true);
    }
    public void BackButton()
    {

        GameManager.instance.pauseShow();
    }

    //public void MainMenuBackButton()
    //{
        
    //}
}