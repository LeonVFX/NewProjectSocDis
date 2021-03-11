using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : MonoBehaviour
{
    [SerializeField] GameObject EscapePod1;
    public bool canEnter;
    public bool canEscape;
    public bool playerInPod;
    public bool launched;
    public float spaceleft = 2;
    public float countdown = 5;
    public float movement = 0;

    private void Update()
    {
        if (GameManager.gm.currentStage == GameManager.GameStage.Stage2)
        {
            if (canEnter == true)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    // GameManager.gm.InEscapePod();
                    Debug.Log("Player has entered escape pod");
                    playerInPod = true;
                    spaceleft--;
                }

                if (spaceleft <= 0)
                {
                    Debug.Log("Countdown has begun");
                    countdown -= Time.deltaTime;
                    if (countdown <= 0)
                    {
                        Debug.Log("Escape Pod 1 is lauching");
                        canEscape = true;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider player)
    {
        if(player.tag == "Researcher" || player.tag == "Creature")
        {
            PhotonView playerView = player.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            Debug.Log($"Player {player} in reach of escape pod");
            canEnter = true;
        }
    }

    private void OnTriggerExit(Collider player)
    {
        if (player.tag == "Researcher" || player.tag == "Creature")
        {
            PhotonView playerView = player.GetComponent<PhotonView>();

            if (!playerView.IsMine)
                return;

            Debug.Log($"Player {player} out of range");
            canEnter = false;
        }
    }
}

/* private void EscapeFacility(PhotonView playerView)
    {
        if (!playerView.IsMine)
            return;
        if (GameManager.gm.currentStage == GameManager.GameStage.Stage2)
        {
            if (canEnter == true)
            {
                if (Input.GetButtonDown("Interact"))
                {
                    // GameManager.gm.InEscapePod();
                    Debug.Log("Player has entered escape pod");
                    spaceleft = spaceleft - 1;
                }
                if (spaceleft <= 0)
                {
                    Debug.Log("Countdown has begun");
                    countdown -= Time.deltaTime;
                    if (countdown <= 0)
                    {
                        Debug.Log("Escape Pod 1 is lauching");
                        canEscape = true;
                    }
                }
            }
        }
        if (canEscape == true)
        {
            EscapePod1.transform.position = new Vector3(-10, movement++, 67);
            launched = true;
            GameManager.gm.NextStage();
        }
    }*/