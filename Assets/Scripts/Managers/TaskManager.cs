using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager tm;

    public int tasksInfected = 0;
    public int maxTasksInfected = 1;
    public int maxNumberOfTasksPerPlayer = 1;
    public int numberOfCompletedTasks = 0;
    public int maxNumberOfTasks = 0;

    private Task[] taskList;
    public Task[] TaskList
    {
        get { return taskList; }
    }

    private void Awake()
    {
        // Singleton
        if (tm == null)
        {
            tm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        taskList = GetComponentsInChildren<Task>();
        int id = 1;
        foreach (Task task in taskList)
        {
            task.TaskID = id++;
        }
        maxNumberOfTasks = maxNumberOfTasksPerPlayer * (PhotonNetwork.PlayerList.Length - 1);
    }

    public void CompleteTask()
    {
        numberOfCompletedTasks++;
    }

    public void AllTasksCompleted()
    {
        if (numberOfCompletedTasks == maxNumberOfTasks)
            GameManager.gm.NextStage();
    }

    public List<Task> RandomizeTasks()
    {
        List<Task> newTaskList = new List<Task>(taskList);

        while (newTaskList.Count > maxNumberOfTasksPerPlayer)
        {
            int randomNum = Random.Range(0, newTaskList.Count);
            newTaskList.RemoveAt(randomNum);
        }

        return newTaskList;
    }
}