using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    [SerializeField] GameObject EscapePod1;
    public bool canEnter;
    public bool canEscape;
    public bool PlayerInPod;
    public bool launched;
    public float spaceleft = 2;
    public float countdown = 5;
    public float movement = 0;
    void Update()
    {
        if(canEnter == true)
        {
            if (Input.GetButtonDown("Interact"))
            {
                // GameManager.gm.InEscapePod();
                Debug.Log("Player has entered escape pod");
                spaceleft = spaceleft - 1;
            }
            if(spaceleft <= 0)
            {
                Debug.Log("Countdown has begun");
                countdown -= Time.deltaTime;
                if(countdown<=0)
                {
                    Debug.Log("Escape Pod 1 is lauching");
                    canEscape = true;
                }
            }
        }
        if(canEscape == true)
        {
            EscapePod1.transform.position = new Vector3(-10, movement++, 67);
            launched = true;
            GameManager.gm.NextStage();
        } 
    }
    private void OnTriggerEnter(Collider player)
    {
        if(player.tag == "Researcher")
        {
            PhotonView playerView = player.GetComponent<PhotonView>();
            Debug.Log("Player in reach of escape pod");
            canEnter = true;

        }
        if (player.tag == "Creature")
        {
            canEnter = true;
            Debug.Log("Creature can't use escape pod");
        }
    }
    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Researcher")
        {
            Debug.Log("Player out of range");
            canEnter = false;
        }
        if (player.tag == "Creature")
        {
            Debug.Log("Creature near escape pods");
        }
    }
}
