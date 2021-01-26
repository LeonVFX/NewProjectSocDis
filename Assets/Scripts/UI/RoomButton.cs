using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text nameText;
    public Text sizeText;

    public string roomName;
    public int roomSize;

    public void SetRoom(int playerCount)
    {
        nameText.text = roomName;
        sizeText.text = (playerCount != roomSize) ? playerCount.ToString() + " / " + roomSize.ToString() : "FULL";
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(roomName);
    }
}
