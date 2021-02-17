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

    [Header("Task Details")]
    public bool isComplete = false;
    public string taskName;
    public string taskDescription;
    public Item.ItemType taskRequiredItemType;
    public GameObject taskDestination;

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