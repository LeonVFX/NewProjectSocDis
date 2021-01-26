using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private InputField playerNameInput = null;
    [SerializeField] private InputField roomNameInput = null;
    [SerializeField] private InputField roomSizeInput = null;
    [SerializeField] private Button reduceSize = null;
    [SerializeField] private Button increaseSize = null;
    [SerializeField] private Button createRoom = null;
    [SerializeField] private Button findRooms = null;

    private int roomSize = 8;
    [SerializeField] private int minRoomSize = 4;
    [SerializeField] private int maxRoomSize = 8;

    private void Start()
    {
        playerNameInput.text = PlayerPrefs.GetString("PlayerName");
        roomSizeInput.text = maxRoomSize.ToString();
    }

    private void Update()
    {
        // Main Menu Activation
        if (playerNameInput.text != string.Empty)
        {
            findRooms.interactable = true;
            if (roomNameInput.text != string.Empty)
                createRoom.interactable = true;
            else
                createRoom.interactable = false;
        }
        else
            findRooms.interactable = false;

        // Start Game Button Activation
        // TODO: Only available if more than 4 players in room
    }

    public void OnReduceSizeClick()
    {
        if (roomSize > minRoomSize)
        {
            roomSize--;
            roomSizeInput.text = roomSize.ToString();
        }
    }

    public void OnIncreaseSizeClick()
    {
        if (roomSize < maxRoomSize)
        {
            roomSize++;
            roomSizeInput.text = roomSize.ToString();
        }
    }

    public void OnClickSaveName()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInput.text);
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName");
    }
}
