using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour
{
    bool open = false;
    bool disable = false;
    bool broke = false;
    int change = 0;
    //float timer = 0;
    int stage = 0;

    //the door
    [SerializeField] GameObject Door;

    //the door button
    [SerializeField] GameObject Button;

    void Update()
    {
        //timer += Time.deltaTime;
        //once true the door will move
        if (open == true)
        {
            //After Interact used moves door away
            if (Input.GetButtonDown("Interact"))
            {
                //timer = 0;      
                change = change + 1;
            }
            if (change >= 1)
            {
                Door.SetActive(false);
                if(change >= 2)
                {
                    change = 0;
                    Door.SetActive(true);
                }
            }
            if(broke == true)
            {
                Door.SetActive(false);
            }
        }
    }


    private void OnTriggerEnter(Collider player)
    {
        if (player.tag == "Researcher")
        {
            open = true;
        }
        if (player.tag == "Creature")
        {
            open = true;
            if(stage >= 2)
            {
                disable = true;
            }
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Researcher")
        {
            open = false;
        }
        if (player.tag == "Creature")
        {
            open = false;
        }
    }
}
