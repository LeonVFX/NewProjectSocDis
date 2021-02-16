using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    // Task Information
    private int taskID;

    public int TaskID
    {
        get { return taskID; }
        set { taskID = value; }
    }

    [SerializeField] private string taskName;
    [SerializeField] private string taskDescription;
    [SerializeField] private Item taskRequiredItem;

    private bool taskinfected = false;
    public bool TaskInfected
    {
        get { return taskinfected; }
    }

    private void Update()
    {
        if (taskinfected)
            GetComponent<Renderer>().material.color = Color.green;
        else
            GetComponent<Renderer>().material.color = Color.white;
    }

    public void SetInfected(bool isInfected)
    {
        taskinfected = isInfected;
    }
}