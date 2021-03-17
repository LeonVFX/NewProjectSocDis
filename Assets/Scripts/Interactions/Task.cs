using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour
{
    public TaskObject taskObject;

    // Task Information
    private int taskID;

    public int TaskID
    {
        get { return taskID; }
        set { taskID = value; }
    }

    [Header("Task Details")]
    public bool isComplete = false;
    private string taskName;
    public string TaskName
    {
        get { return taskName; }
    }
    private string taskDescription;
    public string TaskDescription
    {
        get { return taskDescription; }
    }
    private Item.ItemType taskRequiredItemType;
    public Item.ItemType TaskRequiredItemType
    {
        get { return taskRequiredItemType; }
    }

    private bool taskinfected = false;
    public bool TaskInfected
    {
        get { return taskinfected; }
    }

    private void Start()
    {
        taskName = taskObject.taskName;
        taskDescription = taskObject.taskDescription;
        taskRequiredItemType = taskObject.taskRequiredItemType;
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