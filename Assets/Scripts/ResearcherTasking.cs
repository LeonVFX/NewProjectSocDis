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
        taskList = TaskManager.tm.RandomizeTasks();
        GetComponent<Player>().PHUD.OnInteraction += PressInteract;
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
        if (targetTask.TaskInfected)
        {
            GetComponent<Researcher>().SetInfected(true);
            playerView.RPC("RPC_DisinfectTask", RpcTarget.All);
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
}