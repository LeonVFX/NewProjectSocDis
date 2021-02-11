using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureInfection : MonoBehaviour
{
    private PhotonView playerView;
    
    private bool canInfect = false;
    private Task targetTask;
    public bool visualUpdate;
    public Text infectText;

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
                if(visualUpdate == true)
                {
                    infectText.text = "Task Infected!";
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D task)
    {
        if (task.tag == "Task")
        {            
            visualUpdate = true;
            targetTask = task.GetComponent<Task>();
            canInfect = true;
        }
    }

    private void OnTriggerExit2D(Collider2D task)
    {
        if (task.tag == "Task")
        {
            visualUpdate = false;
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
