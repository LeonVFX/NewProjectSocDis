using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureInfection : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;
    
    private bool canInfect = false;
    private Task targetTask;
    private bool isInteract = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        player.PHUD.OnInteraction += PressInteract;
    }

    void Update()
    {
        if (!playerView.IsMine)
            return;

        if (canInfect == true && TaskManager.tm.tasksInfected < TaskManager.tm.maxTasksInfected)
        {
            if (isInteract)
            {
                playerView.RPC("RPC_InfectTask", RpcTarget.All);
                player.PHUD.UpdateMessageLog($"{targetTask.TaskName} Infected!", Color.red);
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
