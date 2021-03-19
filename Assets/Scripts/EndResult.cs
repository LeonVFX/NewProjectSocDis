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
    [SerializeField] private GameObject resultPrefab;


    private void Start()
    {
        GameManager.gm.OnEnd += DisplayResult;
        
        DontDestroyOnLoad(this);
    }

    private void DisplayResult()
    {
        IEnumerator wait = WaitForDisplayResult();
        StartCoroutine(wait);
    }

    private IEnumerator WaitForDisplayResult()
    {
        yield return new WaitForSeconds(1);
        GameObject resultScreen = Instantiate(resultPrefab, GameObject.Find("Canvas").transform);
        resultScreen.GetComponent<Text>().text = resultString;
    }
}