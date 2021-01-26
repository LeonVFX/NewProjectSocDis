using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    private int taskID;

    public int TaskID
    {
        get { return taskID; }
        set { taskID = value; }
    }

    private bool taskinfected = false;
    public bool TaskInfected
    {
        get { return taskinfected; }
    }

    private void Update()
    {
        if (taskinfected)
            GetComponent<SpriteRenderer>().color = Color.green;
        else
            GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void SetInfected(bool isInfected)
    {
        taskinfected = isInfected;
    }
}