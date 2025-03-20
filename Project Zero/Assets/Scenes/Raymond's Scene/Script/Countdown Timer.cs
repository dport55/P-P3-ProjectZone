using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer instance;

    [SerializeField] public TextMeshProUGUI timeText;
    [SerializeField] public int startMinutes = 1; 
    [SerializeField] public GameManager gameManager;

    private float timeRemaining;
    public bool isRunning = true;

    void Awake()
    {
        instance = this;
        timeRemaining = startMinutes * 60; // Convert minutes to seconds
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

    //public void TimerEnded() //Lose Condition once timer hits 0
    //{
    //    //Debug.Log("Countdown finished!");
    //    if (gameManager != null)
    //    {
    //        gameManager.youLose();
    //    }
    //}
}