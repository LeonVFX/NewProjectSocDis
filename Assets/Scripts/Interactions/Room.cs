using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomObject roomObject;

    private string roomName;
    private string roomDescription;

    private void Start()
    {
        roomName = roomObject.roomName;
        roomDescription = roomObject.roomDescription;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect creature inside
    }

    private void OnTriggerExit(Collider other)
    {
        // Detect creature leave
    }
}
