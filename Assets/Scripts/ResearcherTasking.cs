using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearcherTasking : MonoBehaviour
{
    private PhotonView playerView;
    private List<Task> taskList;
    private Task targetTask;
    private bool isValidTask;
    private bool isInteract = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();

        if (!playerView.IsMine)
            return;

        taskList = TaskManager.tm.RandomizeTasks();
        GetComponent<Player>().PHUD.OnInteraction += PressInteract;
        GetComponent<Player>().PHUD.UpdateTaskList(taskList);
    }

    void Update()
    {
        if (!playerView.IsMine)
            return;

        if (isValidTask)
        {
            if (Input.GetButtonDown("Interact") || isInteract)
                DoTask();
        }
    }

    #region Triggers
    private void OnTriggerEnter(Collider task)
    {
        if (task.tag == "Task")
        {
            if (!playerView.IsMine)
                return;

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
            if (!playerView.IsMine)
                return;

            targetTask = null;
            isValidTask = false;
        }
    }
    #endregion

    private void DoTask()
    {
        if (!playerView.IsMine)
            return;

        // If holding item
        if (GetComponent<Player>().HeldItem != null)
        {
            if (GetComponent<Player>().HeldItem.itemType == targetTask.TaskRequiredItemType)
            {
                if (targetTask.TaskInfected)
                {
                    GetComponent<Researcher>().SetInfected(true);
                    playerView.RPC("RPC_DisinfectTask", RpcTarget.All);
                    GetComponentInChildren<SpriteRenderer>().color = Color.green;
                }
                CompleteTask();
                Debug.Log("Task Completed!");
            }
            else
            {
                Debug.Log($"You need a { targetTask.TaskRequiredItemType }.");
            }
        }
    }

    private void CompleteTask()
    {
        if (!playerView.IsMine)
            return;

        isValidTask = false;
        playerView.RPC("RPC_GroupTaskComplete", RpcTarget.All);
        targetTask.isComplete = true;
        taskList.Remove(targetTask);
        GetComponent<Player>().PHUD.UpdateTaskList(taskList);
    }

    [PunRPC]
    private void RPC_GroupTaskComplete()
    {
        TaskManager.tm.CompleteTask();
    }

    [PunRPC]
    private void RPC_DisinfectTask()
    {
        targetTask.SetInfected(false);
        TaskManager.tm.tasksInfected--;
    }

    #region Base Interactions
    private void PressInteract(PhotonView playerView)
    {
        IEnumerator pressedInteract = InteractPressed();
        isInteract = true;
        StartCoroutine(pressedInteract);
    }

    private IEnumerator InteractPressed()
    {
        yield return new WaitForEndOfFrame();
        isInteract = false;
    }
    #endregion
}