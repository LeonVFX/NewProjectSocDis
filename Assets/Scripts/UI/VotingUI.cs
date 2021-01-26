using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VotingUI : MonoBehaviour
{
    private Timer timer;
    [SerializeField] private Text timerText;

    private void Start()
    {
        timer = FindObjectOfType<Timer>();
    }

    private void Update()
    {
        timerText.text = $"Time Remaining: {timer.minute.ToString("0")}:{timer.gameTime.ToString("0#")}";
    }
}