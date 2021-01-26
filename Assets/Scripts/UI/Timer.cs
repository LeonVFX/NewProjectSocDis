using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float stage1TotalTime;
    [SerializeField] float voteStageTotalTime;
    [SerializeField] float stage2TotalTime;

    // Will be used for seconds
    public float gameTime = 0f;

    public float minute;
    public bool timeOver;

    // When game starts the time is set
    private void Start()
    {
        gameTime = 0.0f;
        minute = stage1TotalTime;
        timeOver = false;

        GameManager.gm.OnVoteStage += NextStage;
        GameManager.gm.OnStage2 += NextStage;
    }

    // Update is called once per frame
    void Update()
    {
        // CurrentTime will be running in seconds, will attempt to change to minutes and seconds
        gameTime -= 1 * Time.deltaTime;

        if (gameTime <= 0)
        {
            minute = minute - 1;
            if (minute < 0)
                timeOver = true;
            
            gameTime = 59f;
        }
    }

    private void NextStage()
    {
        // switch according to GameManager Stage turn
        switch (GameManager.gm.currentStage)
        {
            case GameManager.GameStage.Stage1:
                break;
            case GameManager.GameStage.Voting:
                minute = voteStageTotalTime;
                gameTime = 0.0f;
                timeOver = false;
                break;
            case GameManager.GameStage.Stage2:
                minute = stage2TotalTime;
                gameTime = 0.0f;
                timeOver = false;
                break;
            default:
                break;
        }
    }

    // Single Sapling Games, 2018.[Computer Program] Countdown Timer In Unity - Easy Beginners Tutorial/ Guide. Single Sapling Games 1st July 2018. Available from https://www.youtube.com/watch?v=o0j7PdU88a4

    //  float startingTime2 = 0f;
    //  float gameTime = 0f;
    //  float minutes = 5f;


}
