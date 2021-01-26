using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager tm;

    public int tasksInfected = 0;
    public int maxTasksInfected = 1;

    private int maxNumberOfTasksPerPlayer = 1;
    public int MaxNumberOfTasksPerPlayer
    {
        get { return maxNumberOfTasksPerPlayer; }
    }

    private List<Task> taskList;

    private void Awake()
    {
        // Singleton
        if (tm == null)
        {
            tm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        taskList = GetComponentsInChildren<Task>().ToList();
        int id = 1;
        foreach (Task task in taskList)
        {
            task.TaskID = id++;
        }
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
