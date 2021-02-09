using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInfection : MonoBehaviour
{
    private PhotonView playerView;
    
    private bool canInfect = false;
    private Task targetTask;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!playerView.IsMine)
            return;

        if (canInfect == true && TaskManager.tm.tasksInfected < TaskManager.tm.maxTasksInfected)
        {
            if (Input.GetButtonDown("Interact"))
            {
                playerView.RPC("RPC_InfectTask", RpcTarget.All);
                TaskManager.tm.tasksInfected = TaskManager.tm.tasksInfected + 1;
                Debug.Log("Task Infected");
            }
        }
    }

    private void OnTriggerEnter(Collider task)
    {
        if (task.tag == "Task")
        {
            targetTask = task.GetComponent<Task>();
            canInfect = true;
        }
    }

    private void OnTriggerExit(Collider task)
    {
        if (task.tag == "Task")
        {
            targetTask = null;
            canInfect = false;
        }
    }

    [PunRPC]
    private void RPC_InfectTask()
    {
        targetTask.SetInfected(true);
    }
}
