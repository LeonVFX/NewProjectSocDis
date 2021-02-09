using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearcherTasking : MonoBehaviour
{
    private PhotonView playerView;

    // private PhotonView playerView;
    private List<Task> taskList;
    private Task targetTask;
    private bool isValidTask;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        taskList = TaskManager.tm.RandomizeTasks();
    }

    void Update()
    {
        if (!playerView.IsMine)
            return;

        if (isValidTask)
        {
            if (Input.GetButtonDown("Interact"))
                DoTask();
        }
    }

    private void OnTriggerEnter(Collider task)
    {
        if (task.tag == "Task")
        {
            targetTask = task.GetComponent<Task>();
            foreach (Task checkTask in taskList)
            {
                if (targetTask.TaskID == checkTask.TaskID)
                {
                    isValidTask = true;
                    break;
                }
                else
                    Debug.Log("Not My Task!");
            }
        }
    }

    private void OnTriggerExit(Collider task)
    {
        if (task.tag == "Task")
        {
            targetTask = null;
            isValidTask = false;
        }
    }

    private void DoTask()
    {
        // Do the task. Implement Drew's Task Button
        if (targetTask.TaskInfected)
        {
            GetComponent<Researcher>().SetInfected(true);
            playerView.RPC("RPC_DisinfectTask", RpcTarget.All);
            // TODO: Debug Color
            GetComponentInChildren<SpriteRenderer>().color = Color.green;
        }
        Debug.Log("Task Completed!");
    }

    [PunRPC]
    private void RPC_DisinfectTask()
    {
        targetTask.SetInfected(false);
        TaskManager.tm.tasksInfected--;
    }
}