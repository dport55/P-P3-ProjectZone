using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer instance;

    [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public int startMinutes = 1; 

    private float timeRemaining;
    public bool isRunning = true;

    void Awake()
    {
        instance = this;
        timeRemaining = startMinutes * 60; 
    }

    void Update()
    {
        if (isRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                GameManager.instance.youLose();
                Debug.Log("You lose");
            }
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay() //Timers Format 
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public float GetTimeRemaining()
    {
        return timeRemaining;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void StartTimer()
    {
        isRunning = true;
    }

}