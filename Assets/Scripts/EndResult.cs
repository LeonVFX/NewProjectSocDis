using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndResult : MonoBehaviour
{
    private string resultString = "";
    public string ResultString
    {
        get { return resultString; }
        set { resultString = value; }
    }
    [SerializeField] private Text resultText;


    private void Start()
    {
        GameManager.gm.OnEnd += DisplayResult;
        resultText.gameObject.SetActive(false);
        DontDestroyOnLoad(this);
    }

    private void DisplayResult()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = resultString;
    }
}