using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSabotage : MonoBehaviour
{
    private PhotonView playerView;
    private Player player;

    private bool canSabotage;
    private Room targetRoom;
    private bool isInteract = false;

    private void Start()
    {
        playerView = GetComponent<PhotonView>();
        player = GetComponent<Player>();
        GetComponent<Player>().PHUD.OnInteraction += PressInteract;
    }

    private void Update()
    {
        if (!playerView.IsMine)
            return;

        if (canSabotage == true)
        {
            if (isInteract)
            {
                // Check if holding correct required item
                if (player.HeldItem != null)
                    if (player.HeldItem.itemType == targetRoom.requiredSabotageItem)
                    {
                        playerView.RPC("RPC_SabotageRoom", RpcTarget.All);
                        Debug.Log("Room Sabotaged");
                    }
            }
        }
    }

    private void OnTriggerEnter(Collider room)
    {
        if (room.tag == "Room")
        {
            targetRoom = room.GetComponent<Room>();
            canSabotage = true;
        }
    }

    private void OnTriggerExit(Collider room)
    {
        if (room.tag == "Room")
        {
            targetRoom = null;
            canSabotage = false;
        }
    }

    [PunRPC]
    private void RPC_SabotageRoom()
    {
        targetRoom.Sabotage();
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
