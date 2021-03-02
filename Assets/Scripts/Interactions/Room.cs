using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private RoomObject roomObject;

    private string roomName;
    private string roomDescription;
    public Item.ItemType requiredSabotageItem;

    private Sabotage sabotage;
    private bool isSabotaged = false;

    private void Awake()
    {
        sabotage = GetComponentInChildren<Sabotage>();
        sabotage.gameObject.SetActive(false);
    }

    private void Start()
    {
        roomName = roomObject.roomName;
        roomDescription = roomObject.roomDescription;
        requiredSabotageItem = roomObject.requiredSabotageItem;

        GameManager.gm.OnStage2 += StartStage2;
    }

    public void Sabotage()
    {
        isSabotaged = true;
    }

    private void StartStage2()
    {
        if (isSabotaged)
        {
            sabotage.gameObject.SetActive(true);
        }
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
